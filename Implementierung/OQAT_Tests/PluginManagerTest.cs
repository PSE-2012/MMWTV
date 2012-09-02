using Oqat.ViewModel;
using Oqat.PublicRessources.Plugin;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Collections.Generic;
using Oqat.PublicRessources.Model;

namespace OQAT_Tests
{
    
    
    /// <summary>
    ///Dies ist eine Testklasse für "PluginManagerTest" und soll
    ///alle PluginManagerTest Komponententests enthalten.
    ///</summary>
    ///<remarks>
    ///There can be issues in some tests because the classes interact with the filesystem and 
    ///some reset operations might be locked by instances of PluginManager.
    ///In that case run only one test at a time.
    ///</remarks>
    [TestClass()]
    public class PluginManagerTest
    {

        public static string testPluginPath = PluginManager.PLUGIN_PATH + "\\..\\..\\..\\..\\OQAT_Tests\\TestData\\testPlugins";
        private static string currentPath = PluginManager.PLUGIN_PATH;

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
            if (!Directory.Exists(currentPath))
                Directory.CreateDirectory(currentPath);
        }
        
        //Mit ClassCleanup führen Sie Code aus, nachdem alle Tests in einer Klasse ausgeführt wurden.
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            try
            {
                Directory.Delete(currentPath, true);
            }
            catch { }
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
        }
        
        #endregion




        public static void TestHelper(string[] plugins)
        {
            if (!Directory.Exists(currentPath))
                Directory.CreateDirectory(currentPath);

            foreach(string file in plugins)
            {
                if(!File.Exists(currentPath + "\\" + file))
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
        public void LoadPluginTest_noPluginDir()
        {
            if (Directory.Exists(currentPath))
            {
                try
                {
                    Directory.Delete(currentPath);
                }
                catch
                {
                    Assert.Inconclusive("Plugin directory could not be deleted in order to prepare the test. Please rerun this test alone.");
                }
            }

            PluginManager_Accessor target = new PluginManager_Accessor();

            Assert.IsTrue(Directory.Exists(PluginManager.PLUGIN_PATH), "Plugin directory was not created.");
        }

        /// <summary>
        /// Test Initialization: invalid dll
        /// </summary>
        [TestMethod()]
        [DeploymentItem("OQAT.exe")]
        public void LoadPluginTest_notDLL()
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
        public void LoadPluginTest_notPluginDLL()
        {
            TestHelper(new string[] { "noPluginDLL.dll" });

            PluginManager_Accessor target = new PluginManager_Accessor();
            //assert: no exceptions
        }

        /// <summary>
        /// Test Initialization: plugin dll, outdated interface IPlugin
        /// </summary>
        [TestMethod()]
        [DeploymentItem("OQAT.exe")]
        public void LoadPluginTest_brokenInterface()
        {
            //TestHelper(new string[] { "PF_Invert_brokenIPlugin.dll", "PF_Greyscale.dll" });

            //PluginManager_Accessor target = new PluginManager_Accessor();
            ////assert: no exceptions

            ////try to get a different plugin
            //IPlugin plugin = target.getPlugin<IPlugin>("Greyscale");
            //Assert.IsNotNull(plugin);

            //IPlugin pluginBroken = target.getPlugin<IPlugin>("Invert");
            //Assert.IsNull(pluginBroken);

            Assert.Inconclusive("Test deactivated, so it does not break other tests. Please run this test alone.");
        }

        /// <summary>
        /// Test Initialization: plugin dll, outdated interface IFilterOqat
        /// </summary>
        [TestMethod()]
        [DeploymentItem("OQAT.exe")]
        public void LoadPluginTest_brokenInterface2()
        {
            //TestHelper(new string[] { "PF_Invert_brokenIFilter.dll" });

            //PluginManager_Accessor target = new PluginManager_Accessor();

            ////try to get a different plugin
            //IPlugin plugin = target.getPlugin<IPlugin>("Greyscale");
            //Assert.IsNotNull(plugin);

            //IPlugin pluginBroken = target.getPlugin<IPlugin>("Invert");
            //Assert.IsNull(pluginBroken);

            Assert.Inconclusive("Test deactivated, so it does not break other tests. Please run this test alone.");
        }

        /// <summary>
        /// Test Initialization: add plugins
        /// </summary>
        [TestMethod()]
        [DeploymentItem("OQAT.exe")]
        public void LoadPluginTest_ok()
        {
            TestHelper(new string[] { "PM_MSE.dll", "PM_PSNR.dll" });

            PluginManager_Accessor target = new PluginManager_Accessor();

            IPlugin plugin = target.getPlugin<IPlugin>("MSE");
            IPlugin plugin2 = target.getPlugin<IPlugin>("PSNR");

            Assert.IsNotNull(plugin);
            Assert.IsNotNull(plugin2);
        }


        /// <summary>
        /// Test Initialization: add Plugin dll after loading
        /// </summary>
        [TestMethod()]
        [DeploymentItem("OQAT.exe")]
        public void LoadPluginTest_later()
        {
            PluginManager_Accessor target = new PluginManager_Accessor();

            IPlugin before = target.getPlugin<IPlugin>("RelativeColor");
            Assert.IsNull(before, "Inconclusive: Plugin already available before adding dll.");

            TestHelper(new string[] { "PF_RelativeColor.dll" });

            IPlugin plugin = target.getPlugin<IPlugin>("RelativeColor");
            Assert.IsNotNull(plugin, "Plugin was not loaded properly");
        }

        /// <summary>
        /// Test Initialization: add duplicate Plugin
        /// </summary>
        [TestMethod()]
        [DeploymentItem("OQAT.exe")]
        public void LoadPluginTest_duplicates()
        {
            TestHelper(new string[] { "PF_Greyscale.dll", "PF_Greyscale_copy.dll" });

            PluginManager_Accessor target = new PluginManager_Accessor();

            IPlugin plugin = target.getPlugin<IPlugin>("Greyscale");
            Assert.IsNotNull(plugin, "Plugin was not loaded properly");
            //wether it is loaded twice does not really matter
        }





        #endregion


        #region getPluginTests

        /// <summary>
        ///Test "getPluginNames": ok
        ///</summary>
        [TestMethod()]
        public void getPluginNamesTest()
        {
            TestHelper(new string[] { "PP_Player.dll", "PP_Diagramm.dll" });

            PluginManager_Accessor target = new PluginManager_Accessor();
            List<string> actual;
            actual = target.getPluginNames(PluginType.IPresentation);
            Assert.AreEqual(2, actual.Count, "Length of list not correct.");
            Assert.IsTrue(actual.Contains("PP_Player"), "Expected plugin not in list.");
            Assert.IsTrue(actual.Contains("PP_Diagram"), "Expected plugin not in list.");
        }

        /// <summary>
        ///Test "getPluginNames": empty list
        ///</summary>
        [TestMethod()]
        public void getPluginNamesTest_empty()
        {
            PluginManager_Accessor target = new PluginManager_Accessor();
            List<string> actual;
            actual = target.getPluginNames(PluginType.System);
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Count);
        }




        public void getPluginTestHelper_nameNull<T>()
        {
            PluginManager_Accessor target = new PluginManager_Accessor();
            string namePlugin = null;
            T expected = default(T);
            T actual;
            actual = target.getPlugin<T>(namePlugin);
            Assert.AreEqual(expected, actual);
        }

        public void getPluginTestHelper_nameEmpty<T>()
        {
            PluginManager_Accessor target = new PluginManager_Accessor();
            string namePlugin = "";
            T expected = default(T);
            T actual;
            actual = target.getPlugin<T>(namePlugin);
            Assert.AreEqual(expected, actual);
        }

        public void getPluginTestHelper_nameNot<T>()
        {
            PluginManager_Accessor target = new PluginManager_Accessor();
            string namePlugin = "So ein Plugin gibt es nicht.";
            T expected = default(T);
            T actual;
            actual = target.getPlugin<T>(namePlugin);
            Assert.AreEqual(expected, actual);
        }

        public T getPluginTestHelper_ok<T>()
        {
            PluginManager_Accessor target = new PluginManager_Accessor();
            string namePlugin = "yuvVideoHandler";
            T actual;
            actual = target.getPlugin<T>(namePlugin);

            return actual;
        }


        /// <summary>
        /// Test "getPlugin": IPlugin + name null
        /// </summary>
        [TestMethod()]
        //[ExpectedException(typeof(ArgumentException), "null for pluginname was inappropriately allowed.")]
        public void getPluginTest_null()
        {
            TestHelper(new string[] {"PS_YuvVideoHandler.dll" });

            getPluginTestHelper_nameNull<IPlugin>();
        }

        /// <summary>
        /// Test "getPlugin": IPlugin + name empty
        /// </summary>
        [TestMethod()]
        public void getPluginTest_nameEmpty()
        {
            TestHelper(new string[] { "PS_YuvVideoHandler.dll" });

            getPluginTestHelper_nameEmpty<IPlugin>();
        }

        /// <summary>
        /// Test "getPlugin": IPlugin + name not existing
        /// </summary>
        [TestMethod()]
        public void getPluginTest_nameNot()
        {
            TestHelper(new string[] { "PS_YuvVideoHandler.dll" });

            getPluginTestHelper_nameNot<IPlugin>();
        }

        /// <summary>
        /// Test "getPlugin": IPlugin + name existing
        /// </summary>
        [TestMethod()]
        public void getPluginTest_ok()
        {
            TestHelper(new string[] { "PS_YuvVideoHandler.dll" });

            IPlugin ret = getPluginTestHelper_ok<IPlugin>();
            Assert.IsNotNull(ret, "Plugin not returned");
            Assert.IsTrue(ret is IVideoHandler, "Could not cast to special type.");
        }

        /// <summary>
        /// Test "getPlugin": detail interface + name existing
        /// </summary>
        [TestMethod()]
        public void getPluginTest_okDetail()
        {
            TestHelper(new string[] { "PS_YuvVideoHandler.dll" });

            IPlugin ret = getPluginTestHelper_ok<IVideoHandler>();
            Assert.IsNotNull(ret, "Plugin not returned");
            Assert.IsTrue(ret is IVideoHandler, "Could not cast to special type.");
        }

        /// <summary>
        /// Test "getPlugin": name existing for different type
        /// </summary>
        [TestMethod()]
        public void getPluginTest_NameVsType()
        {
            TestHelper(new string[] { "PS_YuvVideoHandler.dll" });

            IPlugin ret = getPluginTestHelper_ok<IFilterOqat>();
            Assert.IsNull(ret, "Plugin unexpectedly returned.");
        }

        #endregion


        #region MementoTests

        /// <summary>
        ///Test "addMemento": pluginname null
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException), "null for pluginname was inappropriately allowed.")]
        public void addGetMementoTest_pluginnameNull()
        {
            PluginManager_Accessor target = new PluginManager_Accessor();
            string namePlugin = null;
            Memento mem = new Memento("testMemento", 42);

            target.addMemento(namePlugin, mem);
        }

        /// <summary>
        ///Test "addMemento": no plugin
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "Unknown pluginname was inappropriately allowed.")]
        public void addGetMementoTest_noPlugin()
        {
            PluginManager_Accessor target = new PluginManager_Accessor();
            string namePlugin = "So ein Plugin gibt es nicht.";
            Memento mem = new Memento("testMemento", 42);

            target.addMemento(namePlugin, mem);
        }

        /// <summary>
        ///Test "addMemento": overwrite existing memento
        ///</summary>
        [TestMethod()]
        public void addGetMementoTest_overwrite()
        {
            TestHelper(new string[] { "PM_MSE.dll" });

            PluginManager_Accessor target = new PluginManager_Accessor();
            string namePlugin = "MSE";
            string nameMemento = "testMementoOverwrite";
            object valueMemento = 42;
            Memento mem = new Memento(nameMemento, valueMemento);
            target.addMemento(namePlugin, mem);
            Memento actual = target.getMemento(namePlugin, nameMemento);
            Assert.AreEqual(valueMemento, actual.state);

            object valueMemento2 = 13;
            Memento mem2 = new Memento(nameMemento, valueMemento2);
            target.addMemento(namePlugin, mem2);
            Memento actual2 = target.getMemento(namePlugin, nameMemento);
            Assert.AreEqual(valueMemento2, actual2.state);
        }

        /// <summary>
        ///Test "addMemento": add several memento
        ///</summary>
        [TestMethod()]
        public void addGetMementoTest_more()
        {
            TestHelper(new string[] { "PM_MSE.dll" });

            PluginManager_Accessor target = new PluginManager_Accessor();
            string namePlugin = "MSE";
            string nameMemento = "testMementoMore";
            object valueMemento = 42;
            Memento mem = new Memento(nameMemento, valueMemento);
            target.addMemento(namePlugin, mem);
            object valueMemento2 = 13;
            string nameMemento2 = "testMementoMore2";
            Memento mem2 = new Memento(nameMemento2, valueMemento2);
            target.addMemento(namePlugin, mem2);

            Memento actual = target.getMemento(namePlugin, nameMemento);
            Assert.AreEqual(valueMemento, actual.state);
            Memento actual2 = target.getMemento(namePlugin, nameMemento2);
            Assert.AreEqual(valueMemento2, actual2.state);
        }



        /// <summary>
        ///Test "getMementoNames": pluginname null
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException), "null pluginname was inappropriately allowed.")]
        public void getMementoNamesTest_null()
        {
            PluginManager_Accessor target = new PluginManager_Accessor();
            string namePlugin = null;
            List<string> expected = null;
            List<string> actual;
            actual = target.getMementoNames(namePlugin);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Test "getMementoNames": pluginname empty
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "Empty pluginname was inappropriately allowed.")]
        public void getMementoNamesTest_empty()
        {
            PluginManager_Accessor target = new PluginManager_Accessor();
            string namePlugin = "";
            List<string> expected = null;
            List<string> actual;
            actual = target.getMementoNames(namePlugin);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Test "getMementoNames": pluginname unknown
        ///</summary>
        [TestMethod()]
        //TODO: Decision wether null is returned or an exception is thrown is not very consistent.
        //[ExpectedException(typeof(ArgumentException), "Unknown pluginname was inappropriately allowed.")]
        public void getMementoNamesTest_unknown()
        {
            PluginManager_Accessor target = new PluginManager_Accessor();
            string namePlugin = "Dieses Plugin gibt es nicht.";
            List<string> expected = null;
            List<string> actual;
            actual = target.getMementoNames(namePlugin);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Test "getMementoNames": no Memento for plugin
        ///</summary>
        [TestMethod()]
        public void getMementoNamesTest_noMemento()
        {
            TestHelper(new string[] { "PM_PSNR.dll" });

            PluginManager_Accessor target = new PluginManager_Accessor();
            string namePlugin = "PSNR";
            List<string> actual;
            actual = target.getMementoNames(namePlugin);

            //a default plugin is created automatically
            Assert.AreEqual(1, actual.Count);
        }

        /// <summary>
        ///Test "getMementoNames": mementos listed
        ///</summary>
        [TestMethod()]
        public void getMementoNamesTest_ok()
        {
            TestHelper(new string[] { "PM_MSE.dll" });

            PluginManager_Accessor target = new PluginManager_Accessor();
            string namePlugin = "MSE";
            string nameMemento = "MementoNamesMemento";
            Memento mem = new Memento(nameMemento, 42);
            target.addMemento(namePlugin, mem);

            List<string> actual;
            actual = target.getMementoNames(namePlugin);

            Assert.IsTrue(actual.Contains(nameMemento), "The added memento was not listed.");
        }




        /// <summary>
        ///Test "getMemento": mementoname null
        ///</summary>
        [TestMethod()]
        public void getMementoTest_memNameNull()
        {
            TestHelper(new string[] { "PM_MSE.dll" });

            PluginManager_Accessor target = new PluginManager_Accessor();
            string namePlugin = "MSE";
            string nameMemento = null;
            Memento actual = target.getMemento(namePlugin, nameMemento);
            Assert.IsNull(actual);
        }

        /// <summary>
        ///Test "getMemento": mementoname empty
        ///</summary>
        [TestMethod()]
        public void getMementoTest_memNameEmpty()
        {
            TestHelper(new string[] { "PM_MSE.dll" });

            PluginManager_Accessor target = new PluginManager_Accessor();
            string namePlugin = "MSE";
            string nameMemento = "";
            Memento actual = target.getMemento(namePlugin, nameMemento);
            Assert.IsNull(actual);
        }

        /// <summary>
        ///Test "getMemento": memento not existing
        ///</summary>
        [TestMethod()]
        public void getMementoTest_memNot()
        {
            TestHelper(new string[] { "PM_MSE.dll" });

            PluginManager_Accessor target = new PluginManager_Accessor();
            string namePlugin = "MSE";
            string nameMemento = "testMementoNotExisting";
            Memento actual = target.getMemento(namePlugin, nameMemento);
            Assert.IsNull(actual);
        }

        /// <summary>
        ///Test "getMemento": pluginname null
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException), "null pluginname was inappropriately allowed.")]
        public void getMementoTest_pNull()
        {
            PluginManager_Accessor target = new PluginManager_Accessor();
            string namePlugin = null;
            string nameMemento = "";
            Memento actual = target.getMemento(namePlugin, nameMemento);
            Assert.IsNull(actual);
        }

        /// <summary>
        ///Test "getMemento": pluginname empty
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "Empty pluginname was inappropriately allowed.")]
        public void getMementoTest_pEmpty()
        {
            PluginManager_Accessor target = new PluginManager_Accessor();
            string namePlugin = "";
            string nameMemento = "";
            Memento actual = target.getMemento(namePlugin, nameMemento);
            Assert.IsNull(actual);
        }

        /// <summary>
        ///Test "getMemento": pluginname unknown
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException), "Unknown pluginname was inappropriately allowed.")]
        public void getMementoTest_pUnknown()
        {
            PluginManager_Accessor target = new PluginManager_Accessor();
            string namePlugin = "Dieses Plugin gibt es nicht.";
            string nameMemento = "";
            Memento actual = target.getMemento(namePlugin, nameMemento);
            Assert.IsNull(actual);
        }

        #endregion



    }
}
