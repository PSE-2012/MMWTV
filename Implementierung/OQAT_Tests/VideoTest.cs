using Oqat.PublicRessources.Model;
using Oqat.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Oqat.PublicRessources.Plugin;
using Oqat.ViewModel;
using PS_YuvVideoHandler;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Reflection;

namespace OQAT_Tests
{
    /// <summary>
    /// Test class for Video
    ///</summary>
    [TestClass()]
    public class VideoTest
    {
        private static string sampleVideosPath;
        private static string[] sampleVideos;
        private static string plPathSolution;

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {

            plPathSolution = testContext.TestRunDirectory +  "\\..\\..\\Oqat\\bin\\Debug\\Plugins" ;
            sampleVideosPath = testContext.TestDir + "\\..\\..\\Oqat_Tests\\TestData\\sampleVideos";
            string[] plugins = Directory.GetFiles(plPathSolution, "*.dll");
            sampleVideos = Directory.GetFiles(sampleVideosPath, "*.yuv");
            // we are not testing 
            if(!Directory.Exists(testContext.TestRunDirectory + "\\Out\\Plugins"))
                Directory.CreateDirectory(testContext.TestRunDirectory + "\\Out\\Plugins");

            foreach (string s in plugins)
            {
                File.Copy(s, testContext.TestRunDirectory + "\\Out\\Plugins\\" + Path.GetFileName(s));
            }
        }

        /// <summary>
        ///A test for the constructor of Video, as well as all public
        ///getters and setters.
        ///</summary>
        [TestMethod()]
        public void constructorTest()
        {
            YuvVideoInfo info = new YuvVideoInfo(sampleVideos[2]);
            bool isana = false;
            Video target = new Video(false, sampleVideos[2], info, null);
            Assert.AreEqual(sampleVideos[2], target.vidPath);
            Assert.AreEqual(isana, target.isAnalysis);
            Assert.AreEqual(info, target.vidInfo);
            Dictionary<PresentationPluginType, List<string>> er 
                = new Dictionary<PresentationPluginType, List<string>>();
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
            Assert.AreEqual(metrics, target.frameMetricValue);
        }

        /// <summary>
        ///A test for the handler property
        ///</summary>
        [TestMethod()]
        public void getVideoHandlerTest()
        {
            Thread.Sleep(30000); // pluginmanager needs time for consistency check
            YuvVideoInfo info = new YuvVideoInfo(sampleVideos[2]);
            Video target = new Video(false, sampleVideos[2], info, null);
            IVideoHandler expected = new YuvVideoHandler();
            expected.setReadContext(sampleVideos[2], info);
            IVideoHandler actual = target.handler;
            Assert.AreEqual(expected.readPath, actual.readPath);
            Assert.AreEqual(expected.readVidInfo, actual.readVidInfo);
            IVideoHandler extra = target.getExtraHandler();
            Assert.AreEqual(extra.readPath, actual.readPath);
            Assert.AreEqual(extra.readVidInfo, actual.readVidInfo);
            Video target2 = new Video(false, sampleVideos[2], null, null);
            IVideoHandler actual2 = target2.handler;
            string falsePath 
                = "D:\\Documents and Settings\\fenix1\\OQAT\\Implementierung\\OQAT_Tests\\TestData\\sampleVideos\\about.txt";
            Video fakeVideo = new Video(false, falsePath, null, null);
            try
            {
                IVideoHandler noHandler = fakeVideo.handler;
                Assert.Fail("no exception thrown");
            }
            catch (Exception)
            {

            }
        }
    }
}
