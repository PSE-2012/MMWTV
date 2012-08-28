using Oqat.ViewModel;
using Oqat.PublicRessources.Plugin;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace OQAT_Tests
{
    
    
    /// <summary>
    ///Dies ist eine Testklasse für "PluginManagerTest" und soll
    ///alle PluginManagerTest Komponententests enthalten.
    ///</summary>
    [TestClass()]
    public class PluginManagerTest
    {

        private static string testPluginPath;
        private static string currentPath;
        private static int assemblyPluginCount;

        private TestContext testContextInstance;

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
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            assemblyPluginCount = PluginManager.pluginManager.getPluginNames(PluginType.IPlugin).Count;

            testPluginPath = PluginManager.pluginManager.PLUGIN_PATH + "\\..\\..\\..\\..\\OQAT_Tests\\TestData\\testPlugins";
            currentPath = PluginManager.pluginManager.PLUGIN_PATH;
        }
        
        //Mit ClassCleanup führen Sie Code aus, nachdem alle Tests in einer Klasse ausgeführt wurden.
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            Directory.Delete(currentPath, true);
        }
        
        //Mit TestInitialize können Sie vor jedem einzelnen Test Code ausführen.
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Mit TestCleanup können Sie nach jedem einzelnen Test Code ausführen.
        [TestCleanup()]
        public void MyTestCleanup()
        {
            PluginManager_Accessor.plMan = null;
            if (Directory.Exists(currentPath))
            {
                try
                {
                    Directory.Delete(currentPath, true);

                }
                catch { }
            }
            Directory.CreateDirectory(currentPath);
        }
        
        #endregion





        private void TestHelper(string[] plugins)
        {
            PluginManager_Accessor.plMan = null;
            if (Directory.Exists(currentPath))
            {
                Directory.Delete(currentPath, true);
            }
            Directory.CreateDirectory(currentPath);

            foreach (string file in plugins)
            {
                File.Copy(testPluginPath + "\\" + file, currentPath + "\\" + file);
            }
        }





        /// <summary>
        /// Test "pluginManager" singleton getter
        /// PluginDir not present
        /// </summary>
        [TestMethod()]
        public void pluginManagerSingletonTest()
        {
            PluginManager actual = null;
            actual = PluginManager.pluginManager;
            Assert.IsTrue(actual is PluginManager, "The returned object is not a valid PluginManager instance.");

            PluginManager actual2 = null;
            actual2 = PluginManager.pluginManager;
            Assert.AreEqual<PluginManager>(actual, actual2, "Second call did not return reference to the same object.");
        }


        #region InitializationTests

        /// <summary>
        /// Test Initialization: no PluginDir
        /// </summary>
        [TestMethod()]
        [DeploymentItem("OQAT.exe")]
        public void PluginManagerConstructorTest_noPluginDir()
        {
            TestHelper(new string[] { });

            PluginManager_Accessor target = new PluginManager_Accessor();

            Assert.IsTrue(Directory.Exists(target.PLUGIN_PATH), "Plugin directory was not created.");
        }

        /// <summary>
        /// Test Initialization: invalid dll
        /// </summary>
        [TestMethod()]
        [DeploymentItem("OQAT.exe")]
        public void PluginManagerConstructorTest_notDLL()
        {
            TestHelper(new string[] { "noDLL.dll" });
            
            PluginManager_Accessor target = new PluginManager_Accessor();
            //assert: no exceptions
        }

        /// <summary>
        /// Test Initialization: valid dll, no plugin
        /// </summary>
        [TestMethod()]
        [DeploymentItem("OQAT.exe")]
        public void PluginManagerConstructorTest_notPluginDLL()
        {
            TestHelper(new string[] { "noPluginDLL.dll" });

            PluginManager_Accessor target = new PluginManager_Accessor();
            //assert: no exceptions
            Assert.AreEqual(assemblyPluginCount, target.getPluginNames(PluginType.IPlugin).Count);
        }

        /// <summary>
        /// Test Initialization: plugin dll, outdated interface IPlugin
        /// </summary>
        [TestMethod()]
        [DeploymentItem("OQAT.exe")]
        public void PluginManagerConstructorTest_brokenInterface()
        {
            TestHelper(new string[] { "PF_Invert_brokenIPlugin.dll" });

            PluginManager_Accessor target = new PluginManager_Accessor();
            //assert: no exceptions
            Assert.AreEqual(assemblyPluginCount, target.getPluginNames(PluginType.IPlugin).Count);
        }

        /// <summary>
        /// Test Initialization: plugin dll, outdated interface IFilterOqat
        /// </summary>
        [TestMethod()]
        [DeploymentItem("OQAT.exe")]
        public void PluginManagerConstructorTest_brokenInterface2()
        {
            TestHelper(new string[] { "PF_Invert_brokenIFilter.dll" });

            PluginManager_Accessor target = new PluginManager_Accessor();
            //assert: no exceptions
            //Assert.AreEqual(assemblyPluginCount, target.getPluginNames(PluginType.IPlugin).Count);
        }

        /// <summary>
        /// Test Initialization: add plugins
        /// </summary>
        [TestMethod()]
        [DeploymentItem("OQAT.exe")]
        public void PluginManagerConstructorTest_ok()
        {
            TestHelper(new string[] { "PF_Invert.dll", "PM_MSE.dll", "PM_PSNR.dll" });

            PluginManager_Accessor target = new PluginManager_Accessor();
            Assert.AreEqual(assemblyPluginCount+3, target.getPluginNames(PluginType.IPlugin).Count);
        }


        /// <summary>
        /// Test Initialization: add Plugin dll after loading
        /// </summary>
        [TestMethod()]
        [DeploymentItem("OQAT.exe")]
        public void PluginManagerConstructorTest_later()
        {
            PluginManager_Accessor target = new PluginManager_Accessor();

            Assert.IsNull(target.getPlugin<IPlugin>("PF_Invert"), "Plugin already available before moving dll.");

            TestHelper(new string[] { "PF_Invert.dll" });

            Assert.IsNotNull(target.getPlugin<IPlugin>("PF_Invert"), "Plugin was not loaded properly");
        }

        /// <summary>
        /// Test Initialization: add duplicate Plugin
        /// </summary>
        [TestMethod()]
        [DeploymentItem("OQAT.exe")]
        public void PluginManagerConstructorTest_duplicates()
        {
            TestHelper(new string[] { "PM_MSE.dll", "PM_MSE_copy.dll" });

            PluginManager_Accessor target = new PluginManager_Accessor();

            Assert.IsNotNull(target.getPlugin<IPlugin>("PM_MSE"), "Plugin was not loaded properly");
            //wether it is loaded twice does not really matter

            target = null;
        }

        #endregion


        #region getPluginTests

        /*
         * getPluginNames: null, not plugin type, valid type no plugins, valid type has plugins
         * 
         * getPlugin: no plugin type + existing name
         * getPlugin: plugin type + null/empty/nonexistent/valid name
         * getPlugin: existing name + not fitting type
         * getPlugin: existing name + basetype
         * 
         */

        #endregion


        #region MementoTests

        /*
         * addMemento: null/empty/nonexistent name
         * addMemento: existent plugin + mementos (file already exists/no folder/file readonly
         * 
         * getMementoNames: null/empty/nonexistent name
         * getMementoNames: existent plugin + no memento/invalid memento & valid mementos
         * 
         * getMemento: null/empty/nonexistent name
         * getMemento: null/empty/nonexistent mementoname
         * getMemento: existent plugin&mementoname + invalid/valid mementofile
         * 
         */

        #endregion
    }
}
