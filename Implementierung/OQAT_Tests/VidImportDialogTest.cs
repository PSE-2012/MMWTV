using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Oqat.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OQAT_Tests
{
    /// <summary>
    /// Test class for VM_VidImportOptionsDialog
    /// </summary>
    [TestClass]
    public class VidImportDialogTest
    {
        private static string path1 =
            "D:\\Documents and Settings\\fenix1\\OQAT\\Implementierung\\OQAT_Tests\\TestData\\sampleVideos\\bus_cif.yuv";
        private static string path2 =
            "D:\\Documents and Settings\\fenix1\\OQAT\\Implementierung\\OQAT_Tests\\TestData\\sampleVideos\\container_cif.yuv";
        /// <summary>
        /// Not working yet
        /// </summary>
        [TestMethod]
        public void constructorTest()
        {
            StringCollection sc = new StringCollection();
            sc.Add(path1);
            sc.Add(path2);
            VM_VidImportOptionsDialog optdial = new VM_VidImportOptionsDialog(sc);
            Assert.AreEqual(2, optdial.videoList.Count);
        }
    }
}
