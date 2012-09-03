using PP_Player;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;
using Oqat.PublicRessources.Plugin;
using Oqat.PublicRessources.Model;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Threading;
using Oqat.PublicRessources.Plugin;
using System.Windows.Controls;
using PS_YuvVideoHandler;
using Oqat.Model;
using System.IO;
using System.Drawing;
using Oqat.ViewModel;

namespace OQAT_Tests
{


    /// <summary>
    ///Dies ist eine Testklasse für "PlayerTest" und soll
    ///alle PlayerTest Komponententests enthalten.
    ///</summary>
    [TestClass()]
    public class PlayerTest
    {
        private static string readPath;
        private static string writePath;
        private static string sampleVideosPath;
        private static string[] sampleVideos;
        private static string plPathSolution;

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
            plPathSolution = testContext.TestRunDirectory + "\\..\\..\\Oqat\\bin\\Debug\\Plugins";
            sampleVideosPath = testContext.TestDir + "\\..\\..\\Oqat_Tests\\TestData\\sampleVideos";
            string[] plugins = Directory.GetFiles(plPathSolution, "*.dll");


            sampleVideos = Directory.GetFiles(sampleVideosPath, "*.yuv");
            readPath = sampleVideos[0];
            writePath = Path.GetDirectoryName(readPath) + Path.GetFileNameWithoutExtension(readPath) + "_Copy.yuv";

            // we are not testing 
            if (!Directory.Exists(testContext.TestRunDirectory + "\\Out\\Plugins"))
                Directory.CreateDirectory(testContext.TestRunDirectory + "\\Out\\Plugins");

            foreach (string s in plugins)
            {
                string targetpath = testContext.TestRunDirectory + "\\Out\\Plugins\\" + Path.GetFileName(s);
                if (!File.Exists(targetpath))
                    File.Copy(s, targetpath);
            }
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
        ///Test "Player-Konstruktor"
        ///</summary>
        [TestMethod()]
        public void PlayerConstructorTest()
        {
            Player target = new Player();
            Assert.IsTrue(target is Player, "The returned object is not a valid Player instance. ");
        }

        /// <summary>
        ///Ein Test für "Clone"
        ///</summary>
        [TestMethod()]
        public void createExtraPluginInstanceTest()
        {
            Player target = new Player();
            IPlugin actual;
            actual = target.createExtraPluginInstance();
            Assert.IsTrue(actual is Player, "The returned object is not a valid Player instance. ");
        }

        /// <summary>
        ///Ein Test für "CopyMemory"
        ///</summary>
        [TestMethod()]
        public void CopyMemoryTest()
        {
            IntPtr Destination = new IntPtr();
            IntPtr Source = new IntPtr();
            int Length = 0;
            Player.CopyMemory(Destination, Source, Length);
        }

        /// <summary>
        ///Ein Test für "InitializeComponent"
        ///</summary>
        [TestMethod()]
        public void InitializeComponentTest()
        {
            Player target = new Player();
            target.InitializeComponent();
        }

        /// <summary>
        ///Test "OnPropertyChanged": random Jump Position Update
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PP_Player.dll")]
        public void OnPropertyChangedTest_randomJumpPositionUpdate()
        {
            Player_Accessor target = new Player_Accessor();
            string expected = "rjpu";
            object sender = new YuvVideoHandler();
            PropertyChangedEventArgs e = new PropertyChangedEventArgs(target.randomJumpPositionUpdate);
            target.OnPropertyChanged(sender, e);
            Assert.AreEqual(expected, e.PropertyName, "Property name is wrong. ");
            //TODO: Instrumentationsfehler: Binärdatei "...\OQAT\bin\Debug\Plugins\PP_Diagramm.dll" kann nicht gefunden werden.
        }

        /// <summary>
        ///Test "OnPropertyChanged": next Frame Position Update
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PP_Player.dll")]
        public void OnPropertyChangedTest_nextFramePositionUpdate()
        {
            Player_Accessor target = new Player_Accessor();
            string expected = "nfpu";
            object sender = new YuvVideoHandler();
            PropertyChangedEventArgs e = new PropertyChangedEventArgs(target.nextFramePositionUpdate);
            target.OnPropertyChanged(sender, e);
            Assert.AreEqual(expected, e.PropertyName, "Property name is wrong. ");
        }

        /// <summary>
        ///Test "OnPropertyChanged": pos Read Pro Name
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PP_Player.dll")]
        public void OnPropertyChangedTest_posReadProName()
        {
            Player_Accessor target = new Player_Accessor();
            string expected = "positionReader";
            object sender = new YuvVideoHandler();
            PropertyChangedEventArgs e = new PropertyChangedEventArgs(target.posReadProName);
            target.OnPropertyChanged(sender, e);
            Assert.AreEqual(expected, e.PropertyName, "Property name is wrong. ");
        }

        /// <summary>
        ///Test "OnPropertyChanged": fps Ind Pro Name
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PP_Player.dll")]
        public void OnPropertyChangedTest_fpsIndProName()
        {
            Player_Accessor target = new Player_Accessor();
            string expected = "fpsIndicatorValue";
            object sender = new YuvVideoHandler();
            PropertyChangedEventArgs e = new PropertyChangedEventArgs(target.fpsIndProName);
            target.OnPropertyChanged(sender, e);
            Assert.AreEqual(expected, e.PropertyName, "Property name is wrong. ");
        }

        /// <summary>
        ///Ein Test für "Pause_Click"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PP_Player.dll")]
        public void Pause_ClickTest()
        {
            Player_Accessor target = new Player_Accessor();
            object sender = new Button();
            RoutedEventArgs e = null;
            Assert.IsTrue(target._pausePlayTicker is ManualResetEvent);
            target._pausePlayTicker = null;
            Assert.AreEqual(null, target._pausePlayTicker);
            target.Pause_Click(sender, e);
            Assert.IsTrue(target._pausePlayTicker is ManualResetEvent);
        }

        /// <summary>
        ///Ein Test für "Play_Click"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PP_Player.dll")]
        public void Play_ClickTest()
        {
            Player_Accessor target = new Player_Accessor();
            object sender = new Button();
            RoutedEventArgs e = null;
            target._pausePlayTicker = null;
            Assert.AreEqual(null, target._pausePlayTicker);
            target.Play_Click(sender, e);
            Assert.IsTrue(target._pausePlayTicker is ManualResetEvent);
        }

        /// <summary>
        ///Ein Test für "System.Windows.Markup.IComponentConnector.Connect"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PP_Player.dll")]
        public void ConnectTest()
        {
            IComponentConnector target = new Player();
            int connectionId = 0;
            object target1 = null;
            target.Connect(connectionId, target1);
        }

        /// <summary>
        ///Ein Test für "flush"
        ///</summary>
        [TestMethod()]
        public void flushTest()
        {
            Player target = new Player();
            target.flush();
        }

        /// <summary>
        ///Ein Test für "getMemento"
        ///</summary>
        [TestMethod()]
        public void getMementoTest()
        {
            Player target = new Player();
            Memento expected = new Memento("dd", null, "dd");
            Memento actual;
            actual = target.getMemento();
            Assert.AreEqual(expected.name, actual.name);
            Assert.AreEqual(expected.state, actual.state);
            Assert.AreEqual(expected.mementoPath, actual.mementoPath);
        }

        /// <summary>
        ///Ein Test für "jumpToFrameTextBox_GotFocus"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PP_Player.dll")]
        public void jumpToFrameTextBox_GotFocusTest()
        {
            Player_Accessor target = new Player_Accessor();
            object sender = null;
            RoutedEventArgs e = null;
            target.jumpToFrameTextBox_GotFocus(sender, e);
        }

        /// <summary>
        ///Ein Test für "nextFrame_Click"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PP_Player.dll")]
        public void nextFrame_ClickTest()
        {
            Player_Accessor target = new Player_Accessor();
            object sender = null;
            RoutedEventArgs e = null;
            target.nextFrame_Click(sender, e);
        }

        /// <summary>
        ///Ein Test für "positionSlider_DragStarted"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PP_Player.dll")]
        public void positionSlider_DragStartedTest()
        {
            Player_Accessor target = new Player_Accessor();
            object sender = null;
            DragStartedEventArgs e = null;
            target.positionSlider_DragStarted(sender, e);
        }

        /// <summary>
        ///Ein Test für "setMemento"
        ///</summary>
        [TestMethod()]
        public void setMementoTest()
        {
            Player target = new Player();
            Memento memento = null;
            target.setMemento(memento);
            Memento expected = new Memento("dd", null, "dd");
            Memento actual;
            actual = target.getMemento();
            Assert.AreEqual(expected.name, actual.name);
            Assert.AreEqual(expected.state, actual.state);
            Assert.AreEqual(expected.mementoPath, actual.mementoPath);
        }

        /// <summary>
        ///Ein Test für "setVideo"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PP_Player.dll")]
        public void setVideoTest()
        {
            Player_Accessor target = new Player_Accessor();
            string vidPath = sampleVideos[2];
            object video = new Video(false, vidPath, new YuvVideoInfo(vidPath), null);
            target.setVideo(video);
            Assert.AreEqual(video, target.video);
        }

        /// <summary>
        ///Ein Test für "setVideo"
        ///</summary>
        [TestMethod()]
        public void setVideoTest1()
        {
            Player_Accessor target = new Player_Accessor();
            string vidPath = sampleVideos[2];
            IVideo video = new Video(false, vidPath, new YuvVideoInfo(vidPath), null);
            int position = 0;
            target.setVideo(video, position);
            Assert.AreEqual(video, target.video);
        }

        /// <summary>
        ///Ein Test für "slowDownButton_Click"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PP_Player.dll")]
        public void slowDownButton_ClickTest()
        {
            Player_Accessor target = new Player_Accessor();
            object sender = null;
            RoutedEventArgs e = null;
            int playTicker = target.playTickerTimeout;
            target.slowDownButton_Click(sender, e);
            Assert.AreEqual(playTicker + 10, target.playTickerTimeout);
        }

        /// <summary>
        ///Ein Test für "speedUpButton_Click"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PP_Player.dll")]
        public void speedUpButton_ClickTest()
        {
            Player_Accessor target = new Player_Accessor();
            object sender = null;
            RoutedEventArgs e = null;
            int playTicker = target.playTickerTimeout;
            target.speedUpButton_Click(sender, e);
            Assert.AreEqual(playTicker - 10, target.playTickerTimeout);
        }

        /// <summary>
        ///Ein Test für "fpsIndicatorValue"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PP_Player.dll")]
        public void fpsIndicatorValueTest()
        {
            Player_Accessor target = new Player_Accessor();
            int expected = 0;
            int actual;
            target.fpsIndicatorValue = expected;
            actual = target.fpsIndicatorValue;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Test "namePlugin"
        ///</summary>
        [TestMethod()]
        public void namePluginTest()
        {
            Player target = new Player();
            string actual;
            actual = target.namePlugin;
            Assert.AreEqual("PP_Player", actual, "The player is not PP_Player. ");
        }

        /// <summary>
        ///Ein Test für "pausePlayTicker"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PP_Player.dll")]
        public void pausePlayTickerTest()
        {
            Player_Accessor target = new Player_Accessor();
            ManualResetEvent actual;
            target._pausePlayTicker = null;
            actual = target.pausePlayTicker;
            Assert.IsTrue(actual is ManualResetEvent);
        }

        /// <summary>
        ///Test "playTickerTimeout"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PP_Player.dll")]
        public void playTickerTimeoutTest()
        {
            Player_Accessor target = new Player_Accessor();
            int expected = 0;
            int actual;
            target.playTickerTimeout = expected;
            actual = target.playTickerTimeout;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Test "positionReader"
        ///</summary>
        [TestMethod()]
        public void positionReaderTest()
        {
            Player target = new Player();
            int expected = 10;
            int actual;
            target.positionReader = expected;
            actual = target.positionReader;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Test "presentationType"
        ///</summary>
        [TestMethod()]
        public void presentationTypeTest()
        {
            Player target = new Player();
            PresentationPluginType expected = PresentationPluginType.Player;
            PresentationPluginType actual;
            actual = target.presentationType;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Test "propertyView"
        ///</summary>
        [TestMethod()]
        public void propertyViewTest()
        {
            Player target = new Player();
            UserControl actual;
            actual = target.propertyView;
        }

        /// <summary>
        ///Ein Test für "type"
        ///</summary>
        [TestMethod()]
        public void typeTest()
        {
            Player target = new Player();
            PluginType actual;
            actual = target.type;
            Assert.AreEqual(PluginType.IPresentation, actual, "Player is not of type IPresentation. ");
        }

        /// <summary>
        ///Ein Test für "getFrame"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PP_Player.dll")]
        public void getFrameTest()
        {
            Player_Accessor target = new Player_Accessor();
            string vidPath = sampleVideos[2];
            IVideo video = new Video(false, vidPath, new YuvVideoInfo(vidPath), null);
            int position = 0;
            target.setVideo(video, position);
            target.getFrame(position);
        }


        /// <summary>
        ///Ein Test für "jumpToFrame_Click"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PP_Player.dll")]
        public void jumpToFrame_ClickTest()
        {
            Player_Accessor target = new Player_Accessor();
            object sender = null;
            RoutedEventArgs e = null;
            string vidPath = sampleVideos[2];
            IVideo video = new Video(false, vidPath, new YuvVideoInfo(vidPath), null);
            int position = 0;
            target.setVideo(video, position);
            target.jumpToFrame_Click(sender, e);
        }

        /// <summary>
        ///Ein Test für "playTickerThread"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PP_Player.dll")]
        public void playTickerThreadTest()
        {
            Player_Accessor target = new Player_Accessor();
            string vidPath = sampleVideos[2];
            IVideo video = new Video(false, vidPath, new YuvVideoInfo(vidPath), null);
            Thread expected = target.playTickerThread;
            Thread actual;
            target.playTickerThread = expected;
            actual = target.playTickerThread;
            Assert.AreEqual(expected, actual);
            Assert.IsNotNull(actual);
        }
    }
}
