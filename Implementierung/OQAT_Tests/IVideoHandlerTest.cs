using Oqat.PublicRessources.Plugin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Oqat.PublicRessources.Model;
using System.Drawing;

namespace OQAT_Tests
{
    
    
    /// <summary>
    ///Dies ist eine Testklasse für "IVideoHandlerTest" und soll
    ///alle IVideoHandlerTest Komponententests enthalten.
    ///</summary>
    [TestClass()]
    public class IVideoHandlerTest
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


        internal virtual IVideoHandler CreateIVideoHandler()
        {
            // TODO: Geeignete konkrete Klasse instanziieren
            IVideoHandler target = null;
            return target;
        }

        /// <summary>
        ///Ein Test für "vidInfo"
        ///</summary>
        [TestMethod()]
        public void vidInfoTest()
        {
            IVideoHandler target = CreateIVideoHandler(); // TODO: Passenden Wert initialisieren
            IVideoInfo expected = null; // TODO: Passenden Wert initialisieren
            IVideoInfo actual;
            target.vidInfo = expected;
            actual = target.vidInfo;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Überprüfen Sie die Richtigkeit dieser Testmethode.");
        }

        /// <summary>
        ///Ein Test für "writeFrames"
        ///</summary>
        [TestMethod()]
        public void writeFramesTest()
        {
            IVideoHandler target = CreateIVideoHandler(); // TODO: Passenden Wert initialisieren
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
            IVideoHandler target = CreateIVideoHandler(); // TODO: Passenden Wert initialisieren
            int frameNum = 0; // TODO: Passenden Wert initialisieren
            Bitmap frame = null; // TODO: Passenden Wert initialisieren
            target.writeFrame(frameNum, frame);
            Assert.Inconclusive("Eine Methode, die keinen Wert zurückgibt, kann nicht überprüft werden.");
        }

        /// <summary>
        ///Ein Test für "getFrames"
        ///</summary>
        [TestMethod()]
        public void getFramesTest()
        {
            IVideoHandler target = CreateIVideoHandler(); // TODO: Passenden Wert initialisieren
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
            IVideoHandler target = CreateIVideoHandler(); // TODO: Passenden Wert initialisieren
            int frameNm = 0; // TODO: Passenden Wert initialisieren
            Bitmap expected = null; // TODO: Passenden Wert initialisieren
            Bitmap actual;
            actual = target.getFrame(frameNm);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Überprüfen Sie die Richtigkeit dieser Testmethode.");
        }
    }
}
