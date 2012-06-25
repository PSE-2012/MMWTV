using YuvVideoHandler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Oqat.PublicRessources.Model;
using Oqat.PublicRessources.Plugin;
using System.Drawing;
using System.Windows.Controls;
using System.Collections.Generic;

namespace OQAT_Tests
{
    
    
    /// <summary>
    ///Dies ist eine Testklasse für "PS_YuvVideoHandlerTest" und soll
    ///alle PS_YuvVideoHandlerTest Komponententests enthalten.
    ///</summary>
    [TestClass()]
    public class PS_YuvVideoHandlerTest
    {


        private TestContext testContextInstance;
        private const string TESTVIDEO_PATH = "./akiyo_qcif.yuv";

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
        ///Ein Test für Konstruktor-Parameter filepath
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorWrongFilepath()
        {
            PS_YuvVideoHandler handler = new PS_YuvVideoHandler("bla", new YuvVideoInfo());
        }

        /// <summary>
        ///Ein Test für Konstruktor-Parameter videoInfo
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorEmptyVidInfo()
        {
            PS_YuvVideoHandler handler = new PS_YuvVideoHandler(TESTVIDEO_PATH, null);
        }


        /// <summary>
        ///Ein Test für "vidInfo"
        ///</summary>
        [TestMethod()]
        public void vidInfoTest()
        {
            YuvVideoInfo info = new YuvVideoInfo();
            PS_YuvVideoHandler target = new PS_YuvVideoHandler(TESTVIDEO_PATH, info);
            IVideoInfo expected = info; // TODO: Passenden Wert initialisieren
            IVideoInfo actual;
            actual = target.vidInfo;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Ein Test für "type"
        ///</summary>
        [TestMethod()]
        public void typeTest()
        {
            PS_YuvVideoHandler target = new PS_YuvVideoHandler(TESTVIDEO_PATH, new YuvVideoInfo());
            PluginType expected = PluginType.VideoHandler;
            PluginType actual;
            actual = target.type;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Ein Test für "namePlugin"
        ///</summary>
        [TestMethod()]
        public void namePluginTest()
        {
            PS_YuvVideoHandler target = new PS_YuvVideoHandler(TESTVIDEO_PATH, new YuvVideoInfo());
            string expected = "YuvVideoHandler";
            string actual;
            actual = target.namePlugin;
            Assert.AreEqual(expected, actual);
        }





        /// <summary>
        ///Ein Test für "writeFrames"
        ///</summary>
        [TestMethod()]
        public void writeFramesTest()
        {
            PS_YuvVideoHandler target = new PS_YuvVideoHandler(TESTVIDEO_PATH, new YuvVideoInfo());
            int frameNum = 0; // TODO: Passenden Wert initialisieren
            Bitmap[] frames = null; // TODO: Passenden Wert initialisieren
            target.writeFrames(frameNum, frames);
            Assert.Inconclusive("Eine Methode, die keinen Wert zurückgibt, kann nicht überprüft werden.");
        }

        /// <summary>
        ///Ein Test für "writeFrame"
        ///</summary>
        [TestMethod()]
        public void writeFrameTest()
        {
            PS_YuvVideoHandler target = new PS_YuvVideoHandler(TESTVIDEO_PATH, new YuvVideoInfo());
            int frameNum = 0; // TODO: Passenden Wert initialisieren
            Bitmap frame = null; // TODO: Passenden Wert initialisieren
            target.writeFrame(frameNum, frame);
            Assert.Inconclusive("Eine Methode, die keinen Wert zurückgibt, kann nicht überprüft werden.");
        }


        /// <summary>
        ///Ein Test für "setMemento"
        ///</summary>
        [TestMethod()]
        public void setMementoTest()
        {
            PS_YuvVideoHandler target = new PS_YuvVideoHandler(TESTVIDEO_PATH, new YuvVideoInfo());
            Memento memento = null; // TODO: Passenden Wert initialisieren
            target.setMemento(memento);
            Assert.Inconclusive("Eine Methode, die keinen Wert zurückgibt, kann nicht überprüft werden.");
        }

        /// <summary>
        ///Ein Test für "getMemento"
        ///</summary>
        [TestMethod()]
        public void getMementoTest()
        {
            PS_YuvVideoHandler target = new PS_YuvVideoHandler(TESTVIDEO_PATH, new YuvVideoInfo());
            Memento expected = null; // TODO: Passenden Wert initialisieren
            Memento actual;
            actual = target.getMemento();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Überprüfen Sie die Richtigkeit dieser Testmethode.");
        }

        /// <summary>
        ///Ein Test für "getFrames"
        ///</summary>
        [TestMethod()]
        public void getFramesTest()
        {
            PS_YuvVideoHandler target = new PS_YuvVideoHandler(TESTVIDEO_PATH, new YuvVideoInfo());
            int frameNm = 0; // TODO: Passenden Wert initialisieren
            int offset = 0; // TODO: Passenden Wert initialisieren
            Bitmap[] expected = null; // TODO: Passenden Wert initialisieren
            Bitmap[] actual;
            actual = target.getFrames(frameNm, offset);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Überprüfen Sie die Richtigkeit dieser Testmethode.");
        }

        /// <summary>
        ///Ein Test für "getFrame"
        ///</summary>
        [TestMethod()]
        public void getFrameTest()
        {
            PS_YuvVideoHandler target = new PS_YuvVideoHandler(TESTVIDEO_PATH, new YuvVideoInfo());
            int frameNm = 0; // TODO: Passenden Wert initialisieren
            Bitmap expected = null; // TODO: Passenden Wert initialisieren
            Bitmap actual;
            actual = target.getFrame(frameNm);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Überprüfen Sie die Richtigkeit dieser Testmethode.");
        }

        /// <summary>
        ///Ein Test für "getEventHandlers"
        ///</summary>
        [TestMethod()]
        public void getEventHandlersTest()
        {
            PS_YuvVideoHandler target = new PS_YuvVideoHandler(TESTVIDEO_PATH, new YuvVideoInfo());
            Dictionary<EventType, List<Delegate>> expected = null; // TODO: Passenden Wert initialisieren
            Dictionary<EventType, List<Delegate>> actual;
            actual = target.getEventHandlers();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Überprüfen Sie die Richtigkeit dieser Testmethode.");
        }

        /// <summary>
        ///Ein Test für "PS_YuvVideoHandler-Konstruktor"
        ///</summary>
        [TestMethod()]
        public void PS_YuvVideoHandlerConstructorTest()
        {
            PS_YuvVideoHandler target = new PS_YuvVideoHandler(TESTVIDEO_PATH, new YuvVideoInfo());
            Assert.Inconclusive("TODO: Code zum Überprüfen des Ziels implementieren");
        }
    }
}
