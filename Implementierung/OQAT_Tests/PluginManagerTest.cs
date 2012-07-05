using Oqat.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Oqat.PublicRessources.Model;
using System.Collections.Generic;
using Oqat.PublicRessources.Plugin;

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
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Mit ClassCleanup führen Sie Code aus, nachdem alle Tests in einer Klasse ausgeführt wurden.
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Mit TestInitialize können Sie vor jedem einzelnen Test Code ausführen.
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Mit TestCleanup können Sie nach jedem einzelnen Test Code ausführen.
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///Ein Test für "pluginManager"
        ///</summary>
        [TestMethod()]
        public void pluginManagerTest()
        {
            PluginManager actual;
            PluginManager.OqatInfo += onSomeError;
            PluginManager.OqatPanic += onSomeError;
            PluginManager.OqatFailure += onSomeError;
            actual = PluginManager.pluginManager;
            Assert.IsNotNull(actual);
        }

        private void onSomeError(Object sender, ErrorEventArgs e)
        {
            Assert.Fail(e.GetException().Message);
        }

        /// <summary>
        ///Ein Test für "getMemento"
        ///</summary>
        [TestMethod()]
        public void getMementoTest()
        {
            PluginManager_Accessor target = new PluginManager_Accessor(); // TODO: Passenden Wert initialisieren
            string namePlugin = string.Empty; // TODO: Passenden Wert initialisieren
            string nameMemento = string.Empty; // TODO: Passenden Wert initialisieren
            Memento expected = null; // TODO: Passenden Wert initialisieren
            Memento actual;
            actual = target.getMemento(namePlugin, nameMemento);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Überprüfen Sie die Richtigkeit dieser Testmethode.");
        }

        /// <summary>
        ///Ein Test für "getMementoNames"
        ///</summary>
        [TestMethod()]
        public void getMementoNamesTest()
        {
            PluginManager_Accessor target = new PluginManager_Accessor(); // TODO: Passenden Wert initialisieren
            string namePlugin = string.Empty; // TODO: Passenden Wert initialisieren
            List<string> expected = null; // TODO: Passenden Wert initialisieren
            List<string> actual;
            actual = target.getMementoNames(namePlugin);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Überprüfen Sie die Richtigkeit dieser Testmethode.");
        }

        /// <summary>
        ///Ein Test für "getPlugin"
        ///</summary>
        public void getPluginTestHelper<T>()
        {
            PluginManager_Accessor target = new PluginManager_Accessor(); // TODO: Passenden Wert initialisieren
            string namePlugin = string.Empty; // TODO: Passenden Wert initialisieren
            T expected = default(T); // TODO: Passenden Wert initialisieren
            T actual;
            actual = target.getPlugin<T>(namePlugin);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Überprüfen Sie die Richtigkeit dieser Testmethode.");
        }

        [TestMethod()]
        public void getPluginTest()
        {
            getPluginTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///Ein Test für "getPluginNames"
        ///</summary>
        [TestMethod()]
        public void getPluginNamesTest()
        {
            PluginManager_Accessor target = new PluginManager_Accessor(); // TODO: Passenden Wert initialisieren
            PluginType type = new PluginType(); // TODO: Passenden Wert initialisieren
            List<string> expected = null; // TODO: Passenden Wert initialisieren
            List<string> actual;
            actual = target.getPluginNames(type);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Überprüfen Sie die Richtigkeit dieser Testmethode.");
        }
    }
}
