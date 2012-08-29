using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using PS_YuvVideoHandler;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OQAT_Tests
{
    [TestClass]
    public class VideoInfoTest
    {
        [TestMethod]
        public void constructorTest()
        {
            YuvVideoInfo info1 = new YuvVideoInfo();
            info1.width = 352;
            info1.height = 288;
            info1.yuvFormat = YuvFormat.YUV420_IYUV;
            YuvVideoInfo info2 = new YuvVideoInfo(
                "D:\\Documents and Settings\\fenix1\\OQAT\\Implementierung\\OQAT_Tests\\TestData\\sampleVideos\\bus_cif.yuv");
            Assert.AreEqual(info1.width, info2.width);
            Assert.AreEqual(info1.height, info2.height);
            Assert.AreEqual(info1.yuvFormat, info2.yuvFormat);
            Assert.IsTrue(info1.Equals(info2));
        }

        [TestMethod]
        public void cloneTest()
        {
            YuvVideoInfo info2 = new YuvVideoInfo(
                "D:\\Documents and Settings\\fenix1\\OQAT\\Implementierung\\OQAT_Tests\\TestData\\sampleVideos\\bus_cif.yuv");
            YuvVideoInfo info1 = (YuvVideoInfo)info2.Clone();
            Assert.AreEqual(info1.width, info2.width);
            Assert.AreEqual(info1.height, info2.height);
            Assert.AreEqual(info1.yuvFormat, info2.yuvFormat);
        }

        [TestMethod]
        public void frameCountTest()
        {
            YuvVideoInfo info = new YuvVideoInfo(
                "D:\\Documents and Settings\\fenix1\\OQAT\\Implementierung\\OQAT_Tests\\TestData\\sampleVideos\\bus_cif.yuv");
            Assert.AreEqual(150, info.frameCount);
            YuvVideoInfo falseInfo = new YuvVideoInfo("D:\\bla_cif.yuv");
            Assert.AreEqual(-1, falseInfo.frameCount);
        }
    }
}
