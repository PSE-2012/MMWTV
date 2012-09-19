using Oqat.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Oqat.PublicRessources.Model;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OQAT_Tests
{
    
    
    /// <summary>
    ///Dies ist eine Testklasse für "ProjectTest" und soll
    ///alle ProjectTest Komponententests enthalten.
    ///</summary>
    [TestClass()]
    public class ProjectTest
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
        
        // Mit TestCleanup können Sie nach jedem einzelnen Test Code ausführen.
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        #endregion








        /// <summary>
        ///Ein Test für "Project-Konstruktor"
        ///</summary>
        [TestMethod()]
        public void ProjectConstructorTest()
        {
            Project test = new Project("myProject","test/testing/myproject.oqatProj","testprojekt zum testen");
            Assert.AreEqual(test.name, "myProject");
            Assert.AreEqual(test.path_Project, "test/testing/myproject.oqatProj");
            Assert.AreEqual(test.description, "testprojekt zum testen");
            
            Video vid1 = new Video(false,"test/testing/vid1.yuf");
            Video vid2 = new Video(false, "test/testing/vid2.yuf");
            SmartNode sm1 = new SmartNode(vid1,1,0);
            SmartNode sm2 = new SmartNode(vid2, 2, 0);
            test.smartTree.Add(sm1);
            test.smartTree.Add(sm2);
            Assert.AreEqual(test.smartTree.Count, 2);

            
           
            
        }

        /// <summary>
        ///Ein Test für "rmNode"
        ///</summary>
        [TestMethod()]
        public void rmNodeTest()
        {
            Project test = new Project("myProject", "test/testing/myproject.oqatProj", "testprojekt zum testen");
        }

        /// <summary>
        ///Ein Test für "addNode" und "rmNode"
        ///</summary>
        [TestMethod()]
        public void addNodeTest()
        {
            Project test = new Project("myProject", "test/testing/myproject.oqatProj", "testprojekt zum testen");
            Video vid1 = new Video(false, "test/testing/vid1.yuf");
            Video vid2 = new Video(false, "test/testing/vid2.yuf");
            Video vid3 = new Video(false, "test/testing/vid3.yuf");
            Video vid4 = new Video(false, "test/testing/vid3.yuf");   

            test.addNode(vid1, -1);
            Assert.AreEqual(test.smartTree.Count, 1);
            test.rmNode(0, false);
            Assert.AreEqual(test.smartTree.Count, 0);

            test.addNode(vid1, -1);
            test.addNode(vid2, 1);
            test.addNode(vid3, 2);
            Assert.AreEqual(test.smartTree.Count, 1);
            
            test.rmNode(2, false);
            Assert.AreEqual(test.smartTree.Count, 1);

            test.addNode(vid2, -1);
            test.addNode(vid2, -1);
            Assert.AreEqual(test.smartTree.Count, 3);

            Project test2 = new Project("myProject", "test/testing/myproject.oqatProj", "testprojekt zum testen");
            test2.addNode(vid1, -1);
            test2.addNode(vid2, 0);
            test2.addNode(vid3, 0);
            
            test2.rmNode(0, true);
            Assert.AreEqual(test2.smartTree.Count, 0); 

            Project test3 = new Project("myProject", "test/testing/myproject.oqatProj", "testprojekt zum testen");
            test3.addNode(vid1, -1);
            test3.addNode(vid2, 0);
            test3.addNode(vid3, 0);
          
           
            test3.rmNode(0, false);
            Assert.AreEqual(test3.smartTree.Count, 2);



            try
            {
                test.addNode(null, -1);
                Assert.Fail("no exception thrown");
            }
            catch (Exception)
            {

            }
            try
            {
                test.smartTree.Clear();
                test.addNode(vid1, 0);
                Assert.Fail("no exception thrown");
            }
            catch (Exception)
            {

            }
            try
            {
                test.smartTree.Clear();
                test.rmNode(0,true);
                Assert.Fail("no exception thrown");
            }
            catch (Exception)
            {

            }
          
          
        }
    }
}
