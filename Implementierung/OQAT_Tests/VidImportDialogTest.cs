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
        private static string path1 =
            "D:\\Documents and Settings\\fenix1\\OQAT\\Implementierung\\OQAT_Tests\\TestData\\sampleVideos\\bus_cif.yuv";
        private static string path2 =
            "D:\\Documents and Settings\\fenix1\\OQAT\\Implementierung\\OQAT_Tests\\TestData\\sampleVideos\\container_cif.yuv";
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
            currentPath = PluginManager.PLUGIN_PATH;
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
        /// Constructor test
        /// </summary>
        [TestMethod]
        public void constructorTest()
        {
            Thread.Sleep(30000);
            StringCollection sc = new StringCollection();
            sc.Add(path1);
            sc.Add(path2);
            VM_VidImportOptionsDialog optdial = new VM_VidImportOptionsDialog(sc);
            Assert.AreEqual(2, optdial.videoList.Count);
        }
    }
}
