using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Oqat.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading;

namespace OQAT_Tests
{
    /// <summary>
    /// Test class for VM_VidImportOptionsDialog
    /// </summary>
    [TestClass]
    public class VidImportDialogTest
    {
        private static string path1;
        private static string path2;
        private TestContext testContextInstance;
        private static string sampleVideosPath;
        private static string[] sampleVideos;
        private static string plPathSolution;

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            plPathSolution = testContext.TestRunDirectory + "\\..\\..\\Oqat\\bin\\Debug\\Plugins";
            sampleVideosPath = testContext.TestDir + "\\..\\..\\Oqat_Tests\\TestData\\sampleVideos";
            string[] plugins = Directory.GetFiles(plPathSolution, "*.dll");

            sampleVideos = Directory.GetFiles(sampleVideosPath, "*.yuv");
            path1 = sampleVideos[0];
            // Warning: video settings might have to be set manually in test methods!
            path2 = sampleVideos[2];

            // we are not testing 
            if (!Directory.Exists(testContext.TestRunDirectory + "\\Out\\Plugins"))
                Directory.CreateDirectory(testContext.TestRunDirectory + "\\Out\\Plugins");

            foreach (string s in plugins)
            {
                string targetpath = testContext.TestRunDirectory + "\\Out\\Plugins\\" + Path.GetFileName(s);
                if (!File.Exists(targetpath))
                    File.Copy(s, targetpath);
            }
        }

        /// <summary>
        /// Constructor test
        /// </summary>
        [TestMethod]
        public void constructorTest()
        {
            var pm = PluginManager.pluginManager;
            Thread.Sleep(30000);
            StringCollection sc = new StringCollection();
            sc.Add(path1);
            sc.Add(path2);
            VM_VidImportOptionsDialog optdial = new VM_VidImportOptionsDialog(sc);
            Assert.AreEqual(2, optdial.videoList.Count);
        }
    }
}
