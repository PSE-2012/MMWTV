using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using PS_YuvVideoHandler;
using Oqat.PublicRessources.Plugin;
using Oqat.PublicRessources.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace OQAT_Tests
{
    ///<summary>
    ///Class for testing YUV VideoHandler
    ///</summary>
    [TestClass]
    public class VideoHandlerTest
    {
        private static string readPath;
        private static string writePath;
        private static string sampleVideosPath;
        private static string[] sampleVideos;
        private static string plPathSolution;

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {

            plPathSolution = testContext.TestRunDirectory + "\\..\\..\\Oqat\\bin\\Debug\\Plugins";
            sampleVideosPath = testContext.TestDir + "\\..\\..\\Oqat_Tests\\TestData\\sampleVideos";
            string[] plugins = Directory.GetFiles(plPathSolution, "*.dll");


            sampleVideos = Directory.GetFiles(sampleVideosPath, "*.yuv");
            readPath = sampleVideos[0];
            // Warning: video settings might have to be set manually in test methods!
            writePath = Path.GetDirectoryName(readPath) + Path.GetFileNameWithoutExtension(readPath) + "_Copy.yuv";

            // we are not testing 
            if (!Directory.Exists(testContext.TestRunDirectory + "\\Out\\Plugins"))
                Directory.CreateDirectory(testContext.TestRunDirectory + "\\Out\\Plugins");

            foreach (string s in plugins)
            {
                string targetpath = testContext.TestRunDirectory + "\\Out\\Plugins\\" + Path.GetFileName(s);
                if(!File.Exists(targetpath))
                    File.Copy(s, targetpath);
            }
        }

        ///<summary>
        ///Tests setting of the read context, as well as some basic properties
        ///</summary>
        [TestMethod]
        public void readContextTest()
        {
            YuvVideoHandler yvh = new YuvVideoHandler();
            Assert.AreEqual("yuvVideoHandler", yvh.namePlugin);
            Assert.AreEqual(PluginType.IVideoHandler, yvh.type);
            YuvVideoInfo info = new YuvVideoInfo(readPath);
            info.width = 352;
            info.height = 240;
            info.yuvFormat = YuvFormat.YUV420_IYUV;
            yvh.setReadContext(readPath, info);
            Assert.AreEqual(info, yvh.readVidInfo);
            Assert.IsNotNull(yvh.frameByteSize);
            //Assert.IsTrue(yvh.consistent);
            string falsePath = "\\bla_cif.yuv";
            YuvVideoInfo falseInfo = new YuvVideoInfo();
            falseInfo.path = falsePath;
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
                yvh.setReadContext(readPath, falseInfo);
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
            readPath = sampleVideos[2];
            YuvVideoInfo info = new YuvVideoInfo(readPath);
            yvh.setImportContext(readPath);
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
            YuvVideoInfo info = new YuvVideoInfo(readPath);
            info.width = 352;
            info.height = 240;
            info.yuvFormat = YuvFormat.YUV420_IYUV;
            try
            {
                yvh.setWriteContext(readPath, info);
                Assert.Fail("no exception thrown");
            }
            catch (Exception ex)
            {

            }
            yvh.setReadContext(readPath, info); // write context cannot be set without a valid read context
            yvh.setWriteContext(readPath, info);
            Assert.AreEqual(info, yvh.writeVidInfo);
            Assert.IsTrue(yvh.consistent);
            string falsePath = "\\bla_cif.yuv";
            YuvVideoInfo falseInfo = new YuvVideoInfo();
            falseInfo.path = falsePath;
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
                yvh.setWriteContext(readPath, falseInfo);
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
            readPath = sampleVideos[2];
            YuvVideoHandler yvh = new YuvVideoHandler();
            yvh.setImportContext(readPath);
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
            YuvVideoInfo info = new YuvVideoInfo(readPath);
            info.width = 352;
            info.height = 240;
            info.yuvFormat = YuvFormat.YUV420_IYUV;
            yvh.setReadContext(readPath, info); // write context cannot be set without a valid read context
            writePath = sampleVideosPath + "\\americanFootball_352x240_125_Copy.yuv";
            YuvVideoInfo writeinfo = new YuvVideoInfo();
            writeinfo.path = writePath;
            writeinfo.width = 352;
            writeinfo.height = 240;
            writeinfo.yuvFormat = YuvFormat.YUV420_IYUV;
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
            YuvVideoHandler_Accessor yvh = new YuvVideoHandler_Accessor();
            YuvVideoInfo info = new YuvVideoInfo(readPath);
            info.width = 352;
            info.height = 240;
            info.yuvFormat = YuvFormat.YUV420_IYUV;
            yvh.setReadContext(readPath, info);
            // yvh.setImportContext(readPath);
            // dangerous to use setimportcontext here, as it initializes a
            // yuv video info with the given path, without setting height/width
            int i = 0;
            System.Drawing.Bitmap testframe;
            while (yvh.positionReader < yvh.readVidInfo.frameCount)
            {
                testframe = yvh.getFrame();
                if (yvh.positionReader != i + 1)
                {
                    Assert.Fail("Reader possition is " + yvh.positionReader
                 + " while supposed to be " + i + 1);
                }
                if (testframe == null)
                {
                    Assert.Fail("Frame number " + i + " is null" + 
                        " queuecount" + yvh.readQueue.Count);
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
            try
            {
                yvh.positionReader = 500;
                testframe = yvh.getFrame();
                Assert.Fail("no exception thrown");
            }
            catch (Exception ex)
            {

            }
        }

        ///<summary>
        ///Tests writing of frames
        ///</summary>
        [TestMethod]
        public void writeFramesTest()
        {
            YuvVideoHandler_Accessor yvh = new YuvVideoHandler_Accessor();
            writePath = sampleVideosPath + "\\americanFootball_352x240_125_Copy.yuv";
            YuvVideoInfo info = new YuvVideoInfo();
            info.path = writePath;
            info.width = 352;
            info.height = 240;
            info.yuvFormat = YuvFormat.YUV420_IYUV;
            yvh.setReadContext(readPath, info); // write context cannot be set without a valid read context
            YuvVideoInfo writeinfo = new YuvVideoInfo();
            writeinfo.path = sampleVideosPath;
            writeinfo.width = 352;
            writeinfo.height = 240;
            writeinfo.yuvFormat = YuvFormat.YUV420_IYUV;
            yvh.setWriteContext(writePath, writeinfo);
            System.Drawing.Bitmap testframe = yvh.getFrame();
            System.Drawing.Bitmap[] frames = new System.Drawing.Bitmap[20];
            for (int i = 0; i < frames.Length; i++)
            {
                frames[i] = testframe;
            }
            yvh.writeFrames(4, frames);
            yvh.writeFrames(1000, frames); // is > totalFrames
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
            try
            {
                File.Delete(writePath);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
