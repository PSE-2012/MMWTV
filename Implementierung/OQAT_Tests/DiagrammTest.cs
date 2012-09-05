using PP_Diagramm;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Oqat.Model;
using Oqat.PublicRessources.Model;
using Oqat.PublicRessources.Plugin;
using System.Windows.Controls;
using Oqat.ViewModel.MacroPlugin;

namespace OQAT_Tests
{
    
    
    /// <summary>
    ///Dies ist eine Testklasse für "DiagrammTest" und soll
    ///alle DiagrammTest Komponententests enthalten.
    ///</summary>
    [TestClass()]
    public class DiagrammTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Ruft den Testkontext auf, der Informationen
        ///über und Funktionalität für den aktuellen Testlauf bietet, oder legt diesen fest.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Zusätzliche Testattribute
        // 
        //Sie können beim Verfassen Ihrer Tests die folgenden zusätzlichen Attribute verwenden:
        //
        //Mit ClassInitialize führen Sie Code aus, bevor Sie den ersten Test in der Klasse ausführen.
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Mit ClassCleanup führen Sie Code aus, nachdem alle Tests in einer Klasse ausgeführt wurden.
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Mit TestInitialize können Sie vor jedem einzelnen Test Code ausführen.
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Mit TestCleanup können Sie nach jedem einzelnen Test Code ausführen.
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion



        /// <summary>
        ///Test flushing a new Diagram
        ///</summary>
        [TestMethod()]
        public void FlushTest_new()
        {
            Diagramm target = new Diagramm();
            target.flush();

            //no Exception should occur
        }

        /// <summary>
        ///Test "createExtraPluginInstance"
        ///</summary>
        [TestMethod()]
        public void CloneTest()
        {
            Diagramm target = new Diagramm();
            object actual;
            actual = target.createExtraPluginInstance();

            Assert.AreNotSame(target, actual, "Created diagram instance is same object as origin.");
        }

        /// <summary>
        ///Test "Clone"
        ///</summary>
        [TestMethod()]
        public void CloneTest1()
        {
            Diagramm target = new Diagramm();
            object actual;
            actual = target.Clone();

            Assert.AreNotSame(target, actual, "Cloned diagram is same object as origin.");
        }

        /// <summary>
        ///Test "setVideo": null
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException),
            "video null was inappropriately allowed.")]
        public void setVideoTest_null()
        {
            Diagramm target = new Diagramm();
            IVideo video = null;
            target.setVideo(video);
        }

        /// <summary>
        ///Test "setVideo": null data
        ///</summary>
        [TestMethod()]
        public void setVideoTest_nulldata()
        {
            Diagramm target = new Diagramm();
            bool isAnalysis = true;
            string vidPath = "";
            IVideoInfo vinfo = null;
            List<IMacroEntry> processedBy = new List<IMacroEntry>();
            MacroEntry macroEntry = new Oqat.ViewModel.MacroPlugin.MacroEntry("TestPlugin", PluginType.IMetricOqat, "TestMemento");
            processedBy.Add(macroEntry);
            Video video = new Video(isAnalysis, vidPath, vinfo, processedBy);

            target.setVideo(video);
            //assert: no exception
        }

        /// <summary>
        ///Test "setVideo": empty data
        ///</summary>
        [TestMethod()]
        public void setVideoTest_emptyData()
        {
            Diagramm target = new Diagramm();
            bool isAnalysis = true;
            string vidPath = "";
            IVideoInfo vinfo = null;
            List<IMacroEntry> processedBy = new List<IMacroEntry>();
            MacroEntry macroEntry = new MacroEntry("TestPlugin", PluginType.IMetricOqat, "TestMemento");
            processedBy.Add(macroEntry);
            Video video = new Video(isAnalysis, vidPath, vinfo, processedBy);

            float[][] data = new float[2][];
            data[0] = new float[5];
            data[1] = new float[5];
            video.frameMetricValue = data;


            target.setVideo(video);
            //assert: no exception
        }

        /// <summary>
        ///Test "setVideo": correct data
        ///and "flush" after.
        ///</summary>
        [TestMethod()]
        public void setVideoTest_data()
        {
            Diagramm target = new Diagramm();
            bool isAnalysis = true;
            string vidPath = "";
            IVideoInfo vinfo = null;
            List<IMacroEntry> processedBy = new List<IMacroEntry>();
            MacroEntry macroEntry = new MacroEntry("TestPlugin", PluginType.IMetricOqat, "TestMemento");
            processedBy.Add(macroEntry);
            Video video = new Video(isAnalysis, vidPath, vinfo, processedBy);

            float[][] data = new float[2][];
            data[0] = new float[5] { 1, 2, 3, 4, 5 };
            data[1] = new float[5] { 1, 2, 3, 4, 5 };
            video.frameMetricValue = data;

            target.setVideo(video);
            //assert: no exception
            //GUI should show the diagram now >> (automated?) UI test

            target.flush();
            //assert: no exception
            //GUI should be flushed now >> (automated?) UI test
        }

        /// <summary>
        ///Test "setVideo": processedBy null
        ///</summary>
        [TestMethod()]
        public void setVideoTest_nullProcessedBy()
        {
            Diagramm target = new Diagramm();
            bool isAnalysis = true;
            string vidPath = "";
            IVideoInfo vinfo = null;
            List<IMacroEntry> processedBy = null;
            Video video = new Video(isAnalysis, vidPath, vinfo, processedBy);

            float[][] data = new float[2][];
            data[0] = new float[5] { 1, 2, 3, 4, 5 };
            data[1] = new float[5] { 1, 2, 3, 4, 5 };
            video.frameMetricValue = data;

            target.setVideo(video);
            //assert: no exception
        }

        /// <summary>
        ///Test "setVideo": processedBy empty
        ///</summary>
        [TestMethod()]
        public void setVideoTest_emptyProcessedBy()
        {
            Diagramm target = new Diagramm();
            bool isAnalysis = true;
            string vidPath = "";
            IVideoInfo vinfo = null;
            List<IMacroEntry> processedBy = new List<IMacroEntry>();
            Video video = new Video(isAnalysis, vidPath, vinfo, processedBy);

            float[][] data = new float[2][];
            data[0] = new float[5] { 1, 2, 3, 4, 5 };
            data[1] = new float[5] { 1, 2, 3, 4, 5 };
            video.frameMetricValue = data;

            target.setVideo(video);
            //assert: no exception
        }


        /// <summary>
        ///Test "setMemento"
        ///</summary>
        [TestMethod()]
        public void setMementoTest()
        {
            Diagramm target = new Diagramm(); // TODO: Passenden Wert initialisieren
            Memento memento = null; // TODO: Passenden Wert initialisieren
            target.setMemento(memento);

            //setMemento has not been implemented
            //if it should be used in the future, adapt this testcode!
        }

        /// <summary>
        ///Test "getMemento"
        ///</summary>
        [TestMethod()]
        public void getMementoTest()
        {
            Diagramm target = new Diagramm();
            Memento expected = new Memento("Diagram_settings", null);
            Memento actual;
            actual = target.getMemento();

            Assert.IsNotNull(actual, "Memento could not be loaded");
            Assert.AreEqual(expected.name, actual.name);
            Assert.AreEqual(expected.mementoPath, actual.mementoPath);
            Assert.AreEqual(expected.state, actual.state, "Has Memento for PP_Diagram been implemented? Adapt testcode!");
        }

        /// <summary>
        ///Test "propertyView"
        ///</summary>
        [TestMethod()]
        public void propertyViewTest()
        {
            Diagramm target = new Diagramm(); // TODO: Passenden Wert initialisieren
            UserControl actual;
            actual = target.propertyView;

            //propertyView at the moment is returning itself to satisfy interface demands
            Assert.AreSame(target, actual, "the field should return the usercontrol reference of itself.");
        }

        /// <summary>
        ///Ein Test für "type"
        ///</summary>
        [TestMethod()]
        public void typeTest()
        {
            Diagramm target = new Diagramm(); // TODO: Passenden Wert initialisieren
            PluginType actual;
            actual = target.type;

            Assert.AreEqual<PluginType>(PluginType.IPresentation, actual);
        }

        /// <summary>
        ///Ein Test für "presentationType"
        ///</summary>
        [TestMethod()]
        public void presentationTypeTest()
        {
            Diagramm target = new Diagramm(); // TODO: Passenden Wert initialisieren
            PresentationPluginType actual;
            actual = target.presentationType;

            Assert.AreEqual<PresentationPluginType>(PresentationPluginType.Diagram, actual);
        }

        /// <summary>
        ///Ein Test für "namePlugin"
        ///</summary>
        [TestMethod()]
        public void namePluginTest()
        {
            Diagramm target = new Diagramm(); // TODO: Passenden Wert initialisieren
            string actual;
            actual = target.namePlugin;

            Assert.AreEqual<string>("PP_Diagram", actual);
        }

        /// <summary>
        ///Ein Test für "threadSafe"
        ///</summary>
        [TestMethod()]
        public void threadSafeTest()
        {
            Diagramm target = new Diagramm();
            bool expected = false;
            bool actual;
            actual = target.threadSafe;

            Assert.AreEqual<bool>(expected, actual);
        }

        
    }
}
