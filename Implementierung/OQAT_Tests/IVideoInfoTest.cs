using Oqat.PublicRessources.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace OQAT_Tests
{
    
    
    /// <summary>
    ///Dies ist eine Testklasse für "IVideoInfoTest" und soll
    ///alle IVideoInfoTest Komponententests enthalten.
    ///</summary>
    [TestClass()]
    public class IVideoInfoTest
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


        internal virtual IVideoInfo CreateIVideoInfo()
        {
            // TODO: Geeignete konkrete Klasse instanziieren
            IVideoInfo target = null;
            return target;
        }

        /// <summary>
        ///Ein Test für "videoCodecName"
        ///</summary>
        [TestMethod()]
        public void videoCodecNameTest()
        {
            IVideoInfo target = CreateIVideoInfo(); // TODO: Passenden Wert initialisieren
            string expected = string.Empty; // TODO: Passenden Wert initialisieren
            string actual;
            target.videoCodecName = expected;
            actual = target.videoCodecName;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Überprüfen Sie die Richtigkeit dieser Testmethode.");
        }
    }
}
