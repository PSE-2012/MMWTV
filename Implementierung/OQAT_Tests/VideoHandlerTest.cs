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
        ///<summary>
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
            string falsePath = "\\bla_cif.yuv";
            YuvVideoInfo falseInfo = new YuvVideoInfo(falsePath);
            try
            {
                yvh.setReadContext(falsePath, falseInfo);
                Assert.Fail("no exception thrown");
            }
            catch (Exception ex)
            {

            }
            try
            {
                yvh.setReadContext(falsePath, info);
                Assert.Fail("no exception thrown");
            }
            catch (Exception ex)
            {

            }
            try
            {
                yvh.setReadContext(path, falseInfo);
                Assert.Fail("no exception thrown");
            }
            catch (Exception ex)
            {

            }
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
            string falsePath = "\\bla_cif.yuv";
            try
            {
                yvh.setImportContext(falsePath);
                Assert.Fail("no exception thrown");
            }
            catch (Exception ex)
            {

            }
        }

        ///<summary>
        ///Get/setmemento test
        ///</summary>
        [TestMethod]
        public void mementoTest()
        {
            YuvVideoHandler yvh = new YuvVideoHandler();
            Memento mem = yvh.getMemento();
            yvh.setMemento(mem); // nothing is supposed to happen after calling this
            Assert.IsNull(mem.state);
            Assert.AreEqual("MementoStub", mem.name);
        }

        ///<summary>
        ///Write context test
        ///</summary>
        [TestMethod]
        public void writeContextTest()
        {
            YuvVideoHandler yvh = new YuvVideoHandler();
            string path =
                "D:\\Documents and Settings\\fenix1\\OQAT\\Implementierung\\OQAT_Tests\\TestData\\sampleVideos\\bus_cif.yuv";
            YuvVideoInfo info = new YuvVideoInfo(path);
            try
            {
                yvh.setWriteContext(path, info);
                Assert.Fail("no exception thrown");
            }
            catch (Exception ex)
            {

            }
            yvh.setReadContext(path, info); // write context cannot be set without a valid read context
            yvh.setWriteContext(path, info);
            Assert.AreEqual(info, yvh.writeVidInfo);
            Assert.IsTrue(yvh.consistent);
            string falsePath = "\\bla_cif.yuv";
            YuvVideoInfo falseInfo = new YuvVideoInfo(falsePath);
            try
            {
                yvh.setWriteContext(falsePath, falseInfo);
                Assert.Fail("no exception thrown");
            }
            catch (Exception ex)
            {

            }
            try
            {
                yvh.setWriteContext(falsePath, info);
                Assert.Fail("no exception thrown");
            }
            catch (Exception ex)
            {

            }
            try
            {
                yvh.setWriteContext(path, falseInfo);
                Assert.Fail("no exception thrown");
            }
            catch (Exception ex)
            {

            }
        }

        ///<summary>
        ///Tests flushing of current reader context
        ///</summary>
        [TestMethod]
        public void flushReaderTest()
        {
            YuvVideoHandler yvh = new YuvVideoHandler();
            string path =
                "D:\\Documents and Settings\\fenix1\\OQAT\\Implementierung\\OQAT_Tests\\TestData\\sampleVideos\\bus_cif.yuv";
            yvh.setImportContext(path);
            int oldFrameByteSize = yvh.frameByteSize;
            yvh.flushReader();
            Assert.AreEqual(oldFrameByteSize, yvh.frameByteSize);
            Assert.AreEqual(0, yvh.positionReader);
        }

        ///<summary>
        ///Tests flushing of current write context
        ///</summary>
        [TestMethod]
        public void flushWriterTest()
        {
            YuvVideoHandler yvh = new YuvVideoHandler();
            string path =
                "D:\\Documents and Settings\\fenix1\\OQAT\\Implementierung\\OQAT_Tests\\TestData\\sampleVideos\\bus_cif.yuv";
            YuvVideoInfo info = new YuvVideoInfo(path);
            yvh.setReadContext(path, info); // write context cannot be set without a valid read context
            string writePath =
                "D:\\Documents and Settings\\fenix1\\OQAT\\Implementierung\\OQAT_Tests\\TestData\\sampleVideos\\bus_cif_copy.yuv";
            YuvVideoInfo writeinfo = new YuvVideoInfo(writePath);
            yvh.setWriteContext(writePath, writeinfo);
            int oldFrameByteSize = yvh.frameByteSize;
            yvh.flushWriter();
            Assert.AreEqual(oldFrameByteSize, yvh.frameByteSize);
            Assert.AreEqual(0, yvh.positionReader);
            System.Drawing.Bitmap testframe = yvh.getFrame();
            System.Drawing.Bitmap[] frames = new System.Drawing.Bitmap[1];
            frames[0] = testframe;
            yvh.writeFrames(4, frames);
            yvh.flushWriter();
            yvh.writeFrames(4, frames);
        }

        ///<summary>
        ///Tests reading of frames
        ///</summary>
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

        ///<summary>
        ///Tests writing of frames
        ///</summary>
        [TestMethod]
        public void writeFramesTest()
        {
            YuvVideoHandler yvh = new YuvVideoHandler();
            string readPath =
                "D:\\Documents and Settings\\fenix1\\OQAT\\Implementierung\\OQAT_Tests\\TestData\\sampleVideos\\bus_cif.yuv";
            YuvVideoInfo info = new YuvVideoInfo(readPath);
            yvh.setReadContext(readPath, info); // write context cannot be set without a valid read context
            string writePath =
                "D:\\Documents and Settings\\fenix1\\OQAT\\Implementierung\\OQAT_Tests\\TestData\\sampleVideos\\bus_cif_copy.yuv";
            YuvVideoInfo writeinfo = new YuvVideoInfo(writePath);
            yvh.setWriteContext(writePath, writeinfo);
            System.Drawing.Bitmap testframe = yvh.getFrame();
            System.Drawing.Bitmap[] frames = new System.Drawing.Bitmap[20];
            for (int i = 0; i < frames.Length; i++)
            {
                frames[i] = testframe;
            }
            yvh.writeFrames(4, frames);
            yvh.writeFrames(151, frames); // is > totalFrames for bus_cif.yuv
            try
            {
                yvh.writeFrames(-1, frames);
                Assert.Fail("no exception thrown");
            }
            catch (Exception ex)
            {

            }
            try
            {
            yvh.writeFrames(3, new System.Drawing.Bitmap[3]);
            Assert.Fail("no exception thrown");
            }
            catch (Exception ex)
            {

            }
        }
    }
}
