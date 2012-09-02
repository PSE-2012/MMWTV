using PF_RelativeColor;
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
    ///Dies ist eine Testklasse für "RelativeColorTest" und soll
    ///alle RelativeColorTest Komponententests enthalten.
    ///</summary>
    [TestClass()]
    public class RelativeColorTest
    {
        private static Bitmap testBitmap;
        private static Bitmap processedBitmap;
        private static Memento original;
        private static Memento doubleColor;
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
            RelativeColor relCol = new RelativeColor();
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
                newColorValues[i] = 2;
            }
            doubleColor = new Memento("2xColor", newColorValues);

            //get greyscaled Bitmap
            original = relCol.getMemento();
            relCol.setMemento(doubleColor);
            processedBitmap = relCol.process(testBitmap);
            relCol.setMemento(original);
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
        ///Ein Test für "RelativeColor-Konstruktor"
        ///</summary>
        [TestMethod()]
        public void RelativeColorConstructorTest()
        {
            RelativeColor target = new RelativeColor();
            Assert.IsTrue(target is RelativeColor, "The returned object is not a valid Invert instance.");
        }

        /// <summary>
        ///Test "getMemento"
        ///</summary>
        [TestMethod()]
        public void getMementoTest()
        {
            RelativeColor target = new RelativeColor();
            Memento expected = original;
            Memento actual;
            actual = target.getMemento();
            double[] expectedColorValues = (double[])expected.state;
            double[] actualColorValues = (double[])actual.state;
            for (int color = 0; color < actualColorValues.GetLength(0); color++)
            {
                Assert.AreEqual(expectedColorValues[color], actualColorValues[color]);
            }
        }

        /// <summary>
        ///Ein Test für "localize"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PF_RelativeColor.dll")]
        public void localizeTest()
        {
            RelativeColor_Accessor target = new RelativeColor_Accessor();
            string s = "test";
            target.localize(s);
        }

        /// <summary>
        ///Test "process"
        ///</summary>
        [TestMethod()]
        public void processTest()
        {
            RelativeColor target = new RelativeColor();
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
            RelativeColor target = new RelativeColor();
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
            RelativeColor target = new RelativeColor();
            Memento memento = doubleColor;
            target.setMemento(memento);
            double[] expectedColorValues = (double[])target.getMemento().state;
            for (int colorValue = 0; colorValue < expectedColorValues.GetLength(0); colorValue++)
            {
                Assert.AreEqual(expectedColorValues[colorValue], 2, "Setting the Memento did not work. ");
            }
        }

        /// <summary>
        ///Test "namePlugin"
        ///</summary>
        [TestMethod()]
        public void namePluginTest()
        {
            RelativeColor target = new RelativeColor();
            string expected = "RelativeColor";
            string actual;
            target.namePlugin = expected;
            actual = target.namePlugin;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Test "propertyView"
        ///</summary>
        [TestMethod()]
        public void propertyViewTest()
        {
            RelativeColor target = new RelativeColor();
            UserControl actual;
            actual = target.propertyView;
        }

        /// <summary>
        ///Test "type"
        ///</summary>
        [TestMethod()]
        public void typeTest()
        {
            RelativeColor target = new RelativeColor();
            PluginType expected = PluginType.IFilterOqat;
            PluginType actual;
            target.type = expected;
            actual = target.type;
            Assert.AreEqual(expected, actual);
        }
    }
}
