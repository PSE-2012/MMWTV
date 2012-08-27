using Oqat.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Oqat.PublicRessources.Model;
using System.IO;

namespace OQAT_Tests
{
    
    
    /// <summary>
    ///Dies ist eine Testklasse für "CaretakerTest" und soll
    ///alle CaretakerTest Komponententests enthalten.
    ///</summary>
    [TestClass()]
    public class CaretakerTest
    {
        private static string testfolder = "C:\\testfolder\\";//System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(typeof(CaretakerTest)).Location);

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
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            
        }

        //Mit ClassCleanup führen Sie Code aus, nachdem alle Tests in einer Klasse ausgeführt wurden.
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            
        }
        
        //Mit TestInitialize können Sie vor jedem einzelnen Test Code ausführen.
        [TestInitialize()]
        public void MyTestInitialize()
        {
            if (!Directory.Exists(testfolder))
            {
                Directory.CreateDirectory(testfolder);
            }
        }
        
        //Mit TestCleanup können Sie nach jedem einzelnen Test Code ausführen.
        [TestCleanup()]
        public void MyTestCleanup()
        {
            if (Directory.Exists(testfolder))
            {
                string fileName = testfolder + "TestMementoFile.mem";
                if (File.Exists(fileName))
                {
                    FileInfo fInfo = new FileInfo(fileName);
                    fInfo.IsReadOnly = false;
                }
                Directory.Delete(testfolder, true);
            }
        }

        #endregion


        /// <summary>
        ///Test "caretaker" singleton getter
        ///</summary>
        [TestMethod()]
        [DeploymentItem("OQAT.exe")]
        public void caretakerTest()
        {
            Caretaker actual = null;
            actual = Caretaker_Accessor.caretaker;
            Assert.IsTrue(actual is Caretaker, "The returned object is not a valid Caretaker instance.");

            Caretaker actual2 = null;
            actual2 = Caretaker_Accessor.caretaker;
            Assert.AreEqual<Caretaker>(actual, actual2, "Second call did not return reference to the same object.");
        }


        #region getMementoTests

        /// <summary>
        ///Test "getMemento": path null
        ///</summary>
        [TestMethod()]
        public void getMementoTest_null()
        {
            Caretaker target = Caretaker_Accessor.caretaker;

            Memento expected = null;
            string fileName = null;
            Memento actual;
            actual = target.getMemento(fileName);

            Assert.AreEqual<Memento>(expected, actual);
        }

        /// <summary>
        ///Test "getMemento": path empty
        ///</summary>
        [TestMethod()]
        public void getMementoTest_empty()
        {
            Caretaker target = Caretaker_Accessor.caretaker;

            string fileName = "";
            Memento expected = null;
            Memento actual;
            actual = target.getMemento(fileName);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Test "getMemento": targetfile not valid memento
        ///</summary>
        [TestMethod()]
        public void getMementoTest_notMemento()
        {
            Caretaker target = Caretaker_Accessor.caretaker;

            string fileName = testfolder + "noMemento.mem";
            Memento expected = null;
            Memento actual;

            FileStream fs = File.Create(fileName);
            fs.WriteByte(1);
            fs.WriteByte(1);
            fs.WriteByte(1);
            fs.Close();

            actual = target.getMemento(fileName);

            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        ///Test "getMemento": targetfile valid memento but no regular fileextension (.mem)
        ///</summary>
        [TestMethod()]
        public void getMementoTest_MementoWrongExtension()
        {
            Caretaker target = Caretaker_Accessor.caretaker;

            string fileName = testfolder+"PF_Convolution.nomem";
            Memento expected = new Memento("TestMemento", 42);
            expected.mementoPath = fileName;

            target.writeMemento(expected);

            Memento actual;
            actual = target.getMemento(fileName);

            Assert.IsNotNull(actual, "The memento could not be read.");
            Assert.AreEqual(expected.name, actual.name);
            Assert.AreEqual(expected.state, actual.state);
            Assert.AreEqual(expected.mementoPath, actual.mementoPath);
        }

        /// <summary>
        ///Test "writeMemento" / "getMemento": valid write and read
        ///</summary>
        [TestMethod()]
        public void writeReadMementoTest()
        {
            Caretaker target = Caretaker_Accessor.caretaker;

            string fileName = testfolder + "TestMementoFile.mem";
            Memento expected = new Memento("TestMemento", 42);
            expected.mementoPath = fileName;

            target.writeMemento(expected);

            Memento actual;
            actual = target.getMemento(fileName);

            Assert.IsNotNull(actual, "The memento could not be read.");
            Assert.AreEqual(expected.name, actual.name);
            Assert.AreEqual(expected.state, actual.state);
            Assert.AreEqual(expected.mementoPath, actual.mementoPath);
        }

        #endregion


        #region writeMementoTests

        /// <summary>
        ///Test "writeMemento": file already exists
        ///</summary>
        [TestMethod()]
        public void writeMementoTest_fileExists()
        {
            Caretaker target = Caretaker_Accessor.caretaker;

            string fileName = testfolder + "TestMementoFile.mem";
            Memento expected = new Memento("TestMemento", 42);
            expected.mementoPath = fileName;

            FileStream fs = File.Create(fileName);
            fs.Close();

            //expecting to overwrite file
            target.writeMemento(expected);

            Memento actual;
            actual = target.getMemento(fileName);

            Assert.IsNotNull(actual, "The memento could not be read.");
            Assert.AreEqual(expected.name, actual.name);
            Assert.AreEqual(expected.state, actual.state);
            Assert.AreEqual(expected.mementoPath, actual.mementoPath);
        }

        /// <summary>
        ///Test "writeMemento": folder does not exist
        ///</summary>
        [TestMethod()]
        public void writeMementoTest_noFolder()
        {
            Caretaker target = Caretaker_Accessor.caretaker;

            string fileName = testfolder + "allNewOqatTestingFolder\\" + "TestMementoFile.mem";
            Memento expected = new Memento("TestMemento", 42);
            expected.mementoPath = fileName;

            target.writeMemento(expected);

            Assert.IsFalse(File.Exists(fileName), "New folder was created for memento. This might be unwanted behavior or a security risk.");
            //exception is thrown, caught and passed to OQAT ErrorConsole
        }

        /// <summary>
        ///Test "writeMemento": file already exists and is readonly
        ///</summary>
        [TestMethod()]
        public void writeMementoTest_fileReadonly()
        {
            Caretaker target = Caretaker_Accessor.caretaker;

            string fileName = testfolder + "TestMementoFile.mem";
            Memento expected = new Memento("TestMemento", 42);
            expected.mementoPath = fileName;

            FileStream fs = File.Create(fileName);
            fs.WriteByte(1);
            fs.Close();
            FileInfo fInfo = new FileInfo(fileName);
            fInfo.IsReadOnly = true;

            target.writeMemento(expected);

            fInfo.IsReadOnly = false;
            FileStream fsr = File.Open(fileName, FileMode.Open);
            Assert.IsTrue((fsr.Length == 1 && fsr.ReadByte() == 1), "Readonly file was changed.");
            fsr.Close();
            //Exception is thrown, caught and passed to OqatErrorConsole
        }

        #endregion


    }
}
