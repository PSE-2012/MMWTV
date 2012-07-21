using Oqat.PublicRessources.Model;
using Oqat.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Oqat.PublicRessources.Plugin;
using PS_YuvVideoHandler;
using System.Collections.Generic;

namespace OQAT_Tests
{
    
    
    /// <summary>
    ///Dies ist eine Testklasse für "VideoTest" und soll
    ///alle VideoTest Komponententests enthalten.
    ///</summary>
    [TestClass()]
    public class VideoTest
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
        ///Ein Test für "getMemento"
        ///</summary>
        [TestMethod()]
        public void getMementoTest()
        {
            YuvVideoInfo info = new YuvVideoInfo();
            info.height = 200;
            info.width = 100;
            info.yuvFormat = YuvFormat.YUV420_IYUV;

            string path = "mypath";
            bool isana = true;

            Video target = new Video(isana, path, info);

            Dictionary<PresentationPluginType, System.Collections.Generic.List<string>> er = new System.Collections.Generic.Dictionary<PresentationPluginType, System.Collections.Generic.List<string>>();
            List<string> li = new List<string>();
            li.Add("testcustom");
            er.Add(PresentationPluginType.Custom, li);
            target.extraResources = er;

            float[][] metrics = new float[][] { new float[] { 1, 2, 3, 4, 5 } };
            target.frameMetricValue = metrics;
            List<MacroEntry> macros = new List<MacroEntry>();
            target.processedBy = macros;

            Memento actual;
            actual = target.getMemento();

            Video vidmem = (Video)actual.state;

            Assert.AreEqual(vidmem.vidPath, path);
            Assert.AreEqual(vidmem.isAnalysis, isana);
            Assert.AreEqual(vidmem.extraResources, er);
            Assert.AreEqual(vidmem.frameMetricValue, metrics);
            Assert.AreEqual(vidmem.processedBy, macros);
            Assert.AreEqual(vidmem.vidInfo, info);
        }

        /// <summary>
        ///Ein Test für "setMemento"
        ///</summary>
        [TestMethod()]
        public void setMementoTest()
        {
            Video target = new Video(true, "bla", null);
            
            
            
            YuvVideoInfo info = new YuvVideoInfo();
            info.height = 200;
            info.width = 100;
            info.yuvFormat = YuvFormat.YUV420_IYUV;

            string path = "mypath";
            bool isana = true;

            Video refv = new Video(isana, path, info);

            Dictionary<PresentationPluginType, System.Collections.Generic.List<string>> er = new System.Collections.Generic.Dictionary<PresentationPluginType, System.Collections.Generic.List<string>>();
            List<string> li = new List<string>();
            li.Add("testcustom");
            er.Add(PresentationPluginType.Custom, li);
            refv.extraResources = er;

            float[][] metrics = new float[][] { new float[] { 1, 2, 3, 4, 5 } };
            refv.frameMetricValue = metrics;
            List<MacroEntry> macros = new List<MacroEntry>();
            refv.processedBy = macros;

            Memento mem_input = refv.getMemento();

            //TODO: use Cartaker to write and read the memento - only this might show real problems

            target.setMemento(mem_input);

            Assert.AreEqual(target.vidPath, path);
            Assert.AreEqual(target.isAnalysis, isana);
            Assert.AreEqual(target.extraResources, er);
            Assert.AreEqual(target.frameMetricValue, metrics);
            Assert.AreEqual(target.processedBy, macros);
            Assert.AreEqual(target.vidInfo, info);
        }


        /// <summary>
        ///Ein Test für "getVideoHandler"
        ///</summary>
        [TestMethod()]
        public void getVideoHandlerTest()
        {
            //Video target = new Video(); // TODO: Passenden Wert initialisieren
            //IVideoHandler expected = null; // TODO: Passenden Wert initialisieren
            //IVideoHandler actual;
            //actual = target.getVideoHandler();
            //Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Überprüfen Sie die Richtigkeit dieser Testmethode.");
        }

        
    }
}
