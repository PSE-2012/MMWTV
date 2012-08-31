using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Oqat.ViewModel;
using Oqat.PublicRessources.Model;
using Oqat.Model;
using System.Collections;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OQAT_Tests
{
    /// <summary>
    /// Class for testing the Welcome View.
    /// Missing some very minor test cases, like double click selection.
    /// </summary>
    [TestClass]
    public class WelcomeViewTest
    {
        private static string projectPath =
            "D:/Documents and Settings/fenix1/myOqatPrj.oqatPrj";

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
            Assert.AreEqual("3", (string)welcome.listBox1.Items[0]);
            Assert.AreEqual("2", (string)welcome.listBox1.Items[1]);
            Assert.AreEqual("1", (string)welcome.listBox1.Items[2]);
        }

        /// <summary>
        /// The purpose of this test method is to let the tester manually
        /// create a new project and automatically check if it was added to
        /// the recently used projects list. The Welcome View doesn't do
        /// anything else other than adding the project path as a string to 
        /// the recently used projects - creating a valid project file is 
        /// the job of another VM.
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
        /// The purpose of this test method is to let the tester manually
        /// choose an existing project for opening and automatically check 
        /// if it was added to the recently used projects list. The Welcome
        /// View doesn't do anything else other than adding the project path 
        /// as a string to the recently used projects - choosing a valid
        /// project file is the job of the Project Open Dialog VM.
        /// </summary>
        [TestMethod]
        public void openProjectTest()
        {
            VM_Welcome_Accessor welcome = new VM_Welcome_Accessor();
            int projectCount = welcome.projects.Count;
            welcome.exPrjOpen_Click(this, null);
            // open project manually
            Assert.AreEqual(projectCount + 1, welcome.projects.Count);
        }

        /// <summary>
        /// The purpose of this test method is to add project paths to the
        /// recently used projects, both valid and invalid paths, and test
        /// what happens when selecting them for opening. The method tests
        /// everything automatically, but the tester must make sure an
        /// error message for the non-existant project is displayed during 
        /// testing.
        /// </summary>
        [TestMethod]
        public void openSelProjTest()
        {
            VM_Welcome_Accessor welcome = new VM_Welcome_Accessor();
            welcome.projects.Add("2"); // in reality this should only be a
            welcome.projects.Add("3"); // path to a recently used project that
            welcome.updateListBox(); // has been deleted from disk
            int projectCount = welcome.projects.Count;
            welcome.listBox1.SelectedItem = welcome.listBox1.Items[0];
            welcome.listView1_SelectionChanged(this, null); // "3" was selected
            welcome.btnOpSelPrj_Click(this, null);
            // attempt to open non-existant project: expect error message
            // welcome view will then remove it from list
            Assert.AreEqual(projectCount - 1, welcome.projects.Count);
            Assert.AreEqual("2", welcome.listBox1.Items[0]);
            welcome.projects.Add(projectPath);
            welcome.updateListBox();
            projectCount = welcome.projects.Count;
            welcome.listBox1.SelectedItem = welcome.listBox1.Items[0];
            welcome.listView1_SelectionChanged(this, null);
            welcome.btnOpSelPrj_Click(this, null);
            // attempt to open existing project
            Assert.AreEqual(projectCount, welcome.projects.Count);
        }

        /// <summary>
        /// Memento test
        /// </summary>
        [TestMethod]
        public void mementoTest()
        {
            VM_Welcome_Accessor welcome = new VM_Welcome_Accessor();
            ArrayList state = new ArrayList();
            for (int i = 0; i < 17; i++)
            {
                state.Add(i.ToString());
            }
            Memento mem = new Memento("testmem", state);
            welcome.setMemento(mem);
            ArrayList projectList = (ArrayList)welcome.getMemento().state;
            Assert.AreEqual(15, projectList.Count);
            // if project list contains more than 15 projects,
            // first in first out is done until project list contains 15
            for (int i = 0; i < 15; i++)
            {
                Assert.AreEqual((i+2).ToString(), projectList[i]);
            }
        }
    }
}
