using Oqat.ViewModel.MacroPlugin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Oqat.PublicRessources.Plugin;
using Oqat.PublicRessources.Model;

namespace OQAT_Tests
{
    
    
    /// <summary>
    ///Dies ist eine Testklasse für "MacroTest" und soll
    ///alle MacroTest Komponententests enthalten.
    ///</summary>
    [TestClass()]
    public class MacroTest
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
        ///Ein Test für "createExtraPluginInstance"
        ///</summary>
        [TestMethod()]
        public void createExtraPluginInstanceTest()
        {
            Macro target = new Macro(); // TODO: Passenden Wert initialisieren
            IPlugin expected = null; // TODO: Passenden Wert initialisieren
            IPlugin actual;
            actual = target.createExtraPluginInstance();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Überprüfen Sie die Richtigkeit dieser Testmethode.");
        }

        /// <summary>
        ///Ein Test für "addMacroEntry"
        ///</summary>
        [TestMethod()]
        public void addMacroEntryTest()
        {
            Macro target = new Macro(); // TODO: Passenden Wert initialisieren
            object sender = null; // TODO: Passenden Wert initialisieren
            MementoEventArgs e = null; // TODO: Passenden Wert initialisieren
            target.addMacroEntry(sender, e);
            Assert.Inconclusive("Eine Methode, die keinen Wert zurückgibt, kann nicht überprüft werden.");
        }

        /// <summary>
        ///Ein Test für "addMacroEntry"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("OQAT.exe")]
        public void addMacroEntryTest1()
        {
            Macro_Accessor target = new Macro_Accessor(); // TODO: Passenden Wert initialisieren
            MacroEntry child = null; // TODO: Passenden Wert initialisieren
            MacroEntry father = null; // TODO: Passenden Wert initialisieren
            int index = 0; // TODO: Passenden Wert initialisieren
            target.addMacroEntry(child, father, index);
            Assert.Inconclusive("Eine Methode, die keinen Wert zurückgibt, kann nicht überprüft werden.");
        }

        /// <summary>
        ///Ein Test für "setMetricContext"
        ///</summary>
        [TestMethod()]
        public void setMetricContextTest()
        {
            Macro target = new Macro(); // TODO: Passenden Wert initialisieren
            IVideo vidRef = null; // TODO: Passenden Wert initialisieren
            int idProc = 0; // TODO: Passenden Wert initialisieren
            IVideo vidProc = null; // TODO: Passenden Wert initialisieren
            target.setMetricContext(vidRef, idProc, vidProc);
            Assert.Inconclusive("Eine Methode, die keinen Wert zurückgibt, kann nicht überprüft werden.");
        }

        /// <summary>
        ///Ein Test für "setMemento"
        ///</summary>
        [TestMethod()]
        public void setMementoTest()
        {
            Macro target = new Macro(); // TODO: Passenden Wert initialisieren
            Memento memento = null; // TODO: Passenden Wert initialisieren
            target.setMemento(memento);
            Assert.Inconclusive("Eine Methode, die keinen Wert zurückgibt, kann nicht überprüft werden.");
        }

        /// <summary>
        ///Ein Test für "setFilterContext"
        ///</summary>
        [TestMethod()]
        public void setFilterContextTest()
        {
            Macro target = new Macro(); // TODO: Passenden Wert initialisieren
            int idRef = 0; // TODO: Passenden Wert initialisieren
            IVideo vidRef = null; // TODO: Passenden Wert initialisieren
            target.setFilterContext(idRef, vidRef);
            Assert.Inconclusive("Eine Methode, die keinen Wert zurückgibt, kann nicht überprüft werden.");
        }

        /// <summary>
        ///Ein Test für "moveMacroEntry"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("OQAT.exe")]
        public void moveMacroEntryTest()
        {
            Macro_Accessor target = new Macro_Accessor(); // TODO: Passenden Wert initialisieren
            MacroEntry toMoveMacro = null; // TODO: Passenden Wert initialisieren
            MacroEntry target1 = null; // TODO: Passenden Wert initialisieren
            int index = 0; // TODO: Passenden Wert initialisieren
            target.moveMacroEntry(toMoveMacro, target1, index);
            Assert.Inconclusive("Eine Methode, die keinen Wert zurückgibt, kann nicht überprüft werden.");
        }

        /// <summary>
        ///Ein Test für "getMemento"
        ///</summary>
        [TestMethod()]
        public void getMementoTest()
        {
            Macro target = new Macro(); // TODO: Passenden Wert initialisieren
            Memento expected = null; // TODO: Passenden Wert initialisieren
            Memento actual;
            actual = target.getMemento();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Überprüfen Sie die Richtigkeit dieser Testmethode.");
        }
    }
}
