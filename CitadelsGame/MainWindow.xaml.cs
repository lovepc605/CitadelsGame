using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using CitadelsSystem;
using CitadelsSystem.Character;
using CitadelsSystem.Players;
using CitadelsSystem.Utils;
using CitadelsSystem.Interface;
using log4net;
using log4net.Config;

namespace CitadelsGame
{

    public partial class MainWindow : Window
    {
        
        #region Field

        public static string LogPath = AppDomain.CurrentDomain.BaseDirectory + @"conf\log.config";
        public static ILog log;

        public string PlayerName = "";

        //System message binding
        private BackgroundWorker sysMsgWorker;
        private MessageAdapter sysMsgAdapter;

        #endregion

        #region Initialize

        public MainWindow(string playerName)
        {
            PlayerName = playerName;

            MyIni();

            InitializeComponent();

            InitializeGame();

            InitializeWindow();

            GameStart();
        }

        private void MyIni()
        {
            
            log = LogManager.GetLogger("In main program");
            XmlConfigurator.Configure(new System.IO.FileInfo(LogPath));//read log config file
            
        }

        void systemMsgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while(true)
            {
                sysMsgAdapter.PrintMessage();
                this.Dispatcher.BeginInvoke(DispatcherPriority.DataBind,
                (ThreadStart)delegate()
                {
                    textBox1.Text = sysMsgAdapter.SystemMsg;
                }
                );
                Thread.Sleep(100);

            }
        }

        private void InitializeWindow()
        {


            //set for background
            BitmapSource citadels = ResourceHelper.LoadBitmap("Citadels");
            imgMain.Source = citadels;
            //設定&初始 PlayerArea UI 連結 
            #region set Player area
            {
                int count = 0;
                int row = 0, col = 1;
                foreach (AbstractPlayer p in GameSystem.Players)
                {
                    DockPanel dp = new DockPanel();
                    PlayerArea pa = new PlayerArea();

                    //binding object
                    pa.SetBinding(p);
                    
                    //UI dock
                    dp.Children.Add(pa);
                    dp.LastChildFill = true;

                    row = 1 + (count) % 4;

                    if (count >= 4)
                    {
                        col = 2;
                    }
                    //add the row to the specific row and col
                    gridPlayerArea.Children.Add(dp);
                    Grid.SetColumn(dp, col);
                    Grid.SetRow(dp, row);

                    count++;

                    if(p!= GameSystem.TheRealPlayer)
                    {
                        pa.btnHandCard.IsEnabled = false;
                    }
                    //pa.btnMoney.IsEnabled = false;
                    //pa.btnHandCard.Click += new RoutedEventHandler(btnHandCard_Click);
                }


            }
            #endregion

            //設定Game System message UI update
            sysMsgWorker = new BackgroundWorker();
            sysMsgWorker.DoWork += systemMsgWorker_DoWork;

            sysMsgAdapter = new MessageAdapter();


            ////設定GameThread
            //actionWorker = new BackgroundWorker();
            //actionWorker.DoWork += actionWorker_DoWork;

            

        }

        private void InitializeGame()
        {
            GameSystem.OnUserNewRound += new UserNewRound(GameSystem_OnUserNewRound);
            GameSystem.OnUserEndRound += new UserEndRound(GameSystem_OnUserEndRound);
            GameSystem.OnAIUserNewRound += new AIUserNewRound(GameSystem_OnAIUserNewRound);
            GameSystem.OnSystemEnd += new SystemEndRound(GameSystem_OnSystemEndRound);
            //GameSystem.OnCharKilled += new CharKilled(GameSystem_OnCharKilled);

            GameSystem.IsGameOver = false;

            //設定卡片
            GameSystem.LoadCards();

            //設定玩家人數 & 起始順序
            GameSystem.InitializePlayers(7, PlayerName);

            //讀取角色資料
            GameSystem.LoadCharacters();

            //設定Player 選單
            InitializeRealPlayer();
        }

        private void InitializeRealPlayer()
        {
            AbstractPlayer player = GameSystem.TheRealPlayer;
            
            //選擇卡片
            player.ChooseCard = new PlayerWindow.CardUser();

            player.ChooseBuilding = new PlayerWindow.CardUser();

            //選擇角色
            player.ChooseCharacter = new PlayerWindow.CharacterUser();

            //主選單
            player.ChooseAction = null;

            //選擇攻擊目標
            player.ChooseTarget = new PlayerWindow.TargetUser();

            //選擇 是/否
            player.ChooseYesOrNo = new PlayerWindow.YesOrNoUser();


        }

        #endregion

        #region Events

        void GameSystem_OnAIUserNewRound(object sender, EventArgs arg)
        {
            //Update UI
            NewRoundMessage();
            
            
        }

        void GameSystem_OnUserNewRound(object sender, EventArgs arg)
        {
            //Update UI
            NewRoundMessage();
            //show player and character

            //flash the border
            busyAnimation.Visibility = Visibility.Hidden;
            

            if (GameSystem.CurrentPlayer is RealPlayer)
            {
                EnableButtons(true);
            }

            //直接執行角色功能
            if(GameSystem.CurrentCharacter.ExcuteTiming == ExcuteEnum.AtBegining ||
               GameSystem.CurrentCharacter.SelectType == "None"
                )
            {
                btnbtnCharFunc_Click(null, null);
            }

        }

        void GameSystem_OnSystemNewRound(object sender, EventArgs arg)
        {
            txbRound.Text = "回合數：" + GameSystem.CurrentRound;

        }

        void GameSystem_OnUserEndRound(object sender, EventArgs arg)
        {
            //stop flashing the border
            EnableButtons(false);
            busyAnimation.Visibility = Visibility.Visible;
            
        }

        void GameSystem_OnSystemEndRound(object sender, EventArgs arg)
        {
            ShowResult();
        }


        public void EnableButtons(bool value)
        {
            btnTakeMoney.IsEnabled = value;
            btnTakeCard.IsEnabled = value;

            btnBuild.IsEnabled = value;

            btnTax.IsEnabled = value;
            btnPurple.IsEnabled = value;
            btnCharFunc.IsEnabled = value;
            btnEnd.IsEnabled = value;
        }

        #endregion

        #region Methods
       
        private void NewRoundMessage()
        {
            tbCharName.Text = GameSystem.CurrentCharacter.Name;
            tbCharID.Text = GameSystem.CurrentCharacter.Id.ToString();
            tbPlayerName.Text = GameSystem.CurrentPlayer.Name;
            tbCardLeft.Text = GameSystem.CardStack.Count.ToString();
            //tbFaceUP.Text = GameSystem.FaceUpChar.Name;

            Thread.Sleep(100);
            SayWord();

            //show player and character

            if (GameSystem.CurrentCharacter.IsStolen)
            {
                new ShowMessageWindow().SetData(GameSystem.CurrentPlayer, "被偷了!");
                Thread.Sleep(100);
            }
            else
            {
                new ShowMessageWindow().SetData(GameSystem.CurrentPlayer, "");
            }
            txbRound.Text = "回合數：" + GameSystem.CurrentRound;
        }
        
        private void SayWord()
        {
            if(GameSystem.CurrentPlayer is AiPlayer)
            {
                AiPlayer p =(GameSystem.CurrentPlayer as AiPlayer);

                if (p.Words.Count > 0)
                {
                    string word = p.Words[GameSystem.RandomNum.Next(0, 100) % p.Words.Count];
                    tbPlayerWord.Text = p.Name + "：" + word;
                    p.Words.Remove(word);
                }
                else
                    tbPlayerWord.Text = "";
            }
            else
            {
                tbPlayerWord.Text = "";
            }
        }

        private void GameStart()
        {
            //play the music , load pics

            //initialize the board 
            sysMsgWorker.RunWorkerAsync();
            //actionWorker.RunWorkerAsync();

            this.Dispatcher.BeginInvoke(DispatcherPriority.Background,
           (ThreadStart)delegate()
           {
               PlayingGame();
           }
           );
            
        }

        private void PlayingGame()
        {

            //1.遊戲開始，選取角色，共八個
           
            
            //2.直到遊戲結束那輪 才跳出

            //Console.Clear();
            //1.開始新的回合 , clear player ,clear character
            try
            {
                GameSystem.StartNewRound();
                
            }
            catch(Exception e)
            {

                log.Error("UnHandledException", e);
            }
            
            //2.根據角色 ID 順序執行 

        }
        
        private void ShowResult()
        {
            //Console.WriteLine("結果如下");
            var winner = from p in GameSystem.Players orderby p.CalculatePoints() descending select p;
            ShowFinalResult(winner.ToList());
        }

        public void ShowFinalResult(List<AbstractPlayer> input)
        {
            sysMsgWorker.Dispose();
            //show a dialog for the score result
            ResultWindow win = new ResultWindow();
            win.SetData(input);
            VoiceHelper.AllPlayerSound("Applaud", 100);
            win.ShowDialog();
        }

        #endregion

        #region Button Event
        public bool IsValid()
        {

            if (GameSystem.TheRealPlayer == GameSystem.TheStartPlayer)
            {
                Character c = GameSystem.CurrentCharacter;
                if (c.IsCardOrMoneyTaken)
                {
                    btnTakeMoney.IsEnabled = false;
                    btnTakeCard.IsEnabled = false;
                }

                if (c.IsBuilt)
                {
                    btnBuild.IsEnabled = false;
                }

                if (c.IsTaxed)
                {
                    btnTax.IsEnabled = false;
                }

                if (c.IsSpecialFuncUsed)
                {
                    btnCharFunc.IsEnabled = false;
                }
                if(c.IsEnd)
                {
                    btnEnd.IsEnabled = false;
                }

                return true;
            }
            return false;
        }

        //0:End ,1:TakeMoney ,2:TakeCard 
        //,3:Build ,4:Tax, 5:InvokeFunction ,6: purple

        private void btnTakeMoney_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                GameSystem.MakeChoice(GameSystem.TheRealPlayer, 1);
                VoiceHelper.AllPlayerSound("Coins", 100);
                IsValid();
                GameSystem.RefreshUI();
            }
        }

        private void btnTakeCard_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                GameSystem.MakeChoice(GameSystem.TheRealPlayer, 2);
                VoiceHelper.AllPlayerSound("DealCard", 100);
                IsValid();
                GameSystem.RefreshUI();
            }

        }

        private void btnBuild_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                GameSystem.MakeChoice(GameSystem.TheRealPlayer, 3);
                VoiceHelper.AllPlayerSound("Build", 100);
                IsValid();
                GameSystem.RefreshUI();
            }
        }

        private void btnTax_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                GameSystem.MakeChoice(GameSystem.TheRealPlayer, 4);
                VoiceHelper.AllPlayerSound("Coins", 100);
                IsValid();
                GameSystem.RefreshUI();
            }
        }

        private void btnbtnCharFunc_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                GameSystem.MakeChoice(GameSystem.TheRealPlayer, 5);
                IsValid();
                GameSystem.RefreshUI();

                if (GameSystem.CurrentCharacter.IsEnd)
                    btnEnd_Click(null,null);
            }
        }

        private void btnPurple_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                GameSystem.MakeChoice(GameSystem.TheRealPlayer, 6);
                IsValid();
                GameSystem.RefreshUI();
            }
        }

        private void btnEnd_Click(object sender, RoutedEventArgs e)
        {
            //判斷
            if (IsValid())
            {
                GameSystem.MakeChoice(GameSystem.TheRealPlayer, 0);
                VoiceHelper.AllPlayerSound("Start", 100);
                IsValid();
                GameSystem.NextChar();
                //GameSystem.TheRealPlayer.RefreshPlayerInfo();
            }
        }

        #endregion

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(0);
        }

    }

    //System Message binding , between the GameSystem.MessageQueue and TextBox-UI
    public class MessageAdapter
    {
        private string temp="";
        
        
        public void PrintMessage()
        {
            List<string> data;
            lock (GameSystem.MessageQueue)
            {
                data = GameSystem.MessageQueue.ToList();
                GameSystem.MessageQueue.Clear();
            }
            if (data.Count == 0)
                return;


            for (int i = 0; i < data.Count; i++)
            {
                temp = data[i] + "\r\n" + temp;
            }

            SystemMsg = temp;
            
        }

        public string SystemMsg
        {
            get
            {
                return temp;
            }
            set
            {
                temp = value;

            }
        }
    }
}
