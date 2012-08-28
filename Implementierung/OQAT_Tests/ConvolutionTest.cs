using PF_Convolution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Oqat.PublicRessources.Model;
using System.Drawing;
using System.Windows.Controls;
using Oqat.PublicRessources.Plugin;
using System.Collections.Generic;

namespace OQAT_Tests
{
    
    
    /// <summary>
    ///Dies ist eine Testklasse für "ConvolutionTest" und soll
    ///alle ConvolutionTest Komponententests enthalten.
    ///</summary>
    [TestClass()]
    public class ConvolutionTest
    {
        private static Bitmap testBitmap;
        private static Bitmap processedBitmap;
        private static Memento testMemento;
        private static int[,] matrix;
        private static Memento blur;
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
            Convolution conv = new Convolution();
            testMemento = new Memento("PF_Convolution", new int[3, 3]);
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

            //create blur matrix
            int square = 3;
            matrix = new int[square, square];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] = 1;
                }
            }
            blur = new Memento("Blur", matrix);
            //get blured Bitmap
            Memento original = conv.getMemento();
            conv.setMemento(blur);
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
        ///Test "Convolution-Constructor"
        ///</summary>
        [TestMethod()]
        public void ConvolutionConstructorTest()
        {
            Convolution target = new Convolution();
            Assert.IsTrue(target is Convolution, "The returned object is not a valid Convolution instance.");
        }

        /// <summary>
        ///Test "NotifyPropertyChanged"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PF_Convolution.dll")]
        public void NotifyPropertyChangedTest()
        {
            Convolution_Accessor target = new Convolution_Accessor();
            string info = string.Empty; // TODO: Passenden Wert initialisieren
            target.NotifyPropertyChanged(info);
            Assert.Inconclusive("Eine private Methode, soll diese getestet werden?");
        }

        /// <summary>
        ///Test "getMemento": start Mememento is set.
        ///</summary>
        [TestMethod()]
        public void getMementoTest_start()
        {
            Convolution target = new Convolution();
            Memento expected = testMemento;
            Memento actual;
            actual = target.getMemento();
            int[,] matrixExpected = (int[,])expected.state;
            int[,] matrixActual = (int[,])actual.state;
            for (int i = 0; i < matrixExpected.GetLength(0); i++)
            {
                for (int j = 0; j < matrixExpected.GetLength(1); j++)
                {
                    Assert.AreEqual(matrixExpected[i, j], matrixActual[i, j], "Memento object is not set to int[3, 3].");
                }
            }
            Assert.AreEqual(expected.name, actual.name, "Memento name is not PF_Convolution.");
        }

        /// <summary>
        ///Test "process"
        ///</summary>
        [TestMethod()]
        public void processTest()
        {
            Convolution target = new Convolution();
            Bitmap frame = testBitmap;
            Bitmap expected = processedBitmap;
            Bitmap actual;
            target.setMemento(blur);
            actual = target.process(frame);
            for (int height = 0; height < expected.Height; height++)
            {
                for (int width = 0; width < expected.Width; width++)
                {
                    Assert.AreEqual(expected.GetPixel(height, width), actual.GetPixel(height, width), "Process is working randomly");
                }
            }
        }

        /// <summary>
        ///Test "setMemento": Matrix is square and has vailed size.
        ///</summary>
        [TestMethod()]
        public void setMementoTest()
        {
            Convolution target = new Convolution();
            int[,] expectedMatrix = new int[5, 5];
            Memento memento = new Memento("Conv", new int[5, 5]);
            target.setMemento(memento);
            Assert.IsTrue(target.matrix is int[,], "Memento.state is not a Matrix.");
            int[,] actualMatrix = (int[,])target.getMemento().state;
            for (int dim1 = 0; dim1 < expectedMatrix.GetLength(0); dim1++)
            {
                for (int dim2 = 0; dim2 < expectedMatrix.GetLength(1); dim2++)
                {
                    Assert.AreEqual(expectedMatrix[dim1, dim2], actualMatrix[dim1, dim2], "Matrix is not expected matrix.");
                }
            }

        }

        /// <summary>
        ///Test "setMemento": state of memento is not Matrix
        ///</summary>
        [TestMethod()]
        public void setMementoTest_notConvMatrix()
        {
            Convolution target = new Convolution();
            List<Memento> memList = new List<Memento>();
            Memento memento = new Memento("Conv", memList);
            target.setMemento(memento);
            Assert.IsTrue(target.matrix is int[,], "Memento.state should be a Matrix.");
        }

        /// <summary>
        ///Test"setMemento": Matrix is not a square
        ///</summary>
        [TestMethod()]
        public void setMementoTest_notSquareMatrix()
        {
            Convolution target = new Convolution();
            Memento memento = new Memento("Conv", new int[3, 4]);
            target.setMemento(memento);
            Assert.IsTrue(target.matrix.GetLength(0) == target.matrix.GetLength(1), "Matrix is not square.");
        }

        /// <summary>
        ///Test "setMemento": Invalid kernel size.
        ///</summary>
        [TestMethod()]
        public void setMementoTest_MatrixSize()
        {
            Convolution target = new Convolution();
            //Matrix size to small
            Memento memento = new Memento("Conv", new int[2, 2]);
            target.setMemento(memento);
            Assert.IsTrue((target.matrix.GetLength(0) > 2) && (target.matrix.GetLength(1) > 2), "Invalid kernel size. Matrix has to be 2 < squarelength < 26.");

            //Matrix size to big
            memento = new Memento("Conv", new int[26, 26]);
            target.setMemento(memento);
            Assert.IsTrue((target.matrix.GetLength(0) < 26) && (target.matrix.GetLength(1) < 26), "Invalid kernel size. Matrix has to be 2 < squarelength < 26.");
        }

        /// <summary>
        ///              0 0 0
        ///Test "matrix" 0 0 0 is set as matrix at start.
        ///              0 0 0
        ///</summary>
        [TestMethod()]
        public void matrixTest()
        {
            Convolution target = new Convolution();
            int[,] expected = new int[3, 3];
            int[,] actual;
            actual = target.matrix;
            //Check ever matrix entry
            for (int dim1 = 0; dim1 < expected.GetLength(0); dim1++)
            {
                for (int dim2 = 0; dim2 < expected.GetLength(1); dim2++)
                {
                    Assert.AreEqual(expected[dim1, dim2], actual[dim1, dim2], "Matrix " + expected.ToString() + " is not expected matrix " + target.matrix.ToString() + ". ");
                }
            }
        }

        /// <summary>
        ///Test "namePlugin"
        ///</summary>
        [TestMethod()]
        public void namePluginTest()
        {
            Convolution target = new Convolution();
            string expected = "PF_Convolution";
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
            Convolution target = new Convolution();
            UserControl actual;
            actual = target.propertyView;
            Assert.IsNotNull(actual, "propertyView is not set.");
        }

        /// <summary>
        ///Test "type"
        ///</summary>
        [TestMethod()]
        public void typeTest()
        {
            Convolution target = new Convolution();
            PluginType expected = PluginType.IFilterOqat;
            PluginType actual;
            target.type = expected;
            actual = target.type;
            Assert.AreEqual(expected, actual);
        }
    }
}
