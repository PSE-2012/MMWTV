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
    /// Not working yet
    /// </summary>
    [TestClass]
    public class VidImportDialogTest
    {
        [TestMethod]
        public void constructorTest()
        {
            StringCollection sc = new StringCollection();
            sc.Add("D:\\Documents and Settings\\fenix1\\OQAT\\Implementierung\\OQAT_Tests\\TestData\\sampleVideos\\bus_cif.yuv");
            sc.Add("D:\\Documents and Settings\\fenix1\\OQAT\\Implementierung\\OQAT_Tests\\TestData\\sampleVideos\\container_cif.yuv");
            VM_VidImportOptionsDialog optdial = new VM_VidImportOptionsDialog(sc);
            Assert.AreEqual(2, optdial.videoList.Count);
        }
    }
}
