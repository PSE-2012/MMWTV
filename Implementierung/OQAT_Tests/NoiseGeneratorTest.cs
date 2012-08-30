using PF_NoiseGenerator;
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
    ///Dies ist eine Testklasse für "NoiseGeneratorTest" und soll
    ///alle NoiseGeneratorTest Komponententests enthalten.
    ///</summary>
    [TestClass()]
    public class NoiseGeneratorTest
    {
        private static Bitmap testBitmap;
        private static int testPixel = 50;
        private static Memento original;
        private static Memento testNoise;

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
            NoiseGenerator noiGen = new NoiseGenerator();
            original = noiGen.getMemento();
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
            testNoise = new Memento("test", 5.1F);
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
        ///Ein Test für "NoiseGenerator-Konstruktor"
        ///</summary>
        [TestMethod()]
        public void NoiseGeneratorConstructorTest()
        {
            NoiseGenerator target = new NoiseGenerator();
            Assert.IsTrue(target is NoiseGenerator, "The returned object is not a valid NoiseGenerator instance.");
        }

        /// <summary>
        ///Ein Test für "getMemento"
        ///</summary>
        [TestMethod()]
        public void getMementoTest()
        {
            NoiseGenerator target = new NoiseGenerator();
            Memento expected = original;
            Memento actual;
            actual = target.getMemento();
            float expectedMean = (float)expected.state;
            float actualMean = (float)actual.state;
            Assert.AreEqual(expectedMean, actualMean);
        }

        /// <summary>
        ///Ein Test für "localize"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PF_NoiseGenerator.dll")]
        public void localizeTest()
        {
            NoiseGenerator_Accessor target = new NoiseGenerator_Accessor();
            string s = "test";
            target.localize(s);
        }

        /// <summary>
        ///Test "process"
        ///</summary>
        [TestMethod()]
        public void processTest()
        {
            NoiseGenerator target = new NoiseGenerator();
            Bitmap frame = testBitmap;
            float expectedMean = (float)target.getMemento().state;
            Bitmap actual;
            actual = target.process(frame);
            float actualMean = 0;
            for (int height = 0; height < actual.Height; height++)
            {
                for (int width = 0; width < actual.Width; width++)
                {
                    if (!frame.GetPixel(width, height).Equals(actual.GetPixel(width, height)))
                    {
                        actualMean++;
                    }
                }
            }
            Assert.AreEqual(expectedMean, actualMean, "Process does not work with mean. ");
        }

        /// <summary>
        ///Ein Test für "setMemento"
        ///</summary>
        [TestMethod()]
        public void setMementoTest()
        {
            NoiseGenerator target = new NoiseGenerator();
            Memento memento = testNoise;
            target.setMemento(memento);
            float actualMean = (float)target.getMemento().state;
            float expectedMean = (float)memento.state;
            Assert.AreEqual(expectedMean, actualMean, "Memento was not set right. ");
        }

        /// <summary>
        ///Ein Test für "namePlugin"
        ///</summary>
        [TestMethod()]
        public void namePluginTest()
        {
            NoiseGenerator target = new NoiseGenerator();
            string expected = "PF_NoiseGenerator";
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
            NoiseGenerator target = new NoiseGenerator();
            UserControl actual;
            actual = target.propertyView;
        }

        /// <summary>
        ///Ein Test für "type"
        ///</summary>
        [TestMethod()]
        public void typeTest()
        {
            NoiseGenerator target = new NoiseGenerator();
            PluginType expected = PluginType.IFilterOqat;
            PluginType actual;
            target.type = expected;
            actual = target.type;
            Assert.AreEqual(expected, actual);
        }
    }
}
