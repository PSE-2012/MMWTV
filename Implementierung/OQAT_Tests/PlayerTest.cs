using PP_Player;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;
using Oqat.PublicRessources.Model;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Threading;
using Oqat.PublicRessources.Plugin;
using System.Windows.Controls;
using PS_YuvVideoHandler;
using Oqat.Model;
using System.IO;
namespace OQAT_Tests
{
    
    
    /// <summary>
    ///Dies ist eine Testklasse für "PlayerTest" und soll
    ///alle PlayerTest Komponententests enthalten.
    ///</summary>
    [TestClass()]
    public class PlayerTest
    {
        private static string testfolder = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\..\\..\\..\\..\\OQAT_Tests\\TestData\\";
        private static string testVidPath = testfolder + "akiyo_qcif.yuv";

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
        [TestInitialize()]
        public void MyTestInitialize()
        {
            if (!Directory.Exists(testfolder))
            {
                Directory.CreateDirectory(testfolder);
                Player target = new Player();
                YuvVideoInfo vidInfo = new YuvVideoInfo(testVidPath);
                Video testVideo = new Video(false, testfolder, vidInfo, null);
                IVideoHandler handler = testVideo.handler;
                target.setVideo(testVideo, 0);
            }

        }
        //
        //Mit TestCleanup können Sie nach jedem einzelnen Test Code ausführen.
        [TestCleanup()]
        public void MyTestCleanup()
        {
            if (Directory.Exists(testfolder))
            {
                Directory.Delete(testfolder, true);
            }
        }
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
        public void CloneTest()
        {
            Player target = new Player();
            object actual;
            actual = target.createExtraPluginInstance();
            Assert.IsTrue(actual is Player, "The returned object is not a valid Player instance. ");
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
            target.playTickerThread.Start();
            object sender = new Button();
            RoutedEventArgs e = null;
            Assert.AreEqual(ThreadState.Running, target.playTickerThread.ThreadState);
            target.Pause_Click(sender, e);
            Thread.Sleep(500);
            //Thread.CurrentThread.sleep(500);
            Assert.AreEqual(ThreadState.WaitSleepJoin, target.playTickerThread.ThreadState);
        }

        /// <summary>
        ///Ein Test für "Play_Click"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PP_Player.dll")]
        public void Play_ClickTest()
        {
            Player_Accessor target = new Player_Accessor();
            target.playTickerThread.Start();
            object sender = new Button();
            RoutedEventArgs e = null;
            target.playTickerThread.Suspend();
            Assert.AreEqual(ThreadState.Suspended, target.playTickerThread.ThreadState);
            target.Play_Click(sender, e);
            Thread.Sleep(500);
            Assert.AreEqual(ThreadState.Suspended, target.playTickerThread.ThreadState);
        }

        /// <summary>
        ///Ein Test für "Stop_Click"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PP_Player.dll")]
        public void Stop_ClickTest()
        {
            Player_Accessor target = new Player_Accessor();
            object sender = null;
            RoutedEventArgs e = null;
            target.Stop_Click(sender, e);
            Assert.Inconclusive("Eine Methode, die keinen Wert zurückgibt, kann nicht überprüft werden.");
        }

        /// <summary>
        ///Ein Test für "System.Windows.Markup.IComponentConnector.Connect"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PP_Player.dll")]
        public void ConnectTest()
        {
            IComponentConnector target = new Player(); // TODO: Passenden Wert initialisieren
            int connectionId = 0; // TODO: Passenden Wert initialisieren
            object target1 = null; // TODO: Passenden Wert initialisieren
            target.Connect(connectionId, target1);
            Assert.Inconclusive("Eine Methode, die keinen Wert zurückgibt, kann nicht überprüft werden.");
        }

        /// <summary>
        ///Ein Test für "flush"
        ///</summary>
        [TestMethod()]
        public void flushTest()
        {
            Player target = new Player();
            target.flush();
            Assert.Inconclusive("Eine Methode, die keinen Wert zurückgibt, kann nicht überprüft werden.");
        }

        /// <summary>
        ///Ein Test für "getFrame"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PP_Player.dll")]
        public void getFrameTest()
        {
            Player_Accessor target = new Player_Accessor(); // TODO: Passenden Wert initialisieren
            int position = 0; // TODO: Passenden Wert initialisieren
            target.getFrame(position);
            Assert.Inconclusive("Eine Methode, die keinen Wert zurückgibt, kann nicht überprüft werden.");
        }

        /// <summary>
        ///Ein Test für "getMemento"
        ///</summary>
        [TestMethod()]
        public void getMementoTest()
        {
            Player target = new Player(); // TODO: Passenden Wert initialisieren
            Memento expected = null; // TODO: Passenden Wert initialisieren
            Memento actual;
            actual = target.getMemento();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Überprüfen Sie die Richtigkeit dieser Testmethode.");
        }

        /// <summary>
        ///Ein Test für "jumpToFrameTextBox_GotFocus"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PP_Player.dll")]
        public void jumpToFrameTextBox_GotFocusTest()
        {
            Player_Accessor target = new Player_Accessor(); // TODO: Passenden Wert initialisieren
            object sender = null; // TODO: Passenden Wert initialisieren
            RoutedEventArgs e = null; // TODO: Passenden Wert initialisieren
            target.jumpToFrameTextBox_GotFocus(sender, e);
            Assert.Inconclusive("Eine Methode, die keinen Wert zurückgibt, kann nicht überprüft werden.");
        }

        /// <summary>
        ///Ein Test für "jumpToFrame_Click"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PP_Player.dll")]
        public void jumpToFrame_ClickTest()
        {
            Player_Accessor target = new Player_Accessor(); // TODO: Passenden Wert initialisieren
            object sender = null; // TODO: Passenden Wert initialisieren
            RoutedEventArgs e = null; // TODO: Passenden Wert initialisieren
            target.jumpToFrame_Click(sender, e);
            Assert.Inconclusive("Eine Methode, die keinen Wert zurückgibt, kann nicht überprüft werden.");
        }

        /// <summary>
        ///Ein Test für "nextFrame_Click"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PP_Player.dll")]
        public void nextFrame_ClickTest()
        {
            Player_Accessor target = new Player_Accessor(); // TODO: Passenden Wert initialisieren
            object sender = null; // TODO: Passenden Wert initialisieren
            RoutedEventArgs e = null; // TODO: Passenden Wert initialisieren
            target.nextFrame_Click(sender, e);
            Assert.Inconclusive("Eine Methode, die keinen Wert zurückgibt, kann nicht überprüft werden.");
        }

        /// <summary>
        ///Ein Test für "playTicker"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PP_Player.dll")]
        public void playTickerTest()
        {
            Player_Accessor target = new Player_Accessor(); // TODO: Passenden Wert initialisieren
            target.playTicker();
            Assert.Inconclusive("Eine Methode, die keinen Wert zurückgibt, kann nicht überprüft werden.");
        }

        /// <summary>
        ///Ein Test für "positionSlider_DragCompleted"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PP_Player.dll")]
        public void positionSlider_DragCompletedTest()
        {
            Player_Accessor target = new Player_Accessor(); // TODO: Passenden Wert initialisieren
            object sender = null; // TODO: Passenden Wert initialisieren
            DragCompletedEventArgs e = null; // TODO: Passenden Wert initialisieren
            target.positionSlider_DragCompleted(sender, e);
            Assert.Inconclusive("Eine Methode, die keinen Wert zurückgibt, kann nicht überprüft werden.");
        }

        /// <summary>
        ///Ein Test für "positionSlider_DragDelta"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PP_Player.dll")]
        public void positionSlider_DragDeltaTest()
        {
            Player_Accessor target = new Player_Accessor(); // TODO: Passenden Wert initialisieren
            object sender = null; // TODO: Passenden Wert initialisieren
            DragDeltaEventArgs e = null; // TODO: Passenden Wert initialisieren
            target.positionSlider_DragDelta(sender, e);
            Assert.Inconclusive("Eine Methode, die keinen Wert zurückgibt, kann nicht überprüft werden.");
        }

        /// <summary>
        ///Ein Test für "positionSlider_DragStarted"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PP_Player.dll")]
        public void positionSlider_DragStartedTest()
        {
            Player_Accessor target = new Player_Accessor(); // TODO: Passenden Wert initialisieren
            object sender = null; // TODO: Passenden Wert initialisieren
            DragStartedEventArgs e = null; // TODO: Passenden Wert initialisieren
            target.positionSlider_DragStarted(sender, e);
            Assert.Inconclusive("Eine Methode, die keinen Wert zurückgibt, kann nicht überprüft werden.");
        }

        /// <summary>
        ///Ein Test für "previousFrame_Click"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PP_Player.dll")]
        public void previousFrame_ClickTest()
        {
            Player_Accessor target = new Player_Accessor(); // TODO: Passenden Wert initialisieren
            object sender = null; // TODO: Passenden Wert initialisieren
            RoutedEventArgs e = null; // TODO: Passenden Wert initialisieren
            target.previousFrame_Click(sender, e);
            Assert.Inconclusive("Eine Methode, die keinen Wert zurückgibt, kann nicht überprüft werden.");
        }

        /// <summary>
        ///Ein Test für "return_Click"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PP_Player.dll")]
        public void return_ClickTest()
        {
            Player_Accessor target = new Player_Accessor(); // TODO: Passenden Wert initialisieren
            object sender = null; // TODO: Passenden Wert initialisieren
            KeyEventArgs e = null; // TODO: Passenden Wert initialisieren
            target.return_Click(sender, e);
            Assert.Inconclusive("Eine Methode, die keinen Wert zurückgibt, kann nicht überprüft werden.");
        }

        /// <summary>
        ///Ein Test für "setMemento"
        ///</summary>
        [TestMethod()]
        public void setMementoTest()
        {
            Player target = new Player(); // TODO: Passenden Wert initialisieren
            Memento memento = null; // TODO: Passenden Wert initialisieren
            target.setMemento(memento);
            Assert.Inconclusive("Eine Methode, die keinen Wert zurückgibt, kann nicht überprüft werden.");
        }

        /// <summary>
        ///Ein Test für "setVideo"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PP_Player.dll")]
        public void setVideoTest()
        {
            Player_Accessor target = new Player_Accessor(); // TODO: Passenden Wert initialisieren
            object video = null; // TODO: Passenden Wert initialisieren
            target.setVideo(video);
            Assert.Inconclusive("Eine Methode, die keinen Wert zurückgibt, kann nicht überprüft werden.");
        }

        /// <summary>
        ///Ein Test für "setVideo"
        ///</summary>
        [TestMethod()]
        public void setVideoTest1()
        {
            Player target = new Player();
            IVideo video = new Video(false, testfolder, new YuvVideoInfo(testVidPath), null);
            IVideoHandler handler = video.handler;
            int position = 0; // TODO: Passenden Wert initialisieren
            target.setVideo(video, position);
            Assert.Inconclusive("Eine Methode, die keinen Wert zurückgibt, kann nicht überprüft werden.");
        }

        /// <summary>
        ///Ein Test für "slowDownButton_Click"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PP_Player.dll")]
        public void slowDownButton_ClickTest()
        {
            Player_Accessor target = new Player_Accessor(); // TODO: Passenden Wert initialisieren
            object sender = null; // TODO: Passenden Wert initialisieren
            RoutedEventArgs e = null; // TODO: Passenden Wert initialisieren
            target.slowDownButton_Click(sender, e);
            Assert.Inconclusive("Eine Methode, die keinen Wert zurückgibt, kann nicht überprüft werden.");
        }

        /// <summary>
        ///Ein Test für "speedUpButton_Click"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PP_Player.dll")]
        public void speedUpButton_ClickTest()
        {
            Player_Accessor target = new Player_Accessor(); // TODO: Passenden Wert initialisieren
            object sender = null; // TODO: Passenden Wert initialisieren
            RoutedEventArgs e = null; // TODO: Passenden Wert initialisieren
            target.speedUpButton_Click(sender, e);
            Assert.Inconclusive("Eine Methode, die keinen Wert zurückgibt, kann nicht überprüft werden.");
        }

        /// <summary>
        ///Ein Test für "writeToDisplay"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PP_Player.dll")]
        public void writeToDisplayTest()
        {
            Player_Accessor target = new Player_Accessor(); // TODO: Passenden Wert initialisieren
            target.writeToDisplay();
            Assert.Inconclusive("Eine Methode, die keinen Wert zurückgibt, kann nicht überprüft werden.");
        }

        /// <summary>
        ///Ein Test für "fpsIndicatorValue"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PP_Player.dll")]
        public void fpsIndicatorValueTest()
        {
            Player_Accessor target = new Player_Accessor(); // TODO: Passenden Wert initialisieren
            int expected = 0; // TODO: Passenden Wert initialisieren
            int actual;
            target.fpsIndicatorValue = expected;
            actual = target.fpsIndicatorValue;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Überprüfen Sie die Richtigkeit dieser Testmethode.");
        }

        /// <summary>
        ///Ein Test für "namePlugin"
        ///</summary>
        [TestMethod()]
        public void namePluginTest()
        {
            Player target = new Player(); // TODO: Passenden Wert initialisieren
            string actual;
            actual = target.namePlugin;
            Assert.Inconclusive("Überprüfen Sie die Richtigkeit dieser Testmethode.");
        }

        /// <summary>
        ///Ein Test für "pausePlayTicker"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PP_Player.dll")]
        public void pausePlayTickerTest()
        {
            Player_Accessor target = new Player_Accessor(); // TODO: Passenden Wert initialisieren
            ManualResetEvent actual;
            actual = target.pausePlayTicker;
            Assert.Inconclusive("Überprüfen Sie die Richtigkeit dieser Testmethode.");
        }

        /// <summary>
        ///Ein Test für "playTickerThread"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PP_Player.dll")]
        public void playTickerThreadTest()
        {
            Player_Accessor target = new Player_Accessor(); // TODO: Passenden Wert initialisieren
            Thread expected = null; // TODO: Passenden Wert initialisieren
            Thread actual;
            target.playTickerThread = expected;
            actual = target.playTickerThread;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Überprüfen Sie die Richtigkeit dieser Testmethode.");
        }

        /// <summary>
        ///Ein Test für "playTickerTimeout"
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PP_Player.dll")]
        public void playTickerTimeoutTest()
        {
            Player_Accessor target = new Player_Accessor(); // TODO: Passenden Wert initialisieren
            int expected = 0; // TODO: Passenden Wert initialisieren
            int actual;
            target.playTickerTimeout = expected;
            actual = target.playTickerTimeout;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Überprüfen Sie die Richtigkeit dieser Testmethode.");
        }

        /// <summary>
        ///Ein Test für "positionReader"
        ///</summary>
        [TestMethod()]
        public void positionReaderTest()
        {
            Player target = new Player(); // TODO: Passenden Wert initialisieren
            int expected = 0; // TODO: Passenden Wert initialisieren
            int actual;
            target.positionReader = expected;
            actual = target.positionReader;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Überprüfen Sie die Richtigkeit dieser Testmethode.");
        }

        /// <summary>
        ///Ein Test für "presentationType"
        ///</summary>
        [TestMethod()]
        public void presentationTypeTest()
        {
            Player target = new Player(); // TODO: Passenden Wert initialisieren
            PresentationPluginType actual;
            actual = target.presentationType;
            Assert.Inconclusive("Überprüfen Sie die Richtigkeit dieser Testmethode.");
        }

        /// <summary>
        ///Ein Test für "propertyView"
        ///</summary>
        [TestMethod()]
        public void propertyViewTest()
        {
            Player target = new Player(); // TODO: Passenden Wert initialisieren
            UserControl actual;
            actual = target.propertyView;
            Assert.Inconclusive("Überprüfen Sie die Richtigkeit dieser Testmethode.");
        }

        /// <summary>
        ///Ein Test für "type"
        ///</summary>
        [TestMethod()]
        public void typeTest()
        {
            Player target = new Player(); // TODO: Passenden Wert initialisieren
            PluginType actual;
            actual = target.type;
            Assert.Inconclusive("Überprüfen Sie die Richtigkeit dieser Testmethode.");
        }
    }
}
