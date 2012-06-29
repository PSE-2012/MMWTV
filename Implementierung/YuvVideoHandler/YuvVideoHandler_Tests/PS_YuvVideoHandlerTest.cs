using YuvVideoHandler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Oqat.PublicRessources.Model;
using Oqat.PublicRessources.Plugin;
using System.Drawing;
using System.Windows.Controls;
using System.Collections.Generic;

namespace YuvVideoHandler_Tests
{


    /// <summary>
    ///Dies ist eine Testklasse für "PS_YuvVideoHandlerTest" und soll
    ///alle PS_YuvVideoHandlerTest Komponententests enthalten.
    ///</summary>
    [TestClass()]
    public class PS_YuvVideoHandlerTest
    {


        private TestContext testContextInstance;
        private const string TESTVIDEO_PATH = "C:/Dokumente und Einstellungen/Sebastian/Eigene Dateien/PSE/Implementierung/YuvVideoHandler/akiyo_qcif.yuv";

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
        ///Test the constructor for wrong filepath
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorWrongFilepath()
        {
            PS_YuvVideoHandler handler = new PS_YuvVideoHandler("bla", new YuvVideoInfo());
        }



        /// <summary>
        ///Test "vidInfo" getter
        ///</summary>
        [TestMethod()]
        public void vidInfoTest()
        {
            YuvVideoInfo info = new YuvVideoInfo();
            info.height = 666;

            PS_YuvVideoHandler target = new PS_YuvVideoHandler(TESTVIDEO_PATH, info);
            IVideoInfo expected = info;
            IVideoInfo actual;
            actual = target.vidInfo;
            Assert.AreEqual(expected, actual);
        }



        /// <summary>
        ///Test "type" getter according to IPlugin
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
        ///Test "namePlugin" getter according to IPlugin
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
        ///Test "setParentControl"
        ///</summary>
        [TestMethod()]
        public void setParentControlTest()
        {
            PS_YuvVideoHandler target = new PS_YuvVideoHandler(TESTVIDEO_PATH, new YuvVideoInfo());

            Grid parent = new Grid();
            target.setParentControl(parent);

            Assert.AreEqual(1, parent.Children.Count);
        }
        /// <summary>
        ///Test adding the view to two different controls
        ///</summary>
        [TestMethod()]
        public void setTwoParentControlsTest()
        {
            PS_YuvVideoHandler target = new PS_YuvVideoHandler(TESTVIDEO_PATH, new YuvVideoInfo());

            Grid parent1 = new Grid();
            target.setParentControl(parent1);

            Grid parent2 = new Grid();
            target.setParentControl(parent2);

            Assert.AreEqual(1, parent1.Children.Count);
            Assert.AreEqual(1, parent2.Children.Count);
        }


    }
}
