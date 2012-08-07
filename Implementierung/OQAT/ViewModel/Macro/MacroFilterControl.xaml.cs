using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Oqat.PublicRessources.Plugin;
using Oqat.PublicRessources.Model;
using System.Data;
using AC.AvalonControlsLibrary.Controls;
using System.Collections.ObjectModel;


namespace Oqat.ViewModel.Macro
{
    public partial class MacroFilterControl : UserControl
    {
        private PF_MacroFilter _macro;

        public PF_MacroFilter macro
        {
            get
            {
                return this._macro;
            }

            set
            {
                _macro = value;
            }
        }

        private VM_Macro _vmmacro;

        public VM_Macro vmmacro
        {
            get
            {
                return this._vmmacro;
            }
            set
            {
                _vmmacro = value;
            }
        }

        // The user may drag the mouse over the Macro Table itself instead of using a scrollbar, in which case ScrollViewer2 needs to be synchronised.
        // Also when a table entry gets deleted, ScrollViewer1 is automatically reset and ScrollViewer2 requires resynchronisation.
        public void scroll(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer2.ScrollToVerticalOffset(e.VerticalOffset);
        }

        public void scroll2(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer1.ScrollToVerticalOffset(e.VerticalOffset);
        }

        public MacroFilterControl(PF_MacroFilter macro, VM_Macro vmmacro)
        {
            this.macro = macro;
            this.vmmacro = vmmacro;
            InitializeComponent();
            DataTable macroEntryTable = macro.macroQueue;
            GridView gv = new GridView();
            foreach (DataColumn c in this.macro.macroQueue.Columns)
            {
                if (c.ColumnName != "Macro Entry")
                {
                    GridViewColumn gvColumn = new GridViewColumn();
                    if (c.ColumnName == "Start" || c.ColumnName == "Stop")
                    {
                        gvColumn.Width = 50;
                    }
                    else
                    {
                        gvColumn.Width = 200;
                    }
                    // TODO: disable column resizing!?
                    gvColumn.DisplayMemberBinding = new Binding(c.ColumnName);
                    gvColumn.Header = c.ColumnName;
                    gv.Columns.Add(gvColumn);
                }
            }
            macroTable.View = gv;
            macroTable.DataContext = this.macro.macroQueue;
            Binding bind = new Binding();
            macroTable.SetBinding(ListView.ItemsSourceProperty, bind);
            GridView gvs = new GridView();
            GridViewColumn gvsColumn = new GridViewColumn();
            gvsColumn.Header = "Frames Relative";
            gvs.Columns.Add(gvsColumn);
            rangeSliders.View = gvs;
            rangeSliders.DataContext = this.macro.rsl;
            Binding bind2 = new Binding();
            rangeSliders.SetBinding(ListView.ItemsSourceProperty, bind2);
            this.DataContext = this.macro;
            macroTable.Drop += new DragEventHandler(dragdrop);
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            VideoEventArgs ea = new VideoEventArgs(null, false); // TODO: set vidResult through file explorer
            macroTable.IsEnabled = false;
            rangeSliders.IsEnabled = false;
            vmmacro.onStartProcess(this, ea);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            MacroSaveDialog msd = new MacroSaveDialog();
            msd.vmmacro = vmmacro;
            msd.Visibility = System.Windows.Visibility.Visible;
        }
        

        private void dragdrop(object sender, DragEventArgs e)
        {
            // get mementoeventargs from sender somehow
            MementoEventArgs ea = null;
            vmmacro.onEntrySelect(this, ea);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            while (macroTable.SelectedIndex != -1)
            {
                int index = macroTable.SelectedIndex;
                this.macro.rsl.RemoveAt(index);
                List<RangeSelectionChangedEventHandler> tempList = new List<RangeSelectionChangedEventHandler>();
                for (int i = index; i < this.macro.rsl.Count; i++)
                {
                    foreach (RangeSelectionChangedEventHandler ev in this.vmmacro.delList)
                    {
                        this.macro.rsl[i].RangeSelectionChanged -= ev;
                    }
                    // this.macro.rsl[i].RangeSelectionChanged -= this.vmmacro.delList[i]; I have no idea why this doesn't work
                    RangeSelectionChangedEventHandler del;
                    int j = i;
                    del = delegate(object sender2, RangeSelectionChangedEventArgs e2)
                    {
                        MacroEntryFilter mfeTemp = new MacroEntryFilter();
                        mfeTemp = (MacroEntryFilter)this.macro.macroQueue.Rows[j]["Macro Entry"];
                        mfeTemp.startFrameRelative = ((double)e2.NewRangeStart / 500) * 100;
                        mfeTemp.endFrameRelative = ((double)e2.NewRangeStop / 500) * 100;
                        if (mfeTemp.startFrameRelative > 100) mfeTemp.startFrameRelative = 100; // bugfix rangeslider
                        if (mfeTemp.endFrameRelative < 0) mfeTemp.endFrameRelative = 0; // bugfix rangeslider
                        this.macro.macroQueue.Rows[j]["Macro Entry"] = mfeTemp;
                        this.macro.macroQueue.Rows[j]["Start"] = mfeTemp.startFrameRelative;
                        this.macro.macroQueue.Rows[j]["Stop"] = mfeTemp.endFrameRelative;
                    };
                    this.macro.rsl[i].RangeSelectionChanged += del;
                    tempList.Add(del);
                }

                for (int i = index; i < this.macro.rsl.Count; i++)
                {
                    this.vmmacro.delList[i] = null;
                    this.vmmacro.delList[i] += tempList[i - index];
                }
                GridView gvs = new GridView(); // since we can't figure out a way to set the DisplayMemberBinding of the GridViewColumn
                GridViewColumn gvsColumn = new GridViewColumn(); // in a way that the slider is visible, we rebuild the GridViewColumn after we delete an entry
                gvsColumn.Header = "Frames Relative";
                gvs.Columns.Add(gvsColumn);
                rangeSliders.View = gvs;
                rangeSliders.DataContext = this.macro.rsl;
                Binding bind2 = new Binding();
                rangeSliders.SetBinding(ListView.ItemsSourceProperty, bind2);
                this.macro.macroQueue.Rows.RemoveAt(index);
            }           
        }
    }
}
