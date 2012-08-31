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
        private TestContext testContextInstance;
        private static string testPluginPath;
        private static string currentPath;

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

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            var pm = PluginManager.pluginManager;
            currentPath = pm.PLUGIN_PATH;
            testPluginPath =
                "D:\\Documents and Settings\\fenix1\\OQAT\\Implementierung\\OQAT\\bin\\debug\\Plugins";
            string[] plugins = Directory.GetFiles(testPluginPath);
            foreach (string s in plugins)
            {
                string[] pathSplit = s.Split('\\');
                string name = pathSplit[pathSplit.Length - 1];
                File.Copy(testPluginPath + "\\" + name, currentPath + "\\" + name);
            }
        }

        /// <summary>
        ///A test for the constructor of Video, as well as all public
        ///getters and setters.
        ///</summary>
        [TestMethod()]
        public void constructorTest()
        {
            YuvVideoInfo info = new YuvVideoInfo(path);
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
            Assert.AreEqual(metrics, target.frameMetricValue);
        }

        /// <summary>
        ///A test for the handler property
        ///</summary>
        [TestMethod()]
        public void getVideoHandlerTest()
        {
            Thread.Sleep(30000); // pluginmanager needs time for consistency check
            YuvVideoInfo info = new YuvVideoInfo(path);
            Video target = new Video(false, path, info, null);
            IVideoHandler expected = new YuvVideoHandler();
            expected.setReadContext(path, info);
            IVideoHandler actual = target.handler;
            Assert.AreEqual(expected.readPath, actual.readPath);
            Assert.AreEqual(expected.readVidInfo, actual.readVidInfo);
            IVideoHandler extra = target.getExtraHandler();
            Assert.AreEqual(extra.readPath, actual.readPath);
            Assert.AreEqual(extra.readVidInfo, actual.readVidInfo);
            Video target2 = new Video(false, path, null, null);
            IVideoHandler actual2 = target2.handler;
            string falsePath 
                = "D:\\Documents and Settings\\fenix1\\OQAT\\Implementierung\\OQAT_Tests\\TestData\\sampleVideos\\about.txt";
            Video fakeVideo = new Video(false, falsePath, null, null);
            try
            {
                IVideoHandler noHandler = fakeVideo.handler;
                Assert.Fail("no exception thrown");
            }
            catch (Exception ex)
            {

            }
        }
    }
}
