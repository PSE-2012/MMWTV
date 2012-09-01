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

namespace OQAT_UITests
{
    /// <summary>
    /// Class for testing the Welcome View.
    /// Missing some minor cases, like double click selection or localisation.
    /// </summary>
    [TestClass]
    public class WelcomeViewTest
    {
        private static string projectPath =
            "D:/Documents and Settings/fenix1/myOqatPrj.oqatPrj";

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
            welcome.projects.Clear();
            // something goes wrong when running all tests at once if you
            // don't clear the list, not sure where the problem is
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

    }
}
