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



        #region convertToRGB_Tests

        /// <summary>
        ///Test "convertToRGB" on black
        ///</summary>
        [TestMethod()]
        [DeploymentItem("YuvVideoHandler.exe")]
        public void convertToRGB_Black_Test()
        {
            PS_YuvVideoHandler handler = new PS_YuvVideoHandler(TESTVIDEO_PATH, new YuvVideoInfo());

            PrivateObject param0 = new PrivateObject(handler);
            PS_YuvVideoHandler_Accessor target = new PS_YuvVideoHandler_Accessor(param0);


            //Testing standard colors according to 
            //http://msdn.microsoft.com/en-us/library/windows/desktop/bb530104%28v=vs.85%29.aspx
            int y = 16; 
            int u = 128;
            int v = 128;
            Color expected = Color.Black;


            Color actual;
            actual = target.convertToRGB(y, u, v);

            Assert.AreEqual(expected.A, actual.A, "A not equal");
            Assert.AreEqual(expected.R, actual.R, "R not equal");
            Assert.AreEqual(expected.G, actual.G, "G not equal");
            Assert.AreEqual(expected.B, actual.B, "B not equal");
        }
        /// <summary>
        ///Test "convertToRGB" on Red
        ///</summary>
        [TestMethod()]
        [DeploymentItem("YuvVideoHandler.exe")]
        public void convertToRGB_Red_Test()
        {
            PS_YuvVideoHandler handler = new PS_YuvVideoHandler(TESTVIDEO_PATH, new YuvVideoInfo());

            PrivateObject param0 = new PrivateObject(handler);
            PS_YuvVideoHandler_Accessor target = new PS_YuvVideoHandler_Accessor(param0);


            //Testing standard colors according to 
            //http://msdn.microsoft.com/en-us/library/windows/desktop/bb530104%28v=vs.85%29.aspx
            int y = 81;
            int u = 90;
            int v = 240;
            Color expected = Color.Red;


            Color actual;
            actual = target.convertToRGB(y, u, v);

            Assert.AreEqual(expected.A, actual.A, "A not equal");
            Assert.AreEqual(expected.R, actual.R, "R not equal");
            Assert.AreEqual(expected.G, actual.G, "G not equal");
            Assert.AreEqual(expected.B, actual.B, "B not equal");
        }
        /// <summary>
        ///Test "convertToRGB" on green
        ///</summary>
        [TestMethod()]
        [DeploymentItem("YuvVideoHandler.exe")]
        public void convertToRGB_Green_Test()
        {
            PS_YuvVideoHandler handler = new PS_YuvVideoHandler(TESTVIDEO_PATH, new YuvVideoInfo());

            PrivateObject param0 = new PrivateObject(handler);
            PS_YuvVideoHandler_Accessor target = new PS_YuvVideoHandler_Accessor(param0);


            //Testing standard colors according to 
            //http://msdn.microsoft.com/en-us/library/windows/desktop/bb530104%28v=vs.85%29.aspx
            int y = 145;
            int u = 54;
            int v = 34;
            Color expected = Color.Lime;


            Color actual;
            actual = target.convertToRGB(y, u, v);

            Assert.AreEqual(expected.A, actual.A, "A not equal");
            Assert.AreEqual(expected.R, actual.R, "R not equal");
            Assert.AreEqual(expected.G, actual.G, "G not equal");
            Assert.AreEqual(expected.B, actual.B, "B not equal");
        }
        /// <summary>
        ///Test "convertToRGB" on blue
        ///</summary>
        [TestMethod()]
        [DeploymentItem("YuvVideoHandler.exe")]
        public void convertToRGB_Blue_Test()
        {
            PS_YuvVideoHandler handler = new PS_YuvVideoHandler(TESTVIDEO_PATH, new YuvVideoInfo());

            PrivateObject param0 = new PrivateObject(handler);
            PS_YuvVideoHandler_Accessor target = new PS_YuvVideoHandler_Accessor(param0);


            //Testing standard colors according to 
            //http://msdn.microsoft.com/en-us/library/windows/desktop/bb530104%28v=vs.85%29.aspx
            int y = 41;
            int u = 240;
            int v = 110;
            Color expected = Color.Blue;


            Color actual;
            actual = target.convertToRGB(y, u, v);

            Assert.AreEqual(expected.A, actual.A, "A not equal");
            Assert.AreEqual(expected.R, actual.R, "R not equal");
            Assert.AreEqual(expected.G, actual.G, "G not equal");
            Assert.AreEqual(expected.B, actual.B, "B not equal");
        }
        /// <summary>
        ///Test "convertToRGB" on white
        ///</summary>
        [TestMethod()]
        [DeploymentItem("YuvVideoHandler.exe")]
        public void convertToRGB_White_Test()
        {
            PS_YuvVideoHandler handler = new PS_YuvVideoHandler(TESTVIDEO_PATH, new YuvVideoInfo());

            PrivateObject param0 = new PrivateObject(handler);
            PS_YuvVideoHandler_Accessor target = new PS_YuvVideoHandler_Accessor(param0);


            //Testing standard colors according to 
            //http://msdn.microsoft.com/en-us/library/windows/desktop/bb530104%28v=vs.85%29.aspx
            int y = 235;
            int u = 128;
            int v = 128;
            Color expected = Color.White;


            Color actual;
            actual = target.convertToRGB(y, u, v);

            Assert.AreEqual(expected.A, actual.A, "A not equal");
            Assert.AreEqual(expected.R, actual.R, "R not equal");
            Assert.AreEqual(expected.G, actual.G, "G not equal");
            Assert.AreEqual(expected.B, actual.B, "B not equal");
        }

        #endregion


        /// <summary>
        ///Test für "clampToByte"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("YuvVideoHandler.exe")]
        public void clampToByteTest()
        {
            PS_YuvVideoHandler handler = new PS_YuvVideoHandler(TESTVIDEO_PATH, new YuvVideoInfo());

            PrivateObject param0 = new PrivateObject(handler);
            PS_YuvVideoHandler_Accessor target = new PS_YuvVideoHandler_Accessor(param0);

            int[] val = new int[]        { -1, 128, 0, 255, 256, 500, int.MaxValue,  int.MinValue };
            byte[] expected = new byte[] {  0, 128, 0, 255, 255, 255, 255,           0};

            for (int i = 0; i < expected.Length; i++)
            {
                byte actual;
                actual = target.clampToByte(val[i]);
                Assert.AreEqual(expected[i], actual);
            }
        }

        /// <summary>
        ///TODO: Ein Test für "getFrame"
        ///</summary>
        [TestMethod()]
        public void getFrameTest()
        {
            YuvVideoInfo info = new YuvVideoInfo();
            info.height = 144;
            info.width = 176;
            info.yuvFormat = YuvFormat.YUV420_IYUV;
            PS_YuvVideoHandler target = new PS_YuvVideoHandler(TESTVIDEO_PATH, info);

            Bitmap expected = null; // TODO: Passenden Wert initialisieren
            Bitmap actual;

            TestContext.BeginTimer("frame1");
            actual = target.getFrame(0);
            TestContext.EndTimer("frame1");

            TestContext.BeginTimer("frame2");
            actual = target.getFrame(1);
            TestContext.EndTimer("frame2");

            //TODO: Check results of getFrame

        }


        #region convertToYUV_Tests

        /// <summary>
        ///Test "convertToYUV" on black
        ///</summary>
        [TestMethod()]
        [DeploymentItem("YuvVideoHandler.exe")]
        public void convertToYUV_Black_Test()
        {
            PS_YuvVideoHandler handler = new PS_YuvVideoHandler(TESTVIDEO_PATH, new YuvVideoInfo());

            PrivateObject param0 = new PrivateObject(handler);
            PS_YuvVideoHandler_Accessor target = new PS_YuvVideoHandler_Accessor(param0);


            //Testing standard colors according to 
            //http://msdn.microsoft.com/en-us/library/windows/desktop/bb530104%28v=vs.85%29.aspx
            int y = 16;
            int u = 128;
            int v = 128;

            byte[] actual;
            actual = target.convertToYUV(Color.Black);

            Assert.AreEqual(y, actual[0], "Y not equal");
            Assert.AreEqual(u, actual[1], "U not equal");
            Assert.AreEqual(v, actual[2], "V not equal");
        }
        /// <summary>
        ///Test "convertToYUV" on Red
        ///</summary>
        [TestMethod()]
        [DeploymentItem("YuvVideoHandler.exe")]
        public void convertToYUV_Red_Test()
        {
            PS_YuvVideoHandler handler = new PS_YuvVideoHandler(TESTVIDEO_PATH, new YuvVideoInfo());

            PrivateObject param0 = new PrivateObject(handler);
            PS_YuvVideoHandler_Accessor target = new PS_YuvVideoHandler_Accessor(param0);


            //Testing standard colors according to 
            //http://msdn.microsoft.com/en-us/library/windows/desktop/bb530104%28v=vs.85%29.aspx
            int y = 81;
            int u = 90;
            int v = 240;

            byte[] actual;
            actual = target.convertToYUV(Color.Red);

            Assert.AreEqual(y, actual[0], "Y not equal");
            Assert.AreEqual(u, actual[1], "U not equal");
            Assert.AreEqual(v, actual[2], "V not equal");
        }
        /// <summary>
        ///Test "convertToYUV" on green
        ///</summary>
        [TestMethod()]
        [DeploymentItem("YuvVideoHandler.exe")]
        public void convertToYUV_Green_Test()
        {
            PS_YuvVideoHandler handler = new PS_YuvVideoHandler(TESTVIDEO_PATH, new YuvVideoInfo());

            PrivateObject param0 = new PrivateObject(handler);
            PS_YuvVideoHandler_Accessor target = new PS_YuvVideoHandler_Accessor(param0);


            //Testing standard colors according to 
            //http://msdn.microsoft.com/en-us/library/windows/desktop/bb530104%28v=vs.85%29.aspx
            int y = 145;
            int u = 54;
            int v = 34;

            byte[] actual;
            actual = target.convertToYUV(Color.Lime);

            Assert.AreEqual(y, actual[0], "Y not equal");
            Assert.AreEqual(u, actual[1], "U not equal");
            Assert.AreEqual(v, actual[2], "V not equal");
        }
        /// <summary>
        ///Test "convertToYUV" on blue
        ///</summary>
        [TestMethod()]
        [DeploymentItem("YuvVideoHandler.exe")]
        public void convertToYUV_Blue_Test()
        {
            PS_YuvVideoHandler handler = new PS_YuvVideoHandler(TESTVIDEO_PATH, new YuvVideoInfo());

            PrivateObject param0 = new PrivateObject(handler);
            PS_YuvVideoHandler_Accessor target = new PS_YuvVideoHandler_Accessor(param0);


            //Testing standard colors according to 
            //http://msdn.microsoft.com/en-us/library/windows/desktop/bb530104%28v=vs.85%29.aspx
            int y = 41;
            int u = 240;
            int v = 110;

            byte[] actual;
            actual = target.convertToYUV(Color.Blue);

            Assert.AreEqual(y, actual[0], "Y not equal");
            Assert.AreEqual(u, actual[1], "U not equal");
            Assert.AreEqual(v, actual[2], "V not equal");
        }
        /// <summary>
        ///Test "convertToYUV" on white
        ///</summary>
        [TestMethod()]
        [DeploymentItem("YuvVideoHandler.exe")]
        public void convertToYUV_White_Test()
        {
            PS_YuvVideoHandler handler = new PS_YuvVideoHandler(TESTVIDEO_PATH, new YuvVideoInfo());

            PrivateObject param0 = new PrivateObject(handler);
            PS_YuvVideoHandler_Accessor target = new PS_YuvVideoHandler_Accessor(param0);


            //Testing standard colors according to 
            //http://msdn.microsoft.com/en-us/library/windows/desktop/bb530104%28v=vs.85%29.aspx
            int y = 235;
            int u = 128;
            int v = 128;

            byte[] actual;
            actual = target.convertToYUV(Color.White);

            Assert.AreEqual(y, actual[0], "Y not equal");
            Assert.AreEqual(u, actual[1], "U not equal");
            Assert.AreEqual(v, actual[2], "V not equal");
        }

        #endregion

    }
}
