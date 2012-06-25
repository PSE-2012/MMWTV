using YuvVideoHandler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace OQAT_Tests
{
    
    
    /// <summary>
    ///Dies ist eine Testklasse für "YuvVideoInfoTest" und soll
    ///alle YuvVideoInfoTest Komponententests enthalten.
    ///</summary>
    [TestClass()]
    public class YuvVideoInfoTest
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
        ///Ein Test für "width"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("EntwurfLib.dll")]
        public void widthTest()
        {
            YuvVideoInfo_Accessor target = new YuvVideoInfo_Accessor(); // TODO: Passenden Wert initialisieren
            int expected = 0; // TODO: Passenden Wert initialisieren
            int actual;
            target.width = expected;
            actual = target.width;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Überprüfen Sie die Richtigkeit dieser Testmethode.");
        }

        /// <summary>
        ///Ein Test für "videoCodecName"
        ///</summary>
        [TestMethod()]
        public void videoCodecNameTest()
        {
            YuvVideoInfo target = new YuvVideoInfo(); // TODO: Passenden Wert initialisieren
            string expected = string.Empty; // TODO: Passenden Wert initialisieren
            string actual;
            target.videoCodecName = expected;
            actual = target.videoCodecName;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Überprüfen Sie die Richtigkeit dieser Testmethode.");
        }

        /// <summary>
        ///Ein Test für "height"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("EntwurfLib.dll")]
        public void heightTest()
        {
            YuvVideoInfo_Accessor target = new YuvVideoInfo_Accessor(); // TODO: Passenden Wert initialisieren
            int expected = 0; // TODO: Passenden Wert initialisieren
            int actual;
            target.height = expected;
            actual = target.height;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Überprüfen Sie die Richtigkeit dieser Testmethode.");
        }

        /// <summary>
        ///Ein Test für "frameNum"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("EntwurfLib.dll")]
        public void frameNumTest()
        {
            YuvVideoInfo_Accessor target = new YuvVideoInfo_Accessor(); // TODO: Passenden Wert initialisieren
            long expected = 0; // TODO: Passenden Wert initialisieren
            long actual;
            target.frameNum = expected;
            actual = target.frameNum;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Überprüfen Sie die Richtigkeit dieser Testmethode.");
        }

        /// <summary>
        ///Ein Test für "YuvVideoInfo-Konstruktor"
        ///</summary>
        [TestMethod()]
        public void YuvVideoInfoConstructorTest()
        {
            YuvVideoInfo target = new YuvVideoInfo();
            Assert.Inconclusive("TODO: Code zum Überprüfen des Ziels implementieren");
        }
    }
}
