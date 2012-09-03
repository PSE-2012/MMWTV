using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using PS_YuvVideoHandler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace OQAT_Tests
{
    ///<summary>
    ///Class for testing YUV VideoInfo
    ///</summary>
    [TestClass]
    public class VideoInfoTest
    {
        private static string sampleVideosPath;
        private static string path;
        private static int framecount;
        private static string[] sampleVideos;

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            sampleVideosPath = testContext.TestDir + "\\..\\..\\Oqat_Tests\\TestData\\sampleVideos";
            // Warning: video settings might have to be set manually in test methods!
            sampleVideos = Directory.GetFiles(sampleVideosPath, "*.yuv");
        }

        ///<summary>
        ///Constructor test
        ///</summary>
        [TestMethod]
        public void constructorTest()
        {
            path = sampleVideos[2];
            YuvVideoInfo info1 = new YuvVideoInfo();
            info1.width = 352;
            info1.height = 288;
            info1.yuvFormat = YuvFormat.YUV420_IYUV;
            YuvVideoInfo info2 = new YuvVideoInfo(path);
            Assert.AreEqual(info1.width, info2.width);
            Assert.AreEqual(info1.height, info2.height);
            Assert.AreEqual(info1.yuvFormat, info2.yuvFormat);
            Assert.IsTrue(info1.Equals(info2));
            try
            {
                YuvVideoInfo falseInfo = new YuvVideoInfo("D:\\bla_cif.yuv");
                Assert.Fail("no exception thrown");
            }
            catch (Exception ex)
            {

            }
        }

        ///<summary>
        ///Test for the clone method
        ///</summary>
        [TestMethod]
        public void cloneTest()
        {
            path = sampleVideos[2];
            YuvVideoInfo info2 = new YuvVideoInfo(path);
            YuvVideoInfo info1 = (YuvVideoInfo)info2.Clone();
            Assert.AreEqual(info1.width, info2.width);
            Assert.AreEqual(info1.height, info2.height);
            Assert.AreEqual(info1.yuvFormat, info2.yuvFormat);
        }

        ///<summary>
        ///Checks if frame count is calculated correctly
        ///</summary>
        [TestMethod]
        public void frameCountTest()
        {
            framecount = 150;
            path = sampleVideos[2];
            YuvVideoInfo info = new YuvVideoInfo(path);
            Assert.AreEqual(framecount, info.frameCount);
        }
    }
}
