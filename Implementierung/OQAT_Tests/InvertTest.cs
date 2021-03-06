﻿using PF_Invert;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Oqat.PublicRessources.Plugin;
using System.Collections.Generic;
using Oqat.PublicRessources.Model;
using System.Drawing;
using System.Windows.Controls;

namespace OQAT_Tests
{
    
    
    /// <summary>
    ///Dies ist eine Testklasse für "InvertTest" und soll
    ///alle InvertTest Komponententests enthalten.
    ///</summary>
    [TestClass()]
    public class InvertTest
    {
        private static Bitmap testBitmap;
        private static Bitmap processedBitmap;
        private static Memento testMemento;

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
            Invert inv = new Invert();
            testMemento = new Memento("Invert", inv);
            testBitmap = new Bitmap(100, 100);
            for (int height = 0; height < testBitmap.Height; height++)
            {
                for (int width = 0; width < testBitmap.Width; width++)
                {
                    testBitmap.SetPixel(width, height, Color.White);
                    width++;
                    testBitmap.SetPixel(width, height, Color.Black);
                    width++;
                    testBitmap.SetPixel(width, height, Color.Red);
                    width++;
                    testBitmap.SetPixel(width, height, Color.Green);
                    width++;
                    testBitmap.SetPixel(width, height, Color.Blue);
                }
            }
            processedBitmap = inv.process(testBitmap);
        }

        //Mit ClassCleanup führen Sie Code aus, nachdem alle Tests in einer Klasse ausgeführt wurden.
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{

        //}

        //Mit TestInitialize können Sie vor jedem einzelnen Test Code ausführen.
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}

        //Mit TestCleanup können Sie nach jedem einzelnen Test Code ausführen.
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///Ein Test für "Invert-Konstruktor"
        ///</summary>
        [TestMethod()]
        public void InvertConstructorTest()
        {
            Invert target = new Invert();
            Assert.IsTrue(target is Invert, "The returned object is not a valid Invert instance.");
            //TODO überprüfen ob Invert nur einmal existieren darf?
        }

        /// <summary>
        ///Test "getMemento"
        ///</summary>
        [TestMethod()]
        public void getMementoTest()
        {
            Invert target = new Invert();
            Memento actual;
            actual = target.getMemento();
            Assert.IsTrue(actual.state is Invert, "State object of the beginning Memento is not Invert");
        }

        /// <summary>
        ///Test "getMemento": memento is set at the beginning
        ///</summary>
        [TestMethod()]
        public void getMementoTest_isMemento()
        {
            Invert target = new Invert();
            Memento actual;
            actual = target.getMemento();
            Assert.IsTrue(actual is Memento, "Mememento is not set at the start.");
        }

        /// <summary>
        ///Test "process": does return inverted Bitmap
        ///</summary>
        [TestMethod()]
        public void processTest()
        {
            Bitmap frame = testBitmap;
            Invert target = new Invert();
            Bitmap expected = processedBitmap;
            Bitmap actual;
            actual = target.process(frame);
            for (int height = 0; height < expected.Height; height++)
            {
                for (int width = 0; width < expected.Width; width++)
                {
                    Assert.AreEqual(expected.GetPixel(height, width), actual.GetPixel(height, width));
                }
            }
        }

        /// <summary>
        ///Test "process": doesn't return same Bitmap
        ///</summary>
        [TestMethod()]
        public void processTest_notSameBitmap()
        {
            Invert target = new Invert();
            Bitmap frame = new Bitmap(testBitmap);
            Bitmap expected = new Bitmap(testBitmap);
            Bitmap actual;
            actual = target.process(frame);
            for (int height = 0; height < expected.Height; height++)
            {
                for (int width = 0; width < expected.Width; width++)
                {
                    Assert.AreNotEqual(expected.GetPixel(height, width), actual.GetPixel(height, width));
                }
            }
        }

        /// <summary>
        ///Test "process": empty Bitmap
        ///</summary>
        [TestMethod()]
        public void processTest_empty()
        {
            Invert target = new Invert();
            Bitmap frame = new Bitmap(15, 15);
            Bitmap expected = new Bitmap(15, 15);
            Bitmap actual;
            actual = target.process(frame);
            Assert.AreEqual(expected.GetPixel(5, 5), expected.GetPixel(5, 5));
        }

        /// <summary>
        ///Test "setMemento": empty Memento
        ///</summary>
        [TestMethod()]
        public void setMementoTest_empty()
        {
            Invert target = new Invert();
            Memento memento = null;
            Memento expected = memento;
            target.setMemento(expected);
            Assert.AreEqual(expected, null);
        }

        /// <summary>
        ///Test "setMemento": not Invert Memento
        ///</summary>
        [TestMethod()]
        public void setMementoTest_notMemento()
        {
            Invert target = new Invert();
            List<Memento> memList = new List<Memento>();
            Memento memento = new Memento("Invert", memList);
            target.setMemento(memento);
            Assert.IsTrue(target.getMemento().state is Invert, "Invert doesn't care about Memento's, it does not need any.");
        }

        /// <summary>
        ///Test "namePlugin"
        ///</summary>
        [TestMethod()]
        public void namePluginTest()
        {
            Invert target = new Invert();
            string expected = "Invert";
            string actual;
            target.namePlugin = expected;
            actual = target.namePlugin;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Ein Test für "propertyView"
        ///</summary>
        [TestMethod()]
        public void propertyViewTest()
        {
            Invert target = new Invert();
            UserControl actual;
            UserControl expected = null;
            actual = target.propertyView;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Ein Test für "type"
        ///</summary>
        [TestMethod()]
        public void typeTest()
        {
            Invert target = new Invert();
            PluginType expected = PluginType.IFilterOqat;
            PluginType actual;
            target.type = expected;
            actual = target.type;
            Assert.AreEqual(expected, actual);
        }
    }
}
