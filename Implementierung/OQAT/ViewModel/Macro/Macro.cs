using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;
using Oqat.Model;
using Oqat.PublicRessources.Model;
using Oqat.PublicRessources.Plugin;
using System.Xml;
using System.Threading;
using System.IO;
using System.Windows;

namespace Oqat.ViewModel.MacroPlugin
{
    [ExportMetadata("namePlugin", "Macro")]
    [ExportMetadata("type", PluginType.IMacro)]
    [ExportMetadata("threadSafe", false)]
    [Export(typeof(IPlugin))]

    class Macro : IMacro
    {

        /// <summary>
        /// This is the TopLevel macro. All processing
        /// is happening inside this macro, 
        /// </summary>
        private MacroEntry rootEntry
        { get; set; }

        #region macro
        public Macro()
        {
            // init first entry
            this.rootEntry = new MacroEntry(this.namePlugin, PluginType.IMacro, "");
            this.rootEntry.frameCount = 100;

            MacroViewDelegates macroViewDelegates = new MacroViewDelegates(addMacroEntry, moveMacroEntry,
                                                    removeMacroEntry, constructMacroFromMementoArg,
                                                    startProcessing, cancelProcessing, clearMacroEntryList, saveSaveAsHelper,
                                                    registerOnMacroEventsHelper);
            
            propertyView = new Macro_PropertyView(this.rootEntry, macroViewDelegates);
            registeredLock = new Object();
            
            _propertyView.readOnly = false;
            from =  " von ";
            framesProcessed = " Bilder verarbeitet.";
            local("VM_Macro_" + Thread.CurrentThread.CurrentCulture + ".xml");

        
        }

        private int registered = 0;
        private object registeredLock;
        private void registerOnMacroEventsHelper() {
            lock(registeredLock) {
            if (!_propertyView.readOnly && registered < 1)
            {
               
                PluginManager.macroEntryAdd += this.addMacroEntry;
                PluginManager.OqatToggleView += onToggleView;
                PluginManager.setMacroMemento += setMemento;
                registered++;
            }
            else if(_propertyView.readOnly && registered > 0)
            {
                PluginManager.macroEntryAdd -= this.addMacroEntry;
                PluginManager.OqatToggleView -= onToggleView;
                PluginManager.setMacroMemento -= setMemento;
                registered--;
            }
        }
        }

#region fieldsProperties
        [field: NonSerialized]
        private Macro_PropertyView _propertyView;

        private ViewType viewType;

        public UserControl propertyView
        {
            get
            {
                return _propertyView;
            }
            private set
            {
                _propertyView = value as Macro_PropertyView;
            }
        }

        public bool threadSafe
        {
            get { return false; }
        }

        string processCancelled_MsgBox_Text = "Möchten sie das bis jetzt erstellte video behalten, dann klicken Sie auf \"Ja\"." +
            " Klicken Sie auf \"Nein\", falls Sie das Video verwerfen möchten.?";
        string caption_processedCancelled_MsgBox = " Macro prozess abgebrochen.";
#endregion



        private void filterProcessCompleted(object s, RunWorkerCompletedEventArgs e)
        {
            _propertyView.processing = false;
            MessageBoxResult result = MessageBoxResult.Yes;
            if (e.Cancelled == true)
            {
                MessageBoxButton button = MessageBoxButton.YesNo;
                MessageBoxImage icon = MessageBoxImage.Warning;

                result = MessageBox.Show(processCancelled_MsgBox_Text, 
                    caption_processedCancelled_MsgBox, button, icon);

            }


            if (e.Error != null)
            {
                //error + e.Error.Message;
            }
            else if (result == MessageBoxResult.Yes)
            {
                vidRes.vidInfo.frameCount = -1;
                PluginManager.pluginManager.raiseEvent(
                EventType.macroProcessingFinished, new VideoEventArgs(this.vidRes, this.idRes));
            }
            else
            {
                if (File.Exists(vidRes.vidPath))
                    File.Delete(vidRes.vidPath);
            }
        }

        private void metricProcessCompleted(object s, RunWorkerCompletedEventArgs e)
        {
            _propertyView.processing = false;
            MessageBoxResult result = MessageBoxResult.Yes;
            if (((procFinishedResult)e.Result).cancelled == true)
            {
                MessageBoxButton button = MessageBoxButton.YesNo;
                MessageBoxImage icon = MessageBoxImage.Warning;
                result = MessageBox.Show(processCancelled_MsgBox_Text,
                    caption_processedCancelled_MsgBox, button, icon);
            }

            if (e.Error != null)
            {
                //error + e.Error.Message;
            }
            else if (result == MessageBoxResult.Yes)
            {

                if(!(((procFinishedResult)e.Result).seqMetricResultCtxList is List<metricResultContext>))
                 throw new ArgumentException("Result args are not of type procFinishedResult.");

                foreach (var subEntry in ((procFinishedResult)e.Result).seqMetricResultCtxList)
                {
                    subEntry.vidRes.handler.flushReader();
                    subEntry.vidRes.handler.flushWriter();
                    (subEntry.vidRes as Video).handler = null;
                    // if video wasnt constructed till the last frame  (user cancelled)
                    // this will trigger a reinitialization of the frameCount property
                    subEntry.vidRes.vidInfo.frameCount = -1;
                    if (((procFinishedResult)e.Result).cancelled)
                    {
                        var newFrameMetricValues = subEntry.vidRes.frameMetricValue;
                        Array.Resize(ref newFrameMetricValues, subEntry.vidRes.vidInfo.frameCount);

                     //  need Video here cause IVideo does not provide a setter for this property
                       (subEntry.vidRes as Video).frameMetricValue = newFrameMetricValues;

                    }
                   //     (metResContext.vidRes as Video).frameMetricValue = new float[metResContext.vidRes.vidInfo.frameCount][];
                   

                    PluginManager.pluginManager.raiseEvent(
                        EventType.macroProcessingFinished, new VideoEventArgs(subEntry.vidRes, this.idRes));
                }
            }
            else
            {//user clicked No -> delete all files
                foreach (var subEntry in ((procFinishedResult)e.Result).seqMetricResultCtxList)
                {
                    if (File.Exists(subEntry.vidRes.vidInfo.path))
                        File.Delete(subEntry.vidRes.vidInfo.path);
                    subEntry.vidRes.handler.flushReader();
                    subEntry.vidRes.handler.flushWriter();
                    // if video wasnt constructed till the last frame  (user cancelled)
                    // this will trigger a reinitialization of the frameCount property
                    vidRes.vidInfo.frameCount = -1;
                    (subEntry.vidRes as Video).handler = null;
                }
            }
        }

        private void filterProcess(object s, DoWorkEventArgs e)
        {
            // construct result video and init handler writing context
            var tmpVidInfo = handRef.readVidInfo.Clone() as IVideoInfo;
            string ext = this.rootEntry.mementoName;
            if (ext == "" && rootEntry.macroEntries.Count == 1)
            {
                ext = rootEntry.macroEntries[0].mementoName;
            }
            string newPath = findValidVidPath(handRef.readPath, ext);
            tmpVidInfo.path = newPath;
            tmpVidInfo.frameCount = handRef.readVidInfo.frameCount;
            Video vidRes = new Video(isAnalysis: false,
                vidPath: newPath,
                vidInfo: tmpVidInfo);

            this.vidRes = vidRes;
            handRef.setWriteContext(vidRes.vidPath, vidRes.vidInfo);




            List<MacroEntry> seqMacroEntryList = new List<MacroEntry>();
            recursiveFilterExplorer(this.rootEntry, seqMacroEntryList);
            handRef.positionReader = 0;
            while (handRef.positionReader < handRef.readVidInfo.frameCount)
            {
                Bitmap bmp = handRef.getFrame();

                foreach (var filterEntry in seqMacroEntryList)
                {

                    if ((filterEntry.startFrameAbs <= (handRef.positionReader - 1))
                        && (filterEntry.endFrameAbs >= (handRef.positionReader - 1)))
                    {
                        try
                        {
                            IFilterOqat actPlugin = PluginManager.pluginManager.getPlugin<IFilterOqat>(filterEntry.pluginName)
                                as IFilterOqat;
                            actPlugin = actPlugin.createExtraPluginInstance() as IFilterOqat;
                            actPlugin.setMemento(PluginManager.pluginManager.getMemento(
                                filterEntry.pluginName, filterEntry.mementoName));
                            bmp = actPlugin.process(bmp);

                        }
                        catch (Exception ex)
                        {
                            PluginManager.pluginManager.raiseEvent(EventType.failure, new ErrorEventArgs(ex));
                            //TODO
                            // set plugin to blackList if something went wrong
                        }
                    }
                }
                var tmpBmpArray = new Bitmap[1];
                tmpBmpArray[0] = bmp;
                handRef.writeFrames(handRef.positionReader - 1, tmpBmpArray);

                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    worker.ReportProgress((int)(handRef.positionReader / (rootEntry.frameCount / 100.0)));
                }
            }






        }


        private void recursiveFilterExplorer(MacroEntry entry, List<MacroEntry> seqMacroList)
        {
            Debug.Assert(seqMacroList != null, "seqMacroList is null");
            foreach (var subEntry in entry.macroEntries)
            {
                if (subEntry.type != PluginType.IMacro)
                {
                    var newEntry = new MacroEntry(subEntry.pluginName, subEntry.type, subEntry.mementoName);

             
                    newEntry._endFrameRelative = subEntry._endFrameRelative;
                    newEntry._startFrameRelative = subEntry._startFrameRelative;
                    newEntry.frameCount = subEntry.frameCount;
                    newEntry.mementoName = subEntry.mementoName;
                    seqMacroList.Add(newEntry);
                }
                else
                {
                    // go down to first found filter
                    recursiveFilterExplorer(subEntry, seqMacroList);
                }
            }
        }

        private void metricProcess(object s, DoWorkEventArgs e)
        {

            List<metricResultContext> seqMetricResultCtxList = new List<metricResultContext>();

            recursiveMetricExplorer(this.rootEntry, seqMetricResultCtxList);
            handRef.positionReader = 0;
            handProc.positionReader = 0;
            while (handRef.positionReader < handRef.readVidInfo.frameCount)
            {
                Bitmap bmpRef = handRef.getFrame();
                Bitmap bmpProc = handProc.getFrame();

                foreach (var subEntry in seqMetricResultCtxList)
                {
                    // check if set as active
                    if ((subEntry.entry.startFrameAbs <= (handRef.positionReader - 1))
                        && (subEntry.entry.endFrameAbs > handRef.positionReader - 1))
                    {
                        // init plugin
                        IMetricOqat curPlugin =
                            PluginManager.pluginManager.getPlugin<IMetricOqat>(
                            subEntry.entry.pluginName).createExtraPluginInstance() as IMetricOqat;

                        curPlugin.setMemento(PluginManager.pluginManager.getMemento(
                            subEntry.entry.pluginName, subEntry.entry.mementoName));

                        var info = curPlugin.analyse(bmpRef, bmpProc);

                        // write acquired frame
                        (subEntry.vidRes as Video).frameMetricValue[handRef.positionReader - 1 - subEntry.entry.startFrameAbs] = info.values;
                        var tmpArray = new Bitmap[1];
                        tmpArray[0] = info.frame;
                        subEntry.handRes.writeFrames(handRef.positionReader - 1 - (int)subEntry.entry.startFrameAbs, tmpArray);

                    }
                }
                if (worker.CancellationPending)
                {
                    break;
                }
                else
                {
                    worker.ReportProgress((int)(handRef.positionReader /( rootEntry.frameCount / 100.0)));
                }

            }
            procFinishedResult prFinishResult = new procFinishedResult();
            prFinishResult.cancelled = worker.CancellationPending;
            prFinishResult.seqMetricResultCtxList = seqMetricResultCtxList;

            e.Result = prFinishResult;
        }
        struct procFinishedResult
        {
           public bool cancelled;
           public List<metricResultContext> seqMetricResultCtxList;
        }

        struct metricResultContext
        {
            public IVideo vidRes;
            public IVideoHandler handRes;
            public MacroEntry entry;
        }

        private void recursiveMetricExplorer(MacroEntry entry, List<metricResultContext> seqMacroEntryList)
        {
            if(seqMacroEntryList == null)
                throw new ArgumentNullException("Given seqMacroEntryList is null.");

            foreach (var subEntry in entry.macroEntries)
            {
                if (subEntry.type != PluginType.IMacro)
                {
                    metricResultContext metResContext;

                    string path;



                    if (subEntry.path != null)
                        path = subEntry.path;
                    else
                    {
                        path = findValidVidPath(handProc.readPath, subEntry.pluginName);

                    }


                    var tmpEntryList = new List<IMacroEntry>();
                    var newEntry = new MacroEntry(subEntry.pluginName, subEntry.type, subEntry.mementoName);
                   
                    metResContext.entry = newEntry;
                    metResContext.entry._endFrameRelative = subEntry._endFrameRelative;
                    metResContext.entry._startFrameRelative = subEntry._startFrameRelative;
                    metResContext.entry.frameCount = subEntry.frameCount;
                    metResContext.entry.mementoName = subEntry.mementoName;
                   
                    tmpEntryList.Add(newEntry);

                    var tmpVidInfo = handProc.readVidInfo.Clone() as IVideoInfo;

                    tmpVidInfo.path = path;

                    tmpVidInfo.frameCount = (int)(subEntry.endFrameAbs - subEntry.startFrameAbs);

                    metResContext.vidRes = new Video(isAnalysis: true,
                                                    vidPath: path,
                                                    vidInfo: tmpVidInfo,
                                                    processedBy: tmpEntryList);

                    (metResContext.vidRes as Video).frameMetricValue = new float[metResContext.vidRes.vidInfo.frameCount][];
                    metResContext.handRes = metResContext.vidRes.getExtraHandler();
                    metResContext.handRes.setWriteContext(metResContext.vidRes.vidPath, metResContext.vidRes.vidInfo);
                    seqMacroEntryList.Add(metResContext);
                }
                else
                {
                    // go down to first found non macro
                    recursiveMetricExplorer(subEntry, seqMacroEntryList);
                }
            }

        }

        private string findValidVidPath(string possiblePath, string suffix)
        {
            string path = "";


            string ext = Path.GetExtension(possiblePath);
            path = Path.GetDirectoryName(possiblePath) + "\\" + Path.GetFileNameWithoutExtension(possiblePath);
            bool pathValid = false;
            int n = 0;
            string counter = "";
            while (!pathValid)
            {
                if (n > 0)
                    counter = n.ToString();

                string tmpPath = path + suffix + counter + ext;
                if (!File.Exists(tmpPath))
                {
                    pathValid = true;
                    path = tmpPath;
                }
                else
                    n++;

            }
            return path;
        }

        public void flush()
        {
            if (handProc != null)
            {
                handProc.flushReader();
                handProc.flushWriter();
                handProc = null;
            }

            if (handRef != null)
            {
                handRef.flushReader();
                handRef.flushWriter();
                handRef = null;
            }
            clearMacroEntryList();
            this.rootEntry.mementoName = "";
            this.rootEntry.frameCount = 100;
        }

        public IPlugin createExtraPluginInstance()
        {
            return new Macro();
        }


        public void setMemento(object sender, MementoEventArgs e)
        {
            setMemento(e.memento);
        }
    

        public void setMemento(PublicRessources.Model.Memento memento)
        {
            if (memento == null)
                throw new ArgumentNullException("Given memento is null.");

             var newTLMacroEnry = memento.state as MacroEntry;


            if (newTLMacroEnry == null)
                throw new ArgumentNullException("Given state object is null.");


            if (newTLMacroEnry.mementoName != memento.name)
                throw new ArgumentException("Given memento shows inconsistencies. Name of top level macro does not equal to" +
                                            " the memento name.");


            //flush();
            originallTlMacroName = newTLMacroEnry.mementoName;
            addMacroEntry(newTLMacroEnry, null);
           
        }

        private string originallTlMacroName = "";

        public virtual string namePlugin
        {
            get { return "Macro"; }
        }

        public virtual PluginType type
        {
            get { return PluginType.IMacro; }
        }

        // this will not only return a readOnlyView but actually
        // set the (only one view exists at a time) view to readOnly mode
        public UserControl readOnlyPropertiesView
        {
            get
            {
                _propertyView.readOnly = true;

                return this.propertyView;
            }
        }

        private IVideoHandler handRef;
        private IVideoHandler handProc;
        private Video vidRes;
        private int idRes;

        private BackgroundWorker worker;

        public void setFilterContext(int idRef, IVideo vidRef)
        {
            //do not set new context while macro is processing
            if (_propertyView.processing)
                return;


            flush();

            this.idRes = idRef;
            handRef = vidRef.getExtraHandler();

            (propertyView as Macro_PropertyView).filterMode = true;

            this.rootEntry.frameCount = handRef.readVidInfo.frameCount;

            //RemoveClickEvent((propertyView as Macro_PropertyView).startProcessing);

        }

        public void setMetricContext(IVideo vidRef, int idProc, IVideo vidProc)
        {
            //do not set new context while macro is processing
            if (_propertyView.processing)
                return;



            flush();

            this.idRes = idProc;
            if (vidRef != null)
            {
                handRef = vidRef.getExtraHandler();

                this.rootEntry.frameCount = handRef.readVidInfo.frameCount;
            }
            if (vidProc != null)
            {
                handProc = vidProc.getExtraHandler();
                this.rootEntry.frameCount = handProc.readVidInfo.frameCount;
            }


            (propertyView as Macro_PropertyView).filterMode = false;
        }


       // delegate void removeMacroEntry(MacroEntry entry);

        public void addMacroEntry(object sender, MementoEventArgs e)
        {
            addMacroEntry(e, rootEntry);
        }
        private void addMacroEntry(MementoEventArgs e, MacroEntry father, int index = -1)
        {
            var entryToAdd = constructMacroFromMementoArg(e);


            if ((entryToAdd.type == PluginType.IMacro) && (this.rootEntry.macroEntries.Count() == 0))
                addMacroEntry(entryToAdd, null);

            else
                addMacroEntry(entryToAdd, father, index);
        }
        private void removeMacroEntry(MacroEntry entry)
        {

            lock (this.rootEntry)
            {
                if (rootEntry.macroEntries.Contains(entry))
                {
                    rootEntry.macroEntries.Remove(entry);

                }
                else
                {
                    List<MacroEntry> seqMacroEntryList = new List<MacroEntry>();
                    recursiveFilterExplorer(this.rootEntry, seqMacroEntryList);

                    foreach (var listEntry in seqMacroEntryList)
                    {
                        if (listEntry.macroEntries.Contains(entry))
                        {
                            listEntry.macroEntries.Remove(entry);
                            break;
                        }
                    }
                }
            }
        }
        private void addMacroEntry(MacroEntry child, MacroEntry father, int index = -1)
        {

            lock (this.rootEntry)
            {
                if (this.handRef != null)
                    child.frameCount = handRef.readVidInfo.frameCount;

                if (father == null) // child is new TL macro
                {
                    rootEntry.mementoName = child.mementoName;
                    originallTlMacroName = rootEntry.mementoName;
                    _propertyView.rootEntryMem_TextBox.Text = rootEntry.mementoName;
                    clearMacroEntryList();
                    concatObsCollInplace(rootEntry.macroEntries, child.macroEntries);
                }
                else
                {
                    if (index < 0)
                    {
                        father.macroEntries.Add(child);
                    }
                    else
                    {
                        father.macroEntries.Insert(index, child);
                    }
                }
            }
            // triggers reinitialization
            this._propertyView.readOnly = this._propertyView.readOnly;
            
        }
        private void concatObsCollInplace(ObservableCollection<MacroEntry> first, ObservableCollection<MacroEntry> second)
        {
            foreach (var entry in second)
            {
                first.Add(entry);
            }
        }

        private void moveMacroEntry(MacroEntry toMoveMacro, MacroEntry target, int index = -1)
        {
            removeMacroEntry(toMoveMacro);

            addMacroEntry(toMoveMacro, target, index);

        }

        // delete the items (NOT the collection itself)
        private void clearMacroEntryList() { if (rootEntry.macroEntries != null) rootEntry.macroEntries.Clear(); else rootEntry.macroEntries = new ObservableCollection<MacroEntry>(); }
        private void cancelProcessing() {

            if (worker != null)
                worker.CancelAsync();
        }
        private void pauseProcessing() { }

        string from ;
        string framesProcessed; 

        private void startProcessing() 
        {
            if (this.handRef == null)
                throw new ContextNotSetException("No Video loaded.");

            _propertyView.processingStateValue = 0;
            _propertyView.processing = true;


            worker = new BackgroundWorker();

            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;

            if (_propertyView.filterMode)
            {
                worker.DoWork += filterProcess;
                worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(filterProcessCompleted);
            }
            else
            {
                worker.DoWork += metricProcess;
                worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(metricProcessCompleted);

            }

            worker.ProgressChanged += delegate(object s, ProgressChangedEventArgs args)
            {
                Debug.Assert(_propertyView.MacroEntryTreeView.Dispatcher.CheckAccess());
                _propertyView.MacroEntryTreeView.Dispatcher.VerifyAccess();

                _propertyView.processingStateValue = args.ProgressPercentage;
                _propertyView.processingStateMessage = +this.handRef.positionReader + from + this.rootEntry.frameCount + framesProcessed;

            };


            worker.RunWorkerAsync();
        }
       
        #endregion
        private void onToggleView(object sender, ViewTypeEventArgs e)
        {
            //do not switch display of this macro object if it is processing
            if (_propertyView.processing)
                return;

            if (e.viewType == viewType)
                return;

            viewType = e.viewType;
            switch (viewType)
            {
                case ViewType.FilterView:
                    _propertyView.activeState = true;
                    _propertyView.filterMode = true;
                    flush();
                    break;
                case ViewType.MetricView:
                    _propertyView.activeState = true;
                    _propertyView.filterMode = false;
                    flush();
                    break;    
                default:
                    _propertyView.activeState = false;
                    break;
            }
        }


        private void saveSaveAsHelper(EventType saveType)
        {
            if (originallTlMacroName.Equals(rootEntry.mementoName) || (saveType == EventType.saveMacroAs))
                originallTlMacroName = "";

            PluginManager.pluginManager.raiseEvent(saveType,
                new MementoEventArgs(this.rootEntry.mementoName, this.namePlugin, originallTlMacroName, getMemento));
            
            originallTlMacroName = rootEntry.mementoName;
        }

        public Memento getMemento()
        {
            Memento memToReturn;
            if (originallTlMacroName.Equals(rootEntry.mementoName))
                originallTlMacroName = "";

            MacroEntry rootEntryCopy = new MacroEntry(rootEntry.pluginName, rootEntry.type, rootEntry.mementoName);

            rootEntryCopy.startFrameRelative = rootEntry.startFrameRelative;
            rootEntryCopy.endFrameRelative = rootEntry.endFrameRelative;
            rootEntryCopy.path = rootEntry.path;

            doDeepCopy(rootEntry, ref rootEntryCopy);
            if (rootEntryCopy.mementoName.Equals(""))
                memToReturn = new Memento(rootEntryCopy.mementoName, new object());
            else if (rootEntryCopy.macroEntries.Count > 0)
                memToReturn = new Memento(rootEntryCopy.mementoName, rootEntryCopy as IMacroEntry);
            else
                memToReturn = new Memento(rootEntryCopy.mementoName, null);

            return memToReturn;
        }

        private void doDeepCopy(MacroEntry entryToCopy, ref MacroEntry copy)
        {
          
            foreach (var entry in entryToCopy.macroEntries)
            {
                if(entry.macroEntries != null) {

                    var macroEntryCopy = new MacroEntry(entry.pluginName, entry.type, entry.mementoName);
                    macroEntryCopy.startFrameRelative = entry.startFrameRelative;
                    macroEntryCopy.endFrameRelative = entry.endFrameRelative;
                    macroEntryCopy.path = entry.path;
                
                    
                 
                    copy.macroEntries.Add(macroEntryCopy);
                   
                    if (entry.macroEntries.Count() > 0)
                    {
                        macroEntryCopy.macroEntries = new ObservableCollection<MacroEntry>();
                        doDeepCopy(entry, ref macroEntryCopy);
                    }
                }
            }
      
        }

        private MacroEntry constructMacroFromMementoArg(MementoEventArgs e)
        {
            IPlugin plToConstrFrom = PluginManager.pluginManager.getPlugin<IPlugin>(e.pluginKey);
            if (plToConstrFrom == null)
                throw new ArgumentException("Given plugin name either does not refer to a existing plugin or is blacklisted.");
       
            PluginType entryType = PluginManager.pluginManager.getPlugin<IPlugin>(e.pluginKey).type;
            MacroEntry entryToAdd = null;
            if (entryType != PluginType.IMacro)
            {
                entryToAdd = new MacroEntry(e.pluginKey, entryType, e.mementoName);
                entryToAdd.frameCount = this.rootEntry.frameCount;       
            }
            else
            {
                var tmpMem = PluginManager.pluginManager.getMemento(e.pluginKey, e.mementoName);
                if (tmpMem == null)
                    throw new ArgumentException("Given parameters do not refer to a existing memento.");
                if(tmpMem.state != rootEntry)
                    entryToAdd = tmpMem.state as MacroEntry;
            }

            return entryToAdd;
        }
        public void local(String s)
        {
            try
            {
                String sFilename = Directory.GetCurrentDirectory() + "/" + s;
                XmlTextReader reader = new XmlTextReader(sFilename);
                reader.Read();
                reader.Read();
                int count = 11;
                String[] t = new String[count];
                String[] t2 = new String[count];
                for (int i = 0; i < count; i++)
                {
                    reader.Read();
                    reader.Read();
                    t[i] = reader.Name;
                    reader.MoveToNextAttribute();
                    t2[i] = reader.Value;
                    if (t2[i] == "")
                    {
                        throw new XmlException("datei nicht lang genug");
                    }
                }
                from = t2[7];
                framesProcessed = t2[8];
                processCancelled_MsgBox_Text =t2[9];
                caption_processedCancelled_MsgBox = t2[10];
            }
            catch (IndexOutOfRangeException e) { }
            catch (FileNotFoundException e) { }
            catch (XmlException e) { }
        }
     
    }


    public class MacroViewDelegates
    {
       public MacroViewDelegates(AddMacroEntry_Delegate addMacro, 
            MoveMacroEntry_Delegate moveMacro, 
            RemoveMacroEntry_Delegate removeMacro,
            ConstructMacroFromMementoArgs_Delegate constrMacroFromMem, 
            ParamLess_Delegate startProcessing, 
            ParamLess_Delegate cancelProcessing,
            ParamLess_Delegate clearEntries,
            saveSaveAs_Delegate saveSaveAs,
           ParamLess_Delegate registerOnMacroEvents)
        {
            #region nullchecks
            if (addMacro == null ||
                moveMacro == null ||
                removeMacro == null ||
                constrMacroFromMem == null ||
                startProcessing == null ||
                cancelProcessing == null ||
                clearEntries == null ||
                saveSaveAs == null ||
                registerOnMacroEvents == null)
                throw new ArgumentException("Not all given delegates were initialized properly");
            #endregion

            this.addMacro = addMacro;
            this.moveMacro = moveMacro;
            this.removeMacro = removeMacro;
            this.constrMacroFromMem = constrMacroFromMem;
            this.startProcessing = startProcessing;
            this.cancelProcessing = cancelProcessing;
            this.clearEntries = clearEntries;
            this.saveSaveAs = saveSaveAs;
            this.registerOnMacroEvents = registerOnMacroEvents;
        }

        public AddMacroEntry_Delegate addMacro;
        public saveSaveAs_Delegate saveSaveAs;
        public MoveMacroEntry_Delegate moveMacro;
        public RemoveMacroEntry_Delegate removeMacro;
        public ConstructMacroFromMementoArgs_Delegate constrMacroFromMem;
        public ParamLess_Delegate startProcessing;
        public ParamLess_Delegate cancelProcessing;
        public ParamLess_Delegate clearEntries;
        public ParamLess_Delegate registerOnMacroEvents;

        public delegate void AddMacroEntry_Delegate(MacroEntry child, MacroEntry father, int index = - 1);
        public delegate void MoveMacroEntry_Delegate(MacroEntry toMoveMacro, MacroEntry target, int index = -1);
        public delegate void RemoveMacroEntry_Delegate(MacroEntry entry);
        public delegate MacroEntry ConstructMacroFromMementoArgs_Delegate(MementoEventArgs e);
        public delegate void ParamLess_Delegate();
        public delegate void saveSaveAs_Delegate(EventType saveType);
    }


    public class ContextNotSetException : Exception
    {
        public ContextNotSetException(string msg)
            : base(msg)
        {
            
        }
    }
}
