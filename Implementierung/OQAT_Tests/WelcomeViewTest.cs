using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Oqat.ViewModel;
using Oqat.PublicRessources.Model;
using Oqat.Model;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OQAT_Tests
{
    /// <summary>
    /// Class for testing the Welcome View
    /// </summary>
    [TestClass]
    public class WelcomeViewTest
    {
        /// <summary>
        /// Creates a new welcome view and tests whether the displayed
        /// project listbox matches the array of recently used projects.
        /// </summary>
        [TestMethod]
        public void constructorTest()
        {
            VM_Welcome_Accessor welcome = new VM_Welcome_Accessor();
            Assert.IsNotNull(welcome.projects);
            Assert.AreEqual(welcome.listBox1.Items.Count, welcome.projects.Count);
            welcome.projects.Clear();
            welcome.projects.Add("1");
            welcome.projects.Add("2");
            welcome.projects.Add("3");
            welcome.updateListBox();
            Assert.AreEqual("3", (string) welcome.listBox1.Items[0]);
            Assert.AreEqual("2", (string)welcome.listBox1.Items[1]);
            Assert.AreEqual("1", (string)welcome.listBox1.Items[2]);
        }

        /// <summary>
        /// The purpose of this test method is to let the tester manually
        /// create a new project and automatically check if it was added to
        /// the recently used projects list.
        /// </summary>
        [TestMethod]
        public void addProjectTest()
        {
            VM_Welcome_Accessor welcome = new VM_Welcome_Accessor();
            int projectCount = welcome.projects.Count;
            welcome.newPrjCreate_Click(this, null);
            // create new project manually
            Assert.AreEqual(projectCount + 1, welcome.projects.Count);
        }

        /// <summary>
        /// Memento test
        /// </summary>
        [TestMethod]
        public void mementoTest()
        {
            VM_Welcome_Accessor welcome = new VM_Welcome_Accessor();
            
        }
    }
}
