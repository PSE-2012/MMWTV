using Oqat.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Oqat.PublicRessources.Model;
using System.Collections.ObjectModel;

namespace OQAT_Tests
{
    
    
    /// <summary>
    ///Dies ist eine Testklasse für "SmartNodeTest" und soll
    ///alle SmartNodeTest Komponententests enthalten.
    ///</summary>
    [TestClass()]
    public class SmartNodeTest
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
        ///Ein Test für "Equals"
        ///</summary>
        [TestMethod()]
        public void EqualsTest()
        {
            IVideo vid = null; 
            int id = 0; 
            int idFather = 0; 
            ObservableCollection<SmartNode> smartTree = null; 
            SmartNode target = new SmartNode(vid, id, idFather, smartTree); 
            object obj = null;
            bool expected = false; 
            bool actual;
            actual = target.Equals(obj);
            Assert.AreEqual(expected, actual);
         
        }

        /// <summary>
        ///Ein Test für "SmartNode-Konstruktor"
        ///</summary>
        [TestMethod()]
        public void SmartNodeConstructorTest()
        {
            Video testvideo = new Video(true,"test/test.yuv");
            
            IVideo vid = testvideo;

            Video testvideo2 = new Video(true, "test/otto.yuv");

            IVideo vid2 = testvideo2; 
            int id = 5; 
            int idFather = 4;
           
            ObservableCollection<SmartNode> smartTree = new ObservableCollection<SmartNode>();
            smartTree.Add(new SmartNode(vid,7,3));
            SmartNode target = new SmartNode(vid, id, idFather, smartTree);
            Assert.AreEqual(target.id, 5);
            Assert.AreEqual(target.idFather, 4);
            Assert.AreEqual(target.video.vidPath, "test/test.yuv");
            Assert.AreNotEqual(target.smartTree, null);
            Assert.AreEqual(target.smartTree.Count, 1);
             SmartNode target2 = new SmartNode(vid2, 7, 6,null);
             Assert.AreEqual(target2.smartTree.Count, 0);
             target.smartTree.Add(target2);
             Assert.AreEqual(target.smartTree.Count, 2);
            //test von to String + name methode
             Assert.AreEqual(target.ToString(), "test");
             Assert.AreNotEqual(target2.ToString(), "test");
             Assert.AreEqual(target2.ToString(), "otto");
        }

        /// <summary>
        ///Ein Test für "Equals"
        ///</summary>
        [TestMethod()]
        public void EqualsTest1()
        {
            Video testvideo = new Video(true, "test/test.yuv");
            IVideo vid = testvideo;
            int id = 5;
            int idFather = 4;
            ObservableCollection<SmartNode> smartTree = null;
            SmartNode target = new SmartNode(vid, id, idFather, smartTree);

          
            IVideo vid2 = testvideo;
            int id2 = 5;
            int idFather2 = 4;
            ObservableCollection<SmartNode> smartTree2 = null;
            SmartNode target2 = new SmartNode(vid, id2, idFather2, smartTree2);

            Assert.AreEqual(true, target.Equals(target2));

            SmartNode target3 = new SmartNode(vid, 6, idFather2, smartTree2);
            Assert.AreNotEqual(target.Equals(target3), true);
            SmartNode target4 = new SmartNode(vid, id, 1337, smartTree2);
            Assert.AreNotEqual(target.Equals(target4), true);
            target2.smartTree.Add(target3);
            Assert.AreNotEqual(target.Equals(target2), true);
           
        }
    }
}
