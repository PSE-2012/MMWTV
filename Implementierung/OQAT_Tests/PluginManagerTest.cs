//using Oqat.ViewModel;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using Oqat.PublicRessources.Model;
//using System.Collections.Generic;
//using Oqat.PublicRessources.Plugin;

//using System.IO;
//using System.Threading;
//namespace OQAT_Tests
//{
    
    
//    /// <summary>
//    ///Dies ist eine Testklasse für "PluginManagerTest" und soll
//    ///alle PluginManagerTest Komponententests enthalten.
//    ///</summary>
//    [TestClass()]
//    public class PluginManagerTest
//    {


//        private TestContext testContextInstance;

//        /// <summary>
//        ///Ruft den Testkontext auf, der Informationen
//        ///über und Funktionalität für den aktuellen Testlauf bietet, oder legt diesen fest.
//        ///</summary>
//        public TestContext TestContext
//        {
//            get
//            {
//                return testContextInstance;
//            }
//            set
//            {
//                testContextInstance = value;
//            }
//        }

//        #region Zusätzliche Testattribute
//        // 
//        //Sie können beim Verfassen Ihrer Tests die folgenden zusätzlichen Attribute verwenden:
//        //
//        // Initialize exe/plugin path and the ManualResetEvent
//        // empty plugin directory.
//        static EventWaitHandle evtObj;
//        static bool key;
//        static string exePath;
//        static string plPath;
//        static string samplePluginOriginPath;
//        static string filterSamplePath;
//        static string uncompatibleAssembly;
//        static string presentationPath;
//        static string metricPath;
//        static string secondFilterSamplePath;
//        [ClassInitialize()]
//        public static void MyClassInitialize(TestContext testContext)
//        {
//            evtObj = new ManualResetEvent(false);
//           exePath = Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(typeof(PluginManager)).Location);
//         //  exePath = Directory.GetParent(Directory.GetParent(exePath).Parent.FullName).Parent.FullName;
//         //  exePath = exePath + "\\OQAT_Tests\\bin\\Debug";
//           plPath = exePath + "\\Plugins";
//           samplePluginOriginPath = Directory.GetParent(Directory.GetParent(exePath).Parent.FullName).Parent.FullName + "\\Implementierung\\OQAT_Tests" + "\\TestData\\SamplePlugins\\bin";

//           filterSamplePath = samplePluginOriginPath + "\\FilterSample.dll";
//           Assert.IsTrue(File.Exists(filterSamplePath));

//      //     uncompatibleAssembly = samplePluginOriginPath + "\\AForge.dll";
//        //   Assert.IsTrue(File.Exists(uncompatibleAssembly));

//           presentationPath = samplePluginOriginPath + "\\PresentationSample.dll";
//           Assert.IsTrue(File.Exists(presentationPath));

//           metricPath = samplePluginOriginPath + "\\MetricSample.dll";
//           Assert.IsTrue(File.Exists(metricPath));

//           secondFilterSamplePath = samplePluginOriginPath + "\\SecondFilterSample.dll";
//           Assert.IsTrue(File.Exists(secondFilterSamplePath));
//           Assert.IsTrue(Directory.Exists(samplePluginOriginPath));   

//           string[] files;
//           if (Directory.Exists(plPath))
//           {
//               files = Directory.GetFiles(plPath);
//               foreach (string file in files)
//               {
//                   File.Delete(file);
//               }

//           }       
//        }
//        //
//        //Mit ClassCleanup führen Sie Code aus, nachdem alle Tests in einer Klasse ausgeführt wurden.
//        //[ClassCleanup()]
//        //public static void MyClassCleanup()
//        //{
//        //}
//        //
//        //Mit TestInitialize können Sie vor jedem einzelnen Test Code ausführen.
//        //[TestInitialize()]
//        //public void MyTestInitialize()
//        //{
//        //}
//        //
//        //Mit TestCleanup können Sie nach jedem einzelnen Test Code ausführen.
//        //[TestCleanup()]
//        //public void MyTestCleanup()
//        //{
//        //}
//        //
//        #endregion


//        /// <summary>
//        ///Ein Test für "pluginManager"
//        ///</summary>
//        [TestMethod()]
//        public void pluginManagerTest()
//        {
//            /// non or empty plugin folder
           
//            PluginManager actual;
//            PluginManager.OqatInfo += onSomeError;
//            PluginManager.OqatPanic += onSomeError;
//            PluginManager.OqatFailure += onSomeError;
//            PluginManager.OqatPluginTableChanged += onPluginTableChanged;
//            actual = PluginManager.pluginManager;

//            Assert.IsNotNull(actual);
//            /// should exist by now, constructor should have done it
//            /// and raised an info event
//            Assert.IsTrue(Directory.Exists(plPath));
        
//            /// first plugin in pluginFolder
//            /// pluginManager cathes the event triggered by this action
//            /// and refreshes the pluginTable
//            /// after that the pluginTableChanged Event is raised
//            /// the only listener is this TestClass.
//            copyHelper(filterSamplePath);
//            key = false;
//            while (!key)
//            {
//                evtObj.WaitOne();
//            }
//            copyHelper(presentationPath);
//            key = false;
//            while (!key)
//            {
//                evtObj.WaitOne();
//            }
//        }


//        private bool copyHelper(string ressource) 
//        {
//            File.Copy(ressource, plPath + "\\" + Path.GetFileName(ressource));
//            return false;
//        }

//        private void onPluginTableChanged(Object sender, EventArgs e) 
//        {
//            key = true;
//            evtObj.Set();
//        }

//        private void onSomeError(Object sender, ErrorEventArgs e)
//        {
//            Console.WriteLine(e.GetException().Message + "\n" + e.GetException().StackTrace + "\n");
//        }

//        ///// <summary>
//        /////Ein Test für "getMemento"
//        /////</summary>
//        //[TestMethod()]
//        //public void getMementoTest()
//        //{
//        //    PluginManager_Accessor target = new PluginManager_Accessor(); // TODO: Passenden Wert initialisieren
//        //    string namePlugin = string.Empty; // TODO: Passenden Wert initialisieren
//        //    string nameMemento = string.Empty; // TODO: Passenden Wert initialisieren
//        //    Memento expected = null; // TODO: Passenden Wert initialisieren
//        //    Memento actual;
//        //    actual = target.getMemento(namePlugin, nameMemento);
//        //    Assert.AreEqual(expected, actual);
//        //    Assert.Inconclusive("Überprüfen Sie die Richtigkeit dieser Testmethode.");
//        //}

//        ///// <summary>
//        /////Ein Test für "getMementoNames"
//        /////</summary>
//        //[TestMethod()]
//        //public void getMementoNamesTest()
//        //{
//        //    PluginManager_Accessor target = new PluginManager_Accessor(); // TODO: Passenden Wert initialisieren
//        //    string namePlugin = string.Empty; // TODO: Passenden Wert initialisieren
//        //    List<string> expected = null; // TODO: Passenden Wert initialisieren
//        //    List<string> actual;
//        //    actual = target.getMementoNames(namePlugin);
//        //    Assert.AreEqual(expected, actual);
//        //    Assert.Inconclusive("Überprüfen Sie die Richtigkeit dieser Testmethode.");
//        //}

//        /// <summary>
//        ///Ein Test für "getPlugin"
//        ///</summary>
//        public void getPluginTestHelper<T>()
//        {
//            var player = PluginManager.pluginManager.getPlugin<IPresentation>("VideoPlayer");
//            Assert.IsNotNull(player);
//        }

//        //[TestMethod()]
//        //public void getPluginTest()
//        //{
//        //    getPluginTestHelper<GenericParameterHelper>();
//        //}

//        ///// <summary>
//        /////Ein Test für "getPluginNames"
//        /////</summary>
//        //[TestMethod()]
//        //public void getPluginNamesTest()
//        //{
//        //    PluginManager_Accessor target = new PluginManager_Accessor(); // TODO: Passenden Wert initialisieren
//        //    PluginType type = new PluginType(); // TODO: Passenden Wert initialisieren
//        //    List<string> expected = null; // TODO: Passenden Wert initialisieren
//        //    List<string> actual;
//        //    actual = target.getPluginNames(type);
//        //    Assert.AreEqual(expected, actual);
//        //    Assert.Inconclusive("Überprüfen Sie die Richtigkeit dieser Testmethode.");
//        //}
//    }
//}
