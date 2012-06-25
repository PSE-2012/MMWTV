using YuvVideoHandler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace OQAT_Tests
{
    
    
    /// <summary>
    ///Dies ist eine Testklasse für "VM_Properties_YuvVideoHandlerTest" und soll
    ///alle VM_Properties_YuvVideoHandlerTest Komponententests enthalten.
    ///</summary>
    [TestClass()]
    public class VM_Properties_YuvVideoHandlerTest
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
        ///Ein Test für "VM_Properties_YuvVideoHandler-Konstruktor"
        ///</summary>
        [TestMethod()]
        public void VM_Properties_YuvVideoHandlerConstructorTest()
        {
            VM_Properties_YuvVideoHandler target = new VM_Properties_YuvVideoHandler();
            Assert.Inconclusive("TODO: Code zum Überprüfen des Ziels implementieren");
        }
    }
}
