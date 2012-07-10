
using PS_YuvVideoHandler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Oqat.PublicRessources.Model;
using Oqat.PublicRessources.Plugin;
using System.Drawing;
using System.Windows.Controls;
using System.Collections.Generic;
using System.IO;

namespace YuvVideoHandler_Tests
{


    /// <summary>
    ///Dies ist eine Testklasse für "PS_YuvVideoHandlerTest" und soll
    ///alle PS_YuvVideoHandlerTest Komponententests enthalten.
    ///</summary>
    [TestClass()]
    public class YuvVideoHandlerTest
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
        ///Test "vidInfo" getter
        ///</summary>
        [TestMethod()]
        public void vidInfoTest()
        {
            YuvVideoInfo info = new YuvVideoInfo();
            info.height = 666;

            YuvVideoHandler target =new YuvVideoHandler();
            target.setVideo(TESTVIDEO_PATH, info);
            IVideoInfo expected = info;
            IVideoInfo actual;
            actual = target.vidInfo;
            Assert.AreEqual(expected, actual);
        }




        /// <summary>
        ///Test "setParentControl"
        ///</summary>
        [TestMethod()]
        public void setParentControlTest()
        {
            YuvVideoInfo info = new YuvVideoInfo();
            YuvVideoHandler target = new YuvVideoHandler();
            target.setVideo(TESTVIDEO_PATH, info);

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
            YuvVideoInfo info = new YuvVideoInfo();
            YuvVideoHandler target = new YuvVideoHandler();
            target.setVideo(TESTVIDEO_PATH, info);

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
        [DeploymentItem("YuvVideoHandler.dll")]
        public void convertToRGB_Black_Test()
        {
            YuvVideoInfo info = new YuvVideoInfo();
            YuvVideoHandler handler = new YuvVideoHandler();
            handler.setVideo(TESTVIDEO_PATH, info);

            PrivateObject param0 = new PrivateObject(handler);
            YuvVideoHandler_Accessor target = new YuvVideoHandler_Accessor(param0);


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
        [DeploymentItem("YuvVideoHandler.dll")]
        public void convertToRGB_Red_Test()
        {
            YuvVideoInfo info = new YuvVideoInfo();
            YuvVideoHandler handler = new YuvVideoHandler();
            handler.setVideo(TESTVIDEO_PATH, info);

            PrivateObject param0 = new PrivateObject(handler);
            YuvVideoHandler_Accessor target = new YuvVideoHandler_Accessor(param0);


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
        [DeploymentItem("YuvVideoHandler.dll")]
        public void convertToRGB_Green_Test()
        {
            YuvVideoInfo info = new YuvVideoInfo();
            YuvVideoHandler handler = new YuvVideoHandler();
            handler.setVideo(TESTVIDEO_PATH, info);

            PrivateObject param0 = new PrivateObject(handler);
            YuvVideoHandler_Accessor target = new YuvVideoHandler_Accessor(param0);


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
        [DeploymentItem("YuvVideoHandler.dll")]
        public void convertToRGB_Blue_Test()
        {
            YuvVideoInfo info = new YuvVideoInfo();
            YuvVideoHandler handler = new YuvVideoHandler();
            handler.setVideo(TESTVIDEO_PATH, info);

            PrivateObject param0 = new PrivateObject(handler);
            YuvVideoHandler_Accessor target = new YuvVideoHandler_Accessor(param0);


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
        [DeploymentItem("YuvVideoHandler.dll")]
        public void convertToRGB_White_Test()
        {
            YuvVideoInfo info = new YuvVideoInfo();
            YuvVideoHandler handler = new YuvVideoHandler();
            handler.setVideo(TESTVIDEO_PATH, info);

            PrivateObject param0 = new PrivateObject(handler);
            YuvVideoHandler_Accessor target = new YuvVideoHandler_Accessor(param0);


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
        ///Test "clampToByte"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("YuvVideoHandler.dll")]
        public void clampToByteTest()
        {
            YuvVideoInfo info = new YuvVideoInfo();
            YuvVideoHandler handler = new YuvVideoHandler();
            handler.setVideo(TESTVIDEO_PATH, info);

            PrivateObject param0 = new PrivateObject(handler);
            YuvVideoHandler_Accessor target = new YuvVideoHandler_Accessor(param0);

            int[] val = new int[]        { -1, 128, 0, 255, 256, 500, int.MaxValue,  int.MinValue };
            byte[] expected = new byte[] {  0, 128, 0, 255, 255, 255, 255,           0};

            for (int i = 0; i < expected.Length; i++)
            {
                byte actual;
                actual = target.clampToByte(val[i]);
                Assert.AreEqual(expected[i], actual);
            }
        }

        




        #region convertToYUV_Tests

        /// <summary>
        ///Test "convertToYUV" on black
        ///</summary>
        [TestMethod()]
        [DeploymentItem("YuvVideoHandler.dll")]
        public void convertToYUV_Black_Test()
        {
            YuvVideoInfo info = new YuvVideoInfo();
            YuvVideoHandler handler = new YuvVideoHandler();
            handler.setVideo(TESTVIDEO_PATH, info);

            PrivateObject param0 = new PrivateObject(handler);
            YuvVideoHandler_Accessor target = new YuvVideoHandler_Accessor(param0);


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
        [DeploymentItem("YuvVideoHandler.dll")]
        public void convertToYUV_Red_Test()
        {
            YuvVideoInfo info = new YuvVideoInfo();
            YuvVideoHandler handler = new YuvVideoHandler();
            handler.setVideo(TESTVIDEO_PATH, info);

            PrivateObject param0 = new PrivateObject(handler);
            YuvVideoHandler_Accessor target = new YuvVideoHandler_Accessor(param0);


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
        [DeploymentItem("YuvVideoHandler.dll")]
        public void convertToYUV_Green_Test()
        {
            YuvVideoInfo info = new YuvVideoInfo();
            YuvVideoHandler handler = new YuvVideoHandler();
            handler.setVideo(TESTVIDEO_PATH, info);

            PrivateObject param0 = new PrivateObject(handler);
            YuvVideoHandler_Accessor target = new YuvVideoHandler_Accessor(param0);


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
        [DeploymentItem("YuvVideoHandler.dll")]
        public void convertToYUV_Blue_Test()
        {
            YuvVideoInfo info = new YuvVideoInfo();
            YuvVideoHandler handler = new YuvVideoHandler();
            handler.setVideo(TESTVIDEO_PATH, info);

            PrivateObject param0 = new PrivateObject(handler);
            YuvVideoHandler_Accessor target = new YuvVideoHandler_Accessor(param0);


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
        [DeploymentItem("YuvVideoHandler.dll")]
        public void convertToYUV_White_Test()
        {
            YuvVideoInfo info = new YuvVideoInfo();
            YuvVideoHandler handler = new YuvVideoHandler();
            handler.setVideo(TESTVIDEO_PATH, info);

            PrivateObject param0 = new PrivateObject(handler);
            YuvVideoHandler_Accessor target = new YuvVideoHandler_Accessor(param0);


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






        /// <summary>
        ///TODO: Test "getFrame"
        ///</summary>
        [TestMethod()]
        public void getFrameTest()
        {
            YuvVideoInfo info = new YuvVideoInfo();
            info.height = 144;
            info.width = 176;
            info.yuvFormat = YuvFormat.YUV420_IYUV;
            YuvVideoHandler target = new YuvVideoHandler();
            target.setVideo(TESTVIDEO_PATH, info);

            Bitmap expected = null;
            Bitmap actual;

            TestContext.BeginTimer("frame1");
            actual = target.getFrame(0);
            TestContext.EndTimer("frame1");

            TestContext.BeginTimer("frame2");
            actual = target.getFrame(1);
            TestContext.EndTimer("frame2");

            //TODO: How to compare this?!

            Assert.Inconclusive("Implement some way to check result first.");
        }




        /// <summary>
        ///Test getFrame and writeFrame
        ///</summary>
        [TestMethod()]
        public void readANDwriteFrameTest()
        {
            //init handler to read sample file
            YuvVideoInfo info_r = new YuvVideoInfo();
            info_r.height = 144;
            info_r.width = 176;
            info_r.yuvFormat = YuvFormat.YUV420_IYUV;
            YuvVideoHandler reader = new YuvVideoHandler();
            reader.setVideo(TESTVIDEO_PATH, info_r);

            //init handler to write a copy of sample file
            YuvVideoInfo info_w = new YuvVideoInfo();
            info_w.height = 144;
            info_w.width = 176;
            info_w.yuvFormat = YuvFormat.YUV420_IYUV;
            YuvVideoHandler writer = new YuvVideoHandler();
            writer.setVideo(TESTVIDEO_PATH + "copy", info_w);

            

            //copy sample file frame by frame
            //REMARK: This part can be commented out to save time once the copy is written to disk 
            //          if this test is run several times to tweak parameters and error calculations
            for (int j = 0; j < ((YuvVideoInfo)reader.vidInfo).frameCount; j++)
            {
                Bitmap bmp = reader.getFrame(j);

                writer.writeFrame(j, bmp);
            }
            




            //
            //compare original file with the copy read and written by the handler
            //
            //TODO: Debug writeFrame() & getFrame() as there are significant differences when a copy of a file is created through YuvVideoHandlers

            //set buffersize to one frame
            int bsize =(int)( info_r.width * info_r.height * (1 + YuvVideoHandler.getLum2Chrom(info_r.yuvFormat)) ); 

            int error = 0;
            int errorcount = 0;
            
            FileStream fs1;
            fs1 = new FileStream(TESTVIDEO_PATH, FileMode.Open);
            byte[] data1 = new byte[bsize];
            FileStream fs2;
            fs2 = new FileStream(TESTVIDEO_PATH+"copy", FileMode.Open);
            byte[] data2 = new byte[bsize];

            //log files are written to the log folder with error information about frames and the whole file
            //because an unfulfilled assertion cancels the whole testrun
            StreamWriter log = new StreamWriter("C:/Dokumente und Einstellungen/Sebastian/Eigene Dateien/PSE/Implementierung/YuvVideoHandler/log/log.txt");
            
            //compare original and copy bytewise
            for (int i = 0; i < fs1.Length; i += bsize)
            {
                int r = fs1.Read(data1, 0, bsize);
                int r2 = fs2.Read(data2, 0, bsize);

                Assert.AreEqual(r, r2, "file read out of sync");

                //init log writer for this frame
                //the logfile of each frame contains the difference in value of original and copy for each byte
                //the diffs are written in the textfile as a matrix according to pixel position
                StreamWriter logdetail = new StreamWriter("C:/Dokumente und Einstellungen/Sebastian/Eigene Dateien/PSE/Implementierung/YuvVideoHandler/log/log"+(i/bsize)+".txt");

                for (int j = 0; j < r; j++)
                {
                    int diff = Math.Abs(data1[j] - data2[j]);

                    int y = j / info_r.width;
                    int x = j % info_r.width;

                    if (j % info_r.width == 0) logdetail.Write(logdetail.NewLine);

                    //Assert.IsTrue(diff < 5, "big difference at "+x+","+y+": "+diff);
                    logdetail.Write(diff+" ");
                    error += diff;
                    if (diff > 5) errorcount++;


                }

                logdetail.Close();
                


                //the global logfile is written with the accumulated information about this frame
                float errorratio = (((float)errorcount) / bsize);
                log.WriteLine("Frame: "+(i/bsize)+" / errorratio: "+errorratio);
                //Assert.IsTrue(error < 10, i+": error ratio: " + errorratio + " / error count: " + errorcount + " / accumulated error: " + error);
                
                
                error = 0;
                errorcount = 0;
            }

            log.Close();
            
            fs1.Close();
            fs2.Close();

            Assert.Inconclusive("Compare logfiles to determine errorrates");
        }


    }
}
