using PM_PSNR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using Oqat.PublicRessources.Plugin;
using System.Collections.Generic;
using Oqat.PublicRessources.Model;
using System.Windows.Controls;

namespace OQAT_Tests
{
    
    
    /// <summary>
    ///Dies ist eine Testklasse für "PSNRTest" und soll
    ///alle PSNRTest Komponententests enthalten.
    ///</summary>
    [TestClass()]
    public class PSNRTest
    {
        private static Bitmap refBitmap;
        private static Bitmap procBitmap;
        private static Bitmap analysedBitmap;
        private static AnalysisInfo analysisInfo;

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
            PSNR testMSE = new PSNR();
            refBitmap = new Bitmap(100, 100);
            for (int height = 0; height < refBitmap.Height; height++)
            {
                for (int width = 0; width < refBitmap.Width; width++)
                {
                    refBitmap.SetPixel(width, height, Color.White);
                    width++;
                    refBitmap.SetPixel(width, height, Color.Black);
                    width++;
                    refBitmap.SetPixel(width, height, Color.Red);
                    width++;
                    refBitmap.SetPixel(width, height, Color.Green);
                    width++;
                    refBitmap.SetPixel(width, height, Color.Blue);
                }
            }

            procBitmap = new Bitmap(100, 100);
            for (int width = 0; width < refBitmap.Width; width++)
            {
                for (int height = 0; height < procBitmap.Height; height++)
                {
                    procBitmap.SetPixel(width, height, Color.White);
                    height++;
                    procBitmap.SetPixel(width, height, Color.Black);
                    height++;
                    procBitmap.SetPixel(width, height, Color.Red);
                    height++;
                    procBitmap.SetPixel(width, height, Color.Green);
                    height++;
                    procBitmap.SetPixel(width, height, Color.Blue);
                }
            }
            analysisInfo = testMSE.analyse(refBitmap, procBitmap);
            analysedBitmap = analysisInfo.frame;
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
        ///Test "PSNR-Constructor"
        ///</summary>
        [TestMethod()]
        public void PSNRConstructorTest()
        {
            PSNR target = new PSNR();
            Assert.IsTrue(target is PSNR, "The returned object is not a valid PSNR instance.");
        }

        /// <summary>
        ///Test "analyse"
        ///</summary>
        [TestMethod()]
        public void analyseTest()
        {
            PSNR target = new PSNR();
            Bitmap frameRef = refBitmap;
            Bitmap frameProc = procBitmap;
            AnalysisInfo expected = analysisInfo;
            AnalysisInfo actual;
            actual = target.analyse(frameRef, frameProc);

            //Check every Pixel
            for (int height = 0; height < expected.frame.Height; height++)
            {
                for (int width = 0; width < expected.frame.Width; width++)
                {
                    Assert.AreEqual(expected.frame.GetPixel(height, width), actual.frame.GetPixel(height, width), "Analyse is working randomly");
                }
            }
            //Check Values
            for (int floats = 0; floats < expected.values.GetLength(0); floats++)
            {
                Assert.AreEqual(expected.values[floats], actual.values[floats]);
            }
        }

        /// <summary>
        ///Test "getMemento"
        ///</summary>
        [TestMethod()]
        public void getMementoTest()
        {
            PSNR target = new PSNR();
            Memento expected = new Memento("testMemento", target);
            Memento actual;
            actual = target.getMemento();
            Assert.AreEqual(expected.state, actual.state);
        }

        /// <summary>
        ///Test "namePlugin"
        ///</summary>
        [TestMethod()]
        public void namePluginTest()
        {
            PSNR target = new PSNR();
            string expected = "PSNR";
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
            PSNR target = new PSNR();
            UserControl actual;
            actual = target.propertyView;
        }

        /// <summary>
        ///Test "type"
        ///</summary>
        [TestMethod()]
        public void typeTest()
        {
            PSNR target = new PSNR();
            PluginType expected = PluginType.IMetricOqat;
            PluginType actual;
            target.type = expected;
            actual = target.type;
            Assert.AreEqual(expected, actual);
        }
    }
}
