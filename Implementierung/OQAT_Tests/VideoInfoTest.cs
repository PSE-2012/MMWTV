using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using PS_YuvVideoHandler;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OQAT_Tests
{
    ///<summary>
    ///Class for testing YUV VideoInfo
    ///</summary>
    [TestClass]
    public class VideoInfoTest
    {
        private static string path =
            "D:\\Documents and Settings\\fenix1\\OQAT\\Implementierung\\OQAT_Tests\\TestData\\sampleVideos\\bus_cif.yuv";

        ///<summary>
        ///Constructor test
        ///</summary>
        [TestMethod]
        public void constructorTest()
        {
            YuvVideoInfo info1 = new YuvVideoInfo();
            info1.width = 352;
            info1.height = 288;
            info1.yuvFormat = YuvFormat.YUV420_IYUV;
            YuvVideoInfo info2 = new YuvVideoInfo(path);
            Assert.AreEqual(info1.width, info2.width);
            Assert.AreEqual(info1.height, info2.height);
            Assert.AreEqual(info1.yuvFormat, info2.yuvFormat);
            Assert.IsTrue(info1.Equals(info2));
        }

        ///<summary>
        ///Test for the clone method
        ///</summary>
        [TestMethod]
        public void cloneTest()
        {
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
            YuvVideoInfo info = new YuvVideoInfo(path);
            Assert.AreEqual(150, info.frameCount);
            YuvVideoInfo falseInfo = new YuvVideoInfo("D:\\bla_cif.yuv");
            Assert.AreEqual(-1, falseInfo.frameCount);
        }
    }
}
