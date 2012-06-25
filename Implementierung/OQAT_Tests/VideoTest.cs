using Oqat.PublicRessources.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Oqat.PublicRessources.Plugin;
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
        ///Ein Test für "vidPath"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("EntwurfLib.dll")]
        public void vidPathTest()
        {
            PrivateObject param0 = null; // TODO: Passenden Wert initialisieren
            Video_Accessor target = new Video_Accessor(param0); // TODO: Passenden Wert initialisieren
            string expected = string.Empty; // TODO: Passenden Wert initialisieren
            string actual;
            target.vidPath = expected;
            actual = target.vidPath;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Überprüfen Sie die Richtigkeit dieser Testmethode.");
        }

        /// <summary>
        ///Ein Test für "vidInfo"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("EntwurfLib.dll")]
        public void vidInfoTest()
        {
            PrivateObject param0 = null; // TODO: Passenden Wert initialisieren
            Video_Accessor target = new Video_Accessor(param0); // TODO: Passenden Wert initialisieren
            IVideoInfo expected = null; // TODO: Passenden Wert initialisieren
            IVideoInfo actual;
            target.vidInfo = expected;
            actual = target.vidInfo;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Überprüfen Sie die Richtigkeit dieser Testmethode.");
        }

        /// <summary>
        ///Ein Test für "processedBy"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("EntwurfLib.dll")]
        public void processedByTest()
        {
            PrivateObject param0 = null; // TODO: Passenden Wert initialisieren
            Video_Accessor target = new Video_Accessor(param0); // TODO: Passenden Wert initialisieren
            List<MacroEntry> expected = null; // TODO: Passenden Wert initialisieren
            List<MacroEntry> actual;
            target.processedBy = expected;
            actual = target.processedBy;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Überprüfen Sie die Richtigkeit dieser Testmethode.");
        }

        /// <summary>
        ///Ein Test für "isAnalysis"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("EntwurfLib.dll")]
        public void isAnalysisTest()
        {
            PrivateObject param0 = null; // TODO: Passenden Wert initialisieren
            Video_Accessor target = new Video_Accessor(param0); // TODO: Passenden Wert initialisieren
            bool expected = false; // TODO: Passenden Wert initialisieren
            bool actual;
            target.isAnalysis = expected;
            actual = target.isAnalysis;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Überprüfen Sie die Richtigkeit dieser Testmethode.");
        }

        /// <summary>
        ///Ein Test für "frameMetricValue"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("EntwurfLib.dll")]
        public void frameMetricValueTest()
        {
            PrivateObject param0 = null; // TODO: Passenden Wert initialisieren
            Video_Accessor target = new Video_Accessor(param0); // TODO: Passenden Wert initialisieren
            float[][] expected = null; // TODO: Passenden Wert initialisieren
            float[][] actual;
            target.frameMetricValue = expected;
            actual = target.frameMetricValue;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Überprüfen Sie die Richtigkeit dieser Testmethode.");
        }

        /// <summary>
        ///Ein Test für "extraRessources"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("EntwurfLib.dll")]
        public void extraRessourcesTest()
        {
            PrivateObject param0 = null; // TODO: Passenden Wert initialisieren
            Video_Accessor target = new Video_Accessor(param0); // TODO: Passenden Wert initialisieren
            Dictionary<PresentationPluginType, List<string>> expected = null; // TODO: Passenden Wert initialisieren
            Dictionary<PresentationPluginType, List<string>> actual;
            target.extraRessources = expected;
            actual = target.extraRessources;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Überprüfen Sie die Richtigkeit dieser Testmethode.");
        }

        /// <summary>
        ///Ein Test für "setMemento"
        ///</summary>
        [TestMethod()]
        public void setMementoTest()
        {
            bool isAnalysis = false; // TODO: Passenden Wert initialisieren
            string vidPath = string.Empty; // TODO: Passenden Wert initialisieren
            IVideoInfo vidInfo = null; // TODO: Passenden Wert initialisieren
            Video target = new Video(isAnalysis, vidPath, vidInfo); // TODO: Passenden Wert initialisieren
            Memento memento = null; // TODO: Passenden Wert initialisieren
            target.setMemento(memento);
            Assert.Inconclusive("Eine Methode, die keinen Wert zurückgibt, kann nicht überprüft werden.");
        }

        /// <summary>
        ///Ein Test für "getVideoHandler"
        ///</summary>
        [TestMethod()]
        public void getVideoHandlerTest()
        {
            bool isAnalysis = false; // TODO: Passenden Wert initialisieren
            string vidPath = string.Empty; // TODO: Passenden Wert initialisieren
            IVideoInfo vidInfo = null; // TODO: Passenden Wert initialisieren
            Video target = new Video(isAnalysis, vidPath, vidInfo); // TODO: Passenden Wert initialisieren
            IVideoHandler expected = null; // TODO: Passenden Wert initialisieren
            IVideoHandler actual;
            actual = target.getVideoHandler();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Überprüfen Sie die Richtigkeit dieser Testmethode.");
        }

        /// <summary>
        ///Ein Test für "getMemento"
        ///</summary>
        [TestMethod()]
        public void getMementoTest()
        {
            bool isAnalysis = false; // TODO: Passenden Wert initialisieren
            string vidPath = string.Empty; // TODO: Passenden Wert initialisieren
            IVideoInfo vidInfo = null; // TODO: Passenden Wert initialisieren
            Video target = new Video(isAnalysis, vidPath, vidInfo); // TODO: Passenden Wert initialisieren
            Memento expected = null; // TODO: Passenden Wert initialisieren
            Memento actual;
            actual = target.getMemento();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Überprüfen Sie die Richtigkeit dieser Testmethode.");
        }

        /// <summary>
        ///Ein Test für "Video-Konstruktor"
        ///</summary>
        [TestMethod()]
        public void VideoConstructorTest()
        {
            bool isAnalysis = false; // TODO: Passenden Wert initialisieren
            string vidPath = string.Empty; // TODO: Passenden Wert initialisieren
            IVideoInfo vidInfo = null; // TODO: Passenden Wert initialisieren
            Video target = new Video(isAnalysis, vidPath, vidInfo);
            Assert.Inconclusive("TODO: Code zum Überprüfen des Ziels implementieren");
        }
    }
}
