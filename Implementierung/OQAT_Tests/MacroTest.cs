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
        ///Test "createExtraPluginInstance"
        ///</summary>
        [TestMethod()]
        public void createExtraPluginInstanceTest()
        {
            Macro target = new Macro();
            IPlugin actual;
            actual = target.createExtraPluginInstance();
            Assert.IsInstanceOfType(actual, typeof(Macro));
            Assert.AreNotEqual(target, actual);
        }

        /// <summary>
        ///Test "setMemento": null
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException), "Null memento unexpectedly allowed.")]
        public void setMementoTest_null()
        {
            Macro target = new Macro();
            Memento memento = null;
            target.setMemento(memento);
        }
        /// <summary>
        ///Test "setMemento": null state
        ///</summary>
        [TestMethod()]
        public void setMementoTest_nullstate()
        {
            Macro_Accessor target = new Macro_Accessor();

            string mementoname = "testmemento";
            Memento memento = new Memento(mementoname, null);
            target.setMemento(memento);

            Assert.AreNotEqual<string>(mementoname, target.rootEntry.mementoName, "The invalid memento was partly loaded.");
            Assert.IsNotNull(target.rootEntry.macroEntries, "Macro was set to null.");
        }
        /// <summary>
        ///Test "setMemento": state not valid
        ///</summary>
        [TestMethod()]
        public void setMementoTest_invalidstate()
        {
            Macro_Accessor target = new Macro_Accessor();

            string mementoname = "testmemento";
            Memento memento = new Memento(mementoname, 42);
            target.setMemento(memento);

            Assert.AreNotEqual<string>(mementoname, target.rootEntry.mementoName, "The invalid memento was partly loaded.");
            Assert.IsNotNull(target.rootEntry.macroEntries, "Macro was set to null.");
        }
        /// <summary>
        ///Test "setMemento": state valid
        ///</summary>
        [TestMethod()]
        public void setMementoTest_ok()
        {
            Macro_Accessor target = new Macro_Accessor();

            string mementoname = "setmementotestmemento";
            string entrymementoname = "settestMemento";
            MacroEntry macroentry = new MacroEntry("testPlugin", PluginType.IMacro, entrymementoname);
            MacroEntry subentry = new MacroEntry("testPlugin2", PluginType.IFilterOqat, "testMemento2");
            macroentry.macroEntries.Add(subentry);
            Memento memento = new Memento(mementoname, macroentry);

            target.setMemento(memento);

            Assert.AreEqual<string>(entrymementoname, target.rootEntry.mementoName, "The memento's state was not loaded.");
            Assert.AreEqual(subentry, macroentry.macroEntries[0]);
        }

        /// <summary>
        ///Test "getMemento": set before get
        ///</summary>
        [TestMethod()]
        public void getMementoTest()
        {
            Macro_Accessor target = new Macro_Accessor();

            string mementoname = "setmementotestmemento";
            MacroEntry macroentry = new MacroEntry("testPlugin", PluginType.IFilterOqat, "testMemento");
            MacroEntry subentry = new MacroEntry("testPlugin2", PluginType.IFilterOqat, "testMemento2");
            macroentry.macroEntries.Add(subentry);
            Memento memento = new Memento(mementoname, macroentry);
            target.setMemento(memento);

            Memento actual;
            actual = target.getMemento();
            Assert.IsNotNull(actual, "Memento not returned.");
            Assert.AreEqual(mementoname, actual.name);
            Assert.IsNotNull(actual.state, "Memento is empty");
            Assert.AreEqual(subentry, ((MacroEntry)actual.state).macroEntries[0]);
        }



        /// <summary>
        ///Test "namePlugin"
        ///</summary>
        [TestMethod()]
        public void namePluginTest()
        {
            Macro target = new Macro();
            string actual;
            actual = target.namePlugin;
            Assert.AreEqual<string>("Macro", actual);
        }

        /// <summary>
        ///Test "type"
        ///</summary>
        [TestMethod()]
        public void typeTest()
        {
            Macro target = new Macro();
            PluginType actual;
            actual = target.type;
            Assert.AreEqual<PluginType>(PluginType.IMacro, actual);
        }

        

        /// <summary>
        ///Test "addMacroEntry": child valid macro, parent null
        ///rootEntry should be replaced
        ///</summary>
        [TestMethod()]
        [DeploymentItem("OQAT.exe")]
        public void addMacroEntryTest_rootentry()
        {
            Macro_Accessor target = new Macro_Accessor();
            MacroEntry child = new MacroEntry("testPlugin", PluginType.IMacro, "testmemento");
            child.macroEntries.Add(new MacroEntry("testplugin2", PluginType.IFilterOqat, "testmemento2"));
            MacroEntry father = null;
            target.addMacroEntry(child, father);

            Assert.AreEqual(child.mementoName, target.rootEntry.mementoName, "Rootentry was not set correctly.");
            Assert.AreEqual(1, target.rootEntry.macroEntries.Count, "Rootentry children were not set.");
        }

        /// <summary>
        ///Test "addMacroEntry": child valid filter, parent null
        ///</summary>
        [TestMethod()]
        [DeploymentItem("OQAT.exe")]
        public void addMacroEntryTest_notRootentry()
        {
            Macro_Accessor target = new Macro_Accessor();
            MacroEntry child = new MacroEntry("testPlugin", PluginType.IFilterOqat, "testmemento");
            MacroEntry father = null;
            target.addMacroEntry(child, father);

            //expect filter not to be set as root but rather added as child
            Assert.AreNotEqual(child.mementoName, target.rootEntry.mementoName, "Filter was set as rootentry.");
            Assert.IsTrue(target.rootEntry.macroEntries.Contains(child), "Filter was not added to macro.");
        }

        /// <summary>
        ///Test "addMacroEntry": child filter add
        ///</summary>
        [TestMethod()]
        [DeploymentItem("OQAT.exe")]
        public void addMacroEntryTest_ok()
        {
            Macro_Accessor target = new Macro_Accessor();
            MacroEntry child = new MacroEntry("testPlugin", PluginType.IFilterOqat, "testmemento");
            MacroEntry father = target.rootEntry;
            target.addMacroEntry(child, father);

            Assert.IsTrue(target.rootEntry.macroEntries.Contains(child), "Filter was not added to macro.");
        }
        /// <summary>
        ///Test "addMacroEntry": child filter add with index
        ///</summary>
        [TestMethod()]
        [DeploymentItem("OQAT.exe")]
        public void addMacroEntryTest_index()
        {
            Macro_Accessor target = new Macro_Accessor();
            MacroEntry child = new MacroEntry("testPlugin", PluginType.IFilterOqat, "testmemento");
            int index = 1;
            MacroEntry father = target.rootEntry;
            target.addMacroEntry(new MacroEntry("testPlugin2", PluginType.IFilterOqat, "testmementoX"), father);
            target.addMacroEntry(new MacroEntry("testPlugin2", PluginType.IFilterOqat, "testmementoY"), father);
            target.addMacroEntry(child, father, index);

            Assert.IsTrue(target.rootEntry.macroEntries.Contains(child), "Filter was not added to macro.");
            Assert.AreEqual<MacroEntry>(target.rootEntry.macroEntries[index], child, "Filter was not added at correct index.");
        }

        /// <summary>
        ///Test "addMacroEntry": child added twice
        ///</summary>
        [TestMethod()]
        [DeploymentItem("OQAT.exe")]
        public void addMacroEntryTest_twice()
        {
            Macro_Accessor target = new Macro_Accessor();
            MacroEntry child = new MacroEntry("testPlugin", PluginType.IFilterOqat, "testmemento");
            MacroEntry father = target.rootEntry;
            target.addMacroEntry(child, father);
            target.addMacroEntry(child, father);

            Assert.IsTrue(target.rootEntry.macroEntries.Contains(child), "Filter was not added to macro.");
            Assert.AreEqual(2, target.rootEntry.macroEntries.Count);
        }

        /// <summary>
        ///Test "addMacroEntry": child added to itself
        ///
        ///</summary>
        [TestMethod()]
        [DeploymentItem("OQAT.exe")]
        [ExpectedException(typeof(ArgumentException), "Adding Macro to itself was unexpectedly allowed.")]
        public void addMacroEntryTest_self()
        {
            Macro_Accessor target = new Macro_Accessor();
            MacroEntry child = new MacroEntry("testPlugin", PluginType.IMacro, "testmemento");
            MacroEntry father = target.rootEntry;
            target.addMacroEntry(child, father);
            target.addMacroEntry(child, child);

            Assert.IsTrue(target.rootEntry.macroEntries.Contains(child), "Entry was not added to macro.");
            Assert.AreEqual(0, target.rootEntry.macroEntries[0].macroEntries.Count, "Macro was unexpectedly added to itself.");
        }



        /// <summary>
        ///Test "addMacroEntry" EventHandler
        ///</summary>
        [TestMethod()]
        public void addMacroEntryTest_eventhandler()
        {
            string mementoname = "testmemento";

            PluginManagerTest.TestHelper(new string[] { "PF_Invert.dll" });
            Oqat.ViewModel.PluginManager_Accessor pluginManager = new Oqat.ViewModel.PluginManager_Accessor();
            pluginManager.addMemento("Invert", new Memento(mementoname, new PF_Invert.Invert().getMemento()));

            Macro_Accessor target = new Macro_Accessor();
            object sender = null;
            MementoEventArgs e = new MementoEventArgs(mementoname, "Invert");
            target.addMacroEntry(sender, e);

            Assert.AreEqual(1, target.rootEntry.macroEntries.Count, "No macroentry was added.");
            Assert.AreEqual(mementoname, target.rootEntry.macroEntries[0].mementoName, "Macroentry not added by event.");
        }

        /// <summary>
        ///Test "removeMacroEntry": non existent
        ///</summary>
        [TestMethod()]
        [DeploymentItem("OQAT.exe")]
        public void removeMacroEntryTest_nonexistent()
        {
            Macro_Accessor target = new Macro_Accessor();
            MacroEntry child = new MacroEntry("testplugin", PluginType.IFilterOqat, "testmemento");
            target.addMacroEntry(child, target.rootEntry);

            MacroEntry rem = new MacroEntry("blaplugin", PluginType.IFilterOqat, "blamemento");
            target.removeMacroEntry(rem);

            Assert.IsTrue(target.rootEntry.macroEntries.Contains(child), "Entry was unexpectedly removed.");
        }

        /// <summary>
        ///Test "removeMacroEntry"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("OQAT.exe")]
        public void removeMacroEntryTest_ok()
        {
            Macro_Accessor target = new Macro_Accessor();
            MacroEntry child = new MacroEntry("testplugin", PluginType.IFilterOqat, "testmemento");
            target.addMacroEntry(child, target.rootEntry);

            target.removeMacroEntry(child);

            Assert.IsFalse(target.rootEntry.macroEntries.Contains(child), "Entry was not removed.");
        }

        /// <summary>
        ///Test "moveMacroEntry"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("OQAT.exe")]
        public void moveMacroEntryTest()
        {
            Macro_Accessor target = new Macro_Accessor();
            MacroEntry child = new MacroEntry("testplugin", PluginType.IFilterOqat, "testmemento");
            target.addMacroEntry(child, target.rootEntry);
            MacroEntry child2 = new MacroEntry("testpluginmacro", PluginType.IMacro, "testmemento2");
            target.addMacroEntry(child2, target.rootEntry);

            MacroEntry toMoveMacro = child;
            MacroEntry target1 = child2;
            int index = 0;
            target.moveMacroEntry(toMoveMacro, target1, index);

            Assert.AreEqual(1, target.rootEntry.macroEntries.Count, "Entry was not moved away.");
            Assert.IsTrue(target.rootEntry.macroEntries[0].macroEntries.Contains(child), "Entry was not moved to new parent.");
        }

        /// <summary>
        ///Test "clearMacroEntryList"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("OQAT.exe")]
        public void clearMacroEntryListTest()
        {
            Macro_Accessor target = new Macro_Accessor();
            target.clearMacroEntryList();
            Assert.Inconclusive("Function not implemented therefor no tests either.");
        }

    }
}
