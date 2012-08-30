using Oqat.PublicRessources.Model;
using Oqat.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Oqat.PublicRessources.Plugin;
using PS_YuvVideoHandler;
using System.Collections.Generic;

namespace OQAT_Tests
{
    /// <summary>
    /// Test class for Video
    ///</summary>
    [TestClass()]
    public class VideoTest
    {
        private static string path =
            "D:\\Documents and Settings\\fenix1\\OQAT\\Implementierung\\OQAT_Tests\\TestData\\sampleVideos\\bus_cif.yuv";

        /// <summary>
        ///A test for the constructor of Video, as well as all public
        ///getters and setters.
        ///</summary>
        [TestMethod()]
        public void constructorTest()
        {
            YuvVideoInfo info = new YuvVideoInfo();
            info.width = 352;
            info.height = 288;
            info.yuvFormat = YuvFormat.YUV420_IYUV;
            bool isana = false;
            Video target = new Video(false, path, info, null);
            Assert.AreEqual(path, target.vidPath);
            Assert.AreEqual(isana, target.isAnalysis);
            Assert.AreEqual(info, target.vidInfo);
            Dictionary<PresentationPluginType, System.Collections.Generic.List<string>> er = new System.Collections.Generic.Dictionary<PresentationPluginType, System.Collections.Generic.List<string>>();
            List<string> li = new List<string>();
            li.Add("testcustom");
            er.Add(PresentationPluginType.Custom, li);
            target.extraResources = er;
            float[][] metrics = new float[][] { new float[] { 1, 2, 3, 4, 5 } };
            target.frameMetricValue = metrics;
            List<IMacroEntry> macros = new List<IMacroEntry>();
            target.processedBy = macros;
            Assert.AreEqual(macros, target.processedBy);
            Assert.AreEqual(er, target.extraResources);
        }

        /// <summary>
        ///A test for "getVideoHandler" - not working yet
        ///</summary>
        [TestMethod()]
        public void getVideoHandlerTest()
        {
            YuvVideoInfo info = new YuvVideoInfo(path);
            Video target = new Video(false, path, info, null);
            IVideoHandler expected = new YuvVideoHandler();
            expected.setReadContext(path, info);
            IVideoHandler actual = target.handler;
            Assert.AreEqual(expected, actual);
            YuvVideoInfo falseInfo = new YuvVideoInfo("D\\bla_cif.yuv");
            string falsePath =
                "D\\bla_cif.yuv";
            Video falseTarget = new Video(false, falsePath, falseInfo, null);
            try
            {
                IVideoHandler falseHandler = falseTarget.handler;
                Assert.Fail("no exception thrown");
            }
            catch (Exception ex)
            {

            }
        }
    }
}
