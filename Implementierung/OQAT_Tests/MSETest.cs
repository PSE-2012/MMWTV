using PM_MSE;
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
    ///Dies ist eine Testklasse für "MSETest" und soll
    ///alle MSETest Komponententests enthalten.
    ///</summary>
    [TestClass()]
    public class MSETest
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
            MSE testMSE = new MSE();
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
        ///Ein Test für "MSE-Konstruktor"
        ///</summary>
        [TestMethod()]
        public void MSEConstructorTest()
        {
            MSE target = new MSE();
            Assert.IsTrue(target is MSE, "The returned object is not a valid MSE instance.");
        }

        /// <summary>
        ///Ein Test für "analyse"
        ///</summary>
        [TestMethod()]
        public void analyseTest()
        {
            MSE target = new MSE();
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
        ///Test "analyse": Bitmap's null
        ///</summary>
        [TestMethod()]
        public void analyseTest_null()
        {
            MSE target = new MSE();
            Bitmap frameRef = null;
            Bitmap frameProc = null;
            AnalysisInfo actual;
            actual = target.analyse(frameRef, frameProc);
            Assert.IsNull(actual, "analyse can not handle null.");
        }

        /// <summary>
        ///Test "analyse": Bitmap's empty
        ///</summary>
        [TestMethod()]
        public void analyseTest_empty()
        {
            MSE target = new MSE();
            Bitmap frameRef = new Bitmap(15, 15);
            Bitmap frameProc = new Bitmap(15, 15);
            AnalysisInfo actual;
            actual = target.analyse(frameRef, frameProc);
            Assert.IsTrue(actual is AnalysisInfo, "analyse can not handle empty Bitmaps.");
        }

        /// <summary>
        ///Test "getMemento": start Mememento is set.
        ///</summary>
        [TestMethod()]
        public void getMementoTest()
        {
            MSE target = new MSE();
            Memento expected = new Memento("PM_MSE", 0);
            Memento actual;
            actual = target.getMemento();
            Assert.AreEqual(expected.state, actual.state);
            Assert.AreEqual(expected.name, actual.name);
        }

        /// <summary>
        ///Ein Test für "localize"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PM_MSE.dll")]
        public void localizeTest()
        {
            MSE_Accessor target = new MSE_Accessor();
            string s = string.Empty; // TODO: Passenden Wert initialisieren
            target.localize(s);
            Assert.Inconclusive("Eine Methode, die keinen Wert zurückgibt, kann nicht überprüft werden.");
        }

        /// <summary>
        ///Test "setMemento"
        ///</summary>
        [TestMethod()]
        public void setMementoTest()
        {
            MSE_Accessor target = new MSE_Accessor();
            Memento memento = new Memento("MSE", 2);
            target.setMemento(memento);
            Assert.AreEqual(target.propertiesView.getRb(), 2, "propertiesView Radio Button could not be set.");
        }

        /// <summary>
        ///Test "setMemento": Object is not type int
        ///</summary>
        [TestMethod()]
        public void setMementoTest_notInt()
        {
            MSE target = new MSE();
            List<Memento> memList = new List<Memento>();
            Memento memento = new Memento("MEMLIST", memList);
            target.setMemento(memento);
        }

        /// <summary>
        ///Test "setMemento": Object has to be in range [0, 3]
        ///</summary>
        [TestMethod()]
        public void setMementoTest_range()
        {
            MSE_Accessor target = new MSE_Accessor();
            Memento memento = new Memento("RANGE", -1);
            target.setMemento(memento);
            Assert.IsTrue(target.propertiesView.getRb() > -1, "Invailed Range for MSE. Range [0, 3]");

            memento = new Memento("RANGE", 4);
            target.setMemento(memento);
            Assert.IsTrue(target.propertiesView.getRb() < 4, "Invailed Range for MSE. Range [0, 3]");
        }

        /// <summary>
        ///Test "namePlugin"
        ///</summary>
        [TestMethod()]
        public void namePluginTest()
        {
            MSE target = new MSE();
            string expected = "PM_MSE";
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
            MSE target = new MSE();
            UserControl actual;
            actual = target.propertyView;
        }

        /// <summary>
        ///Test "type"
        ///</summary>
        [TestMethod()]
        public void typeTest()
        {
            MSE target = new MSE();
            PluginType expected = PluginType.IMetricOqat;
            PluginType actual;
            target.type = expected;
            actual = target.type;
            Assert.AreEqual(expected, actual);
        }
    }
}
