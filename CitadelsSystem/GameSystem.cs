using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using CitadelsSystem.Interface;
using CitadelsSystem.Players;
using CitadelsSystem.Properties;
using CitadelsSystem.Card;
using CitadelsSystem.Character;
using CitadelsSystem.Utils;

namespace CitadelsSystem
{
    #region Delegate & Event
    
    public delegate void UserNewRound(object sender, EventArgs arg);
    public delegate void UserEndRound(object sender, EventArgs arg);
    public delegate void AIUserNewRound(object sender, EventArgs arg);
    public delegate void SystemEndRound(object sender, EventArgs arg);
    public delegate void CharKilled(object sender, EventArgs arg);


    public delegate void UserAction(object sender, ActionEventArgs arg);

    #endregion

    public class GameSystem
    { 
        public static event UserNewRound OnUserNewRound;
        public static event UserEndRound OnUserEndRound;
        public static event AIUserNewRound OnAIUserNewRound;
        public static event SystemEndRound OnSystemEnd;
        //public static event CharKilled OnCharKilled;

        private static Mutex uiMutex = new Mutex();
        private static object uiObj = new object();

        #region Fields

        public static GameCharEnum SelectCharEnum ;


        public static List<AbstractCard> AllCards;
        /// <summary>
        /// 牌堆
        /// </summary>
        public static LinkedList<AbstractCard> CardStack;

        /// <summary>
        /// 棄牌
        /// </summary>
        public static LinkedList<AbstractCard> JunkStack;
        
        /// <summary>
        /// 目前回合數
        /// </summary>
        public static int CurrentRound =0;

        public static Character.Character FaceUpChar;

        /// <summary>
        /// Loaded Characters
        /// </summary>
        public static List<Character.Character> Characters;

        /// <summary>
        /// All Players in the game , the index is the position for this player
        /// </summary>
        public static List<AbstractPlayer> Players;
         
        public static AbstractPlayer TheStartPlayer;

        public static Character.Character CurrentCharacter;

        public static AbstractPlayer CurrentPlayer
        {
            get { return CurrentCharacter.CurrentPlayer; }
        }

        public static Queue<string> MessageQueue = new Queue<string>();
         
        public static Queue<string> WarningMessageQueue = new Queue<string>();

        public static bool IsGameOver = false;

        public static bool IsFirstEnd = false;

        public static AbstractPlayer TheRealPlayer;

        public static AbstractCard WizardDrawCard;

        public int TotalPlayer
        {
            get { return Players.Count; }
        }

        #endregion

        #region Initialize    

        private GameSystem()
        {
        }

        static GameSystem()
        {
            AddSystemMessage("富饒之城遊戲開始...");
            
        }

        public static Random RandomNum = new Random();

        /// <summary>
        /// Total players in the game , the real player is only one currently.
        /// Load words, dispatch cards and money for each player
        /// </summary>
        /// <param name="totalPlayers"></param>
        public static void InitializePlayers(int totalPlayers, string name)
        {


            Players = new List<AbstractPlayer>(totalPlayers);

            TheRealPlayer = new RealPlayer(name);
            //TheRealPlayer = new AiPlayer(name);
            Players.Add(TheRealPlayer);


            //AddSystemMessage("載入台詞＆AI人物...");
            //1.讀取Resource file , Players set up
            string[] words = Resources.Words.Split( new string[]{ "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 1; i < totalPlayers; i++)
            {
                List<string> temp = new List<string>(words[i].Split(','));

                AiPlayer ai = new AiPlayer(temp[0]);
                temp.RemoveAt(0);
                ai.Words = temp;

                Players.Add(ai);
                //AddSystemMessage(ai.Name + " : "+ temp.Count);
            }

            //2.Choose the position for each players , RealPlayer always be the first!


            //3.Choose the realplayer as  startPlayer 
            TheStartPlayer = Players[0];

            
        }

        /// <summary>
        /// Load the cards 
        /// </summary>
        /// <param name="dataPath"></param>
        public static void LoadCards()
        {
            JunkStack = new LinkedList<AbstractCard>();
            CardStack = new LinkedList<AbstractCard>();
            AllCards = new List<AbstractCard>();
            


            //AddSystemMessage("開始載入卡片...");
            string simpleCard = Resources.SimpleCard;
            string purpleCard = Resources.PurpleCard;

            
            List<Card.AbstractCard> tempList = new List<CitadelsSystem.Card.AbstractCard>();
            
            
            //Name,Point,Color,FileName,Number;
            foreach (string cardstr in simpleCard.Split(new string[] { @";", "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                tempList.AddRange(SimpleCardFactory.Create(cardstr));
                //AddSystemMessage("載入普通卡片:" + cardstr);
            }
            //AddSystemMessage("載入普通卡片成功");


            //Name,Point,Color,FileName,Number,Exec,Func,Desc;
            foreach (string cardstr in purpleCard.Split(new string[] { @";", "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
            {

                tempList.AddRange(PurpleCardFactory.Create(cardstr));
                //AddSystemMessage("載入紫色卡片:" + cardstr);
            }
            //AddSystemMessage("載入紫色卡片成功");

            //shuffle the cards , and put them in the stack
            while(tempList.Count>0)
            {
                int index = ((int)(RandomNum.NextDouble() * 123)) % tempList.Count;
                AllCards.Add(tempList[index]);
                CardStack.AddLast(tempList[index]);
                tempList.RemoveAt(index);

            }
            //AddSystemMessage("洗牌完畢");
        }

        /// <summary>
        /// Load chars
        /// </summary>
        public static void LoadCharacters()
        {
            AddSystemMessage("開始載入遊戲角色...");
            Characters = new List<CitadelsSystem.Character.Character>();
            string charStd = Resources.CharStd;
            string charExt = Resources.CharExt;

            //ID,Name,Color,Excutable,Func,FileName,Desc;
            
            foreach (string c in charStd.Split(new string[] { @";", "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                Characters.Add(CharacterFactory.Create(c));
                //AddSystemMessage("載入標準角色:" + c);
            }

            
            foreach (string c in charExt.Split(new string[] { @";", "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                Characters.Add(CharacterFactory.Create(c));
                //AddSystemMessage("載入擴充角色:" + c);
            }
            

            SelectCharacters();
             

        }

        /// <summary>
        /// 從讀取進來的角色 進行設定
        /// </summary>
        public static void SelectCharacters()
        {

            List<Character.Character> tempChar = new List<CitadelsSystem.Character.Character>();

            //1.隨機選取
            //假設選8 / 9 個 (根據人數)
            Character.Character selected;
            for (int i = 0; i < 8; i++ ) 
            {

                switch (SelectCharEnum)
                {
                    case GameCharEnum.Standard:
                        selected = Characters[i]; 
                        break;
                    case GameCharEnum.Extension:
                        selected = Characters[i+9]; 
                        break;
                    case GameCharEnum.Mix:
                        selected = RandomNum.NextDouble() > 0.5 ? Characters[i] : Characters[i + 9];
                        break;
                    default:
                        throw new Exception("Select char problem!");
                }
       
                
                tempChar.Add(selected);
                AddSystemMessage("選取角色：" + selected.ToString());
            }

            Characters = tempChar;
                
        }

        #endregion

        #region Methods

        public static void AddSystemMessage(string input)
        {
            lock(MessageQueue)
            {
                MessageQueue.Enqueue(input);
            }
        }


        /// <summary>
        /// Start a new round.
        /// </summary>
        public static void StartNewRound()
        {
            

            ++CurrentRound;
            
            WizardDrawCard = null;
            VoiceHelper.AllPlayerSound("Loading_1", 100);
            ResetChars();
            ResetPlayers(); 
            RefreshUI();
            

            AddSystemMessage(string.Format("第{0}回合開始", CurrentRound));
            
            NextChar(); 
        }

        public static void RefreshUI()
        {
            try
            {
                uiMutex.WaitOne();
                foreach (AbstractPlayer p in GameSystem.Players)
                {

                    p.RefreshPlayerInfo();
                    Thread.Sleep(50);
                }
            }
            finally
            {
                uiMutex.ReleaseMutex();
            }

        }

        /// <summary>
        /// Check if the game is over.(Someone has eight buildings)
        /// </summary>
        public static void EndThisRound()
        {
            
            if(IsGameOver)
            {
                Scoring();   
            }
        }

        /// <summary>
        /// return the final score for each player
        /// </summary>
        /// <returns>the score and the PlayerName pair</returns>
        public static SortedList<int,string> Scoring()
        {
            SortedList<int, string> scoreList = new SortedList<int, string>();
            foreach(AbstractPlayer p in Players)
            {
                scoreList.Add(p.CalculatePoints(),p.Name);
            }
            return scoreList;
        }
        
        // invoke this method when starting a new round , reset the parameters for each character
        public static void ResetChars()
        {

            //清除character 狀態
            foreach (Character.Character ch in Characters)
            {
                ch.StartNewRound();
            }
            ////設定FaceUP
            //int index = RandomNum.Next(0, 100);
            
            //FaceUpChar = Characters[index%Characters.Count];
            //FaceUpChar.CurrentState = StateEnum.FaceUp;
        }

        public static void ResetPlayers()
        {
            //1.重新讓User選擇char , 要根據 國王的順序
            // the index of the king .
            int index = Players.IndexOf(TheStartPlayer);

            //2.根據人數的關係 ，要蓋牌 (＆明牌)
            Character.Character tempChar = Characters[(RandomNum.Next(0, Characters.Count))];
            tempChar.CurrentState = StateEnum.FaceDown;

            //3.每個player都重新選擇char , 但是從 index 開始
            for (int i = 0; i < Players.Count; i++)
            {
                //最後選的玩家
                if(i == 6)
                {
                    tempChar.CurrentState = StateEnum.NotSelected;
                }

                //找出沒選過的char讓 player 去選 
                var obj = from c in Characters where (c.CurrentState == StateEnum.NotSelected) select c;
                Character.Character newChar = Players[index].ChooseCharacter.Choose(obj.ToList());
                Players[index].StartNewRound(newChar);
                index = (index + 1)%Players.Count;
               
            }
        }

        public static void NextChar()
        {
            //取得目前應該要執行的角色，並判斷是否合法
            
            var obj = (from c in Characters where !c.IsEnd && c.CurrentState == StateEnum.Selected && !c.IsKilled select c);

            


            Character.Character[] tmp = obj.ToArray();
            //判斷目前角色是否被殺,被選

            //讓下個玩家開始遊戲
            if (tmp.Length > 0)
            {
                
                //設定目前運作角色
                CurrentCharacter = tmp[0];

                //if (CurrentCharacter.IsKilled)
                //{
                //    //發出被殺
                //    VoiceHelper.AllPlayerSound("No_3", 100);
                //    OnCharKilled(null, null);
                //    CurrentCharacter.IsEnd = true;
                //    NextChar();
                //}


                if (CurrentCharacter.IsStolen)
                {
                    //發出被偷
                    VoiceHelper.AllPlayerSound("No_2", 100);

                    Characters[1].CurrentPlayer.Money += CurrentCharacter.CurrentPlayer.Money;
                    CurrentCharacter.CurrentPlayer.Money = 0;
                }
                
                PlayingTurn();
                
            }

            else
            {
                //開始新的回合
                if (!IsGameOver)
                {
                    StartNewRound();
                }
                else
                {
                    //發出遊戲結束~
                    OnSystemEnd(null, null);
                    return;
                }
            }


        }

        public static Character.Character PlayerToChar(AbstractPlayer input)
        {
            var obj = from c in Characters where (c.CurrentPlayer!=null && c.CurrentPlayer.Name == input.Name) select c;
            return obj.ToArray()[0];
        }

        public static void PlayingTurn()
        {

            AbstractPlayer player = CurrentPlayer;
            
            AddSystemMessage("=======================================================");
            AddSystemMessage(string.Format("目前玩家：{0} ,選取角色：{1} ,金幣 {2}, 手牌{3} ,建築{4}", player.Name, CurrentCharacter, player.Money, player.HandCards.Count,player.Buildings.Count));
            int count = 0;
            VoiceHelper.AllPlayerSound("Loading_2", 1500);
            RefreshUI();




            #region 1.Invoke the Purple card function first (Excutable == false)

            var purpleList = from p in player.Buildings where (p is PurpleCard) && !(p as PurpleCard).Excutable select p;
            foreach(PurpleCard p in purpleList)
            {
                p.Invoke(player);
            }

            #endregion

            if (player is RealPlayer)
            {
                //玩家開始
                OnUserNewRound(null, null);
                
            }
            else
            {
                //AI玩家開始
                OnAIUserNewRound(null,null);

                #region 2.Do the action which is selected 
                do
                {
                    count++;
 
                    int choice = CurrentCharacter.ChooseAction.ChooseActionList();
                    // 接Character傳回來的選項, 
                    // 0:End ,1:TakeMoney ,2:TakeCard ,3:Build ,4:Tax, 5:InvokeFunction ,6: purple
                    MakeChoice(player, choice);
                    RefreshUI();
                    //player.RefreshPlayerInfo();
                    //CurrentCharacter.ChooseAction.Reset();
                    //使用者按了End就跳出 || 執行太久跳出
                } while (!CurrentCharacter.IsEnd && count < 15);
                CurrentCharacter.IsEnd = true;

                

                NextChar();
                
                #endregion
            }
        }

        public static void MakeChoice(AbstractPlayer player, int choice)
        {
            if (player != CurrentPlayer)
                return;

            switch (choice)
            {
                case 0:
                    {
                        CurrentCharacter.IsEnd = true;
                        AddSystemMessage(player + "結束這回合");
                        OnUserEndRound(null, null);
                    }
                    break;

                case 1:
                    {
                        //1.1 拿＄
                        player.Money += 2;
                        AddSystemMessage(player + "拿了2枚金幣");
                        CurrentCharacter.IsCardOrMoneyTaken = true;

                    }
                    break;
                case 2:
                    {
                        //1.2 抽牌
                        player.DrawCardPick(player.DrawNewCard, player.KeepNewCard);
                        AddSystemMessage(string.Format("{0}抽了{1}張手牌", player, player.KeepNewCard));
                        CurrentCharacter.IsCardOrMoneyTaken = true;
                        //player.ToBuildNumber = 1;
                    }
                    break;
                case 3:
                    {
                        //1.3 蓋房子,這回合沒蓋過

                        player.DoBuilding();


                    }
                    break;
                case 4:
                    {
                        //1.4進行徵稅
                        int tax = CurrentCharacter.Tax();
                        player.Money += tax;

                    }
                    break;
                case 5:
                    {
                        //1.5行使角色特權
                        CurrentCharacter.DoAction();
                        break;

                    }
                case 6:
                    {
                        //1.6行使紫色卡片特權 , 未執行 ,可執行的
                        List<AbstractCard> purpleList = player.Buildings.Where(b => (b is PurpleCard) && (b as PurpleCard).Excutable && !(b as PurpleCard).IsUsed).Select(p => p).ToList();
                        if(purpleList.Count>0)
                        {
                            AbstractCard card = player.ChooseCard.Choose("請選擇想要使用的牌卡", 1, purpleList)[0];
                           (card as PurpleCard).Invoke(player);
                        }

                        break;
                    }

                default:
                    throw new Exception("ChooseAction Command not found!");
            }
        }

        #endregion


        
    }

    public class ActionEventArgs: EventArgs
    {
        public int Action{get;set;}

    }
}
