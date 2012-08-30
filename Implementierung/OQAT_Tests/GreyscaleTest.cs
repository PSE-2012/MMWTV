using PF_Greyscale;
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
    ///Dies ist eine Testklasse für "GreyscaleTest" und soll
    ///alle GreyscaleTest Komponententests enthalten.
    ///</summary>
    [TestClass()]
    public class GreyscaleTest
    {
        private static Bitmap testBitmap;
        private static Bitmap processedBitmap;
        private static Memento original;
        private static Memento fullGrey;
        private static int testPixel = 50;

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
            Greyscale conv = new Greyscale();
            testBitmap = new Bitmap(testPixel, testPixel);
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

            //create greyscale
            double[] newColorValues = new double[3];
            for (int i = 0; i < newColorValues.GetLength(0); i++)
            {
                newColorValues[i] = 1;
            }
            fullGrey = new Memento("Blur", newColorValues);

            //get greyscaled Bitmap
            original = conv.getMemento();
            conv.setMemento(fullGrey);
            processedBitmap = conv.process(testBitmap);
            conv.setMemento(original);
        }
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
        ///Ein Test für "Greyscale-Konstruktor"
        ///</summary>
        [TestMethod()]
        public void GreyscaleConstructorTest()
        {
            Greyscale target = new Greyscale();
            Assert.IsTrue(target is Greyscale, "The returned object is not a valid Convolution instance.");
        }


        /// <summary>
        ///Ein Test für "getMemento"
        ///</summary>
        [TestMethod()]
        public void getMementoTest()
        {
            Greyscale target = new Greyscale();
            Memento expected = original;
            Memento actual;
            actual = target.getMemento();
            double[] expectedColor = (double[])expected.state;
            double[] actualColor = (double[])actual.state;
            for (int colorValue = 0; colorValue < expectedColor.GetLength(0); colorValue++)
            {
                Assert.AreEqual(expectedColor[colorValue], actualColor[colorValue], "");
            }
            Assert.AreEqual(expected.name, actual.name);
        }

        /// <summary>
        ///Ein Test für "local"
        ///</summary>
        [TestMethod()]
        public void localTest()
        {
            Greyscale target = new Greyscale();
            string s = "test";
            target.local(s);
        }

        /// <summary>
        ///Test "process"
        ///</summary>
        [TestMethod()]
        public void processTest()
        {
            Greyscale target = new Greyscale();
            Bitmap frame = testBitmap;
            Bitmap expected = processedBitmap;
            Bitmap actual;
            actual = target.process(frame);
            for (int width = 0; width < expected.Width; width++)
            {
                for (int hight = 0; hight < expected.Height; hight++)
                {
                    Assert.AreEqual(expected.GetPixel(width, hight), expected.GetPixel(width, hight), "Process working randomly. ");
                }
            }
        }

        /// <summary>
        ///Test "process": empty Bitmap
        ///</summary>
        [TestMethod()]
        public void processTest_empty()
        {
            Greyscale target = new Greyscale();
            Bitmap frame = new Bitmap(testPixel, testPixel);
            Bitmap expected = new Bitmap(testPixel, testPixel);
            Bitmap actual;
            actual = target.process(frame);
            for (int width = 0; width < expected.Width; width++)
            {
                for (int hight = 0; hight < expected.Height; hight++)
                {
                    Assert.AreEqual(expected.GetPixel(width, hight), expected.GetPixel(width, hight), "Process does not work properly. Bitmap was empty and should be empty. ");
                }
            }
        }

        /// <summary>
        ///Test "setMemento"
        ///</summary>
        [TestMethod()]
        public void setMementoTest()
        {
            Greyscale target = new Greyscale();
            Memento memento = fullGrey;
            target.setMemento(memento);
            double[] expectedColorValues = (double[])target.getMemento().state;
            for (int colorValue = 0; colorValue < expectedColorValues.GetLength(0); colorValue++)
            {
                Assert.AreEqual(expectedColorValues[colorValue], 1, "Setting the Memento did not work. ");
            }
        }

        /// <summary>
        ///Ein Test für "namePlugin"
        ///</summary>
        [TestMethod()]
        public void namePluginTest()
        {
            Greyscale target = new Greyscale();
            string expected = "PF_Greyscale";
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
            Greyscale target = new Greyscale();
            UserControl actual;
            actual = target.propertyView;
        }

        /// <summary>
        ///Ein Test für "type"
        ///</summary>
        [TestMethod()]
        public void typeTest()
        {
            Greyscale target = new Greyscale();
            PluginType expected = PluginType.IFilterOqat;
            PluginType actual;
            target.type = expected;
            actual = target.type;
            Assert.AreEqual(expected, actual);
        }
    }
}
