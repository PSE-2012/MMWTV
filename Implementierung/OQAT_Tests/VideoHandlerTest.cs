using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using PS_YuvVideoHandler;
using Oqat.PublicRessources.Plugin;
using Oqat.PublicRessources.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OQAT_Tests
{
    [TestClass]
    public class VideoHandlerTest
    {
        /// <summary>
        ///Tests setting of the read context, as well as some basic properties
        ///</summary>
        [TestMethod]
        public void readContextTest()
        {
            YuvVideoHandler yvh = new YuvVideoHandler();
            Assert.AreEqual("yuvVideoHandler", yvh.namePlugin);
            Assert.AreEqual(PluginType.IVideoHandler, yvh.type);
            string path =
                "D:\\Documents and Settings\\fenix1\\OQAT\\Implementierung\\OQAT_Tests\\TestData\\sampleVideos\\bus_cif.yuv";
            YuvVideoInfo info = new YuvVideoInfo(path);
            yvh.setReadContext(path, info);
            Assert.AreEqual(info, yvh.readVidInfo);
            Assert.IsNotNull(yvh.frameByteSize);
            Assert.IsTrue(yvh.consistent);
        }

        /// <summary>
        ///Tests setting of the read context through the setImportContext
        ///method that only requires a file path.
        ///</summary>
        [TestMethod]
        public void importContextTest()
        {
            YuvVideoHandler yvh = new YuvVideoHandler();
            string path =
                "D:\\Documents and Settings\\fenix1\\OQAT\\Implementierung\\OQAT_Tests\\TestData\\sampleVideos\\bus_cif.yuv";
            YuvVideoInfo info = new YuvVideoInfo(path);
            yvh.setImportContext(path);
            Assert.AreEqual(info, yvh.readVidInfo);
            Assert.IsNotNull(yvh.frameByteSize);
            Assert.IsTrue(yvh.consistent);
        }

        [TestMethod]
        public void mementoTest()
        {
            YuvVideoHandler yvh = new YuvVideoHandler();
            Memento mem = yvh.getMemento();
            yvh.setMemento(mem); // nothing is supposed to happen after calling this
            Assert.IsNull(mem.state);
            Assert.AreEqual("MementoStub", mem.name);
        }

        [TestMethod]
        public void writeContextTest()
        {
            YuvVideoHandler yvh = new YuvVideoHandler();
            string path =
                "D:\\Documents and Settings\\fenix1\\OQAT\\Implementierung\\OQAT_Tests\\TestData\\sampleVideos\\bus_cif.yuv";
            YuvVideoInfo info = new YuvVideoInfo(path);
            yvh.setReadContext(path, info); // write context cannot be set without a valid read context
            yvh.setWriteContext(path, info);
            Assert.AreEqual(info, yvh.writeVidInfo);
            Assert.IsTrue(yvh.consistent);
        }

        [TestMethod]
        public void flushReaderTest()
        {
            YuvVideoHandler yvh = new YuvVideoHandler();
            string path =
                "D:\\Documents and Settings\\fenix1\\OQAT\\Implementierung\\OQAT_Tests\\TestData\\sampleVideos\\bus_cif.yuv";
            YuvVideoInfo info = new YuvVideoInfo(path);
            yvh.setImportContext(path);
            int oldFrameByteSize = yvh.frameByteSize;
            yvh.flushReader();
            Assert.AreEqual(oldFrameByteSize, yvh.frameByteSize);
            Assert.AreEqual(0, yvh.positionReader);
        }

        [TestMethod]
        public void getFrameTest()
        {
            YuvVideoHandler yvh = new YuvVideoHandler();
            string path =
                "D:\\Documents and Settings\\fenix1\\OQAT\\Implementierung\\OQAT_Tests\\TestData\\sampleVideos\\bus_cif.yuv";
            YuvVideoInfo info = new YuvVideoInfo(path);
            yvh.setImportContext(path);
            int i = 0;
            System.Drawing.Bitmap testframe;
            yvh.positionReader = 0;
            while (i < yvh.readVidInfo.frameCount)
            {
                testframe = yvh.getFrame();
                if (yvh.positionReader != i + 1)
                {
                    Assert.Fail("Reader possition is " + yvh.positionReader
                 + " while supposed to be " + i + 1);
                }
                if (testframe == null)
                {
                    Assert.Fail("Frame number " + i + " is null");
                }
                if (testframe.Width != yvh.readVidInfo.width
                    || testframe.Height != yvh.readVidInfo.height)
                {
                    int[] param = new int[3] {i, testframe.Width,
                        testframe.Height};
                    Assert.Fail("Wrong frame size for frame "
                        + param[0] + ":" + param[1]
                        + "," + param[2]);
                }
                i++;
            }
        }
    }
}
