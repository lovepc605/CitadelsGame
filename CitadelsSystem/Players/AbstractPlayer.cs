using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Media.Imaging;
using CitadelsSystem.Card;
using CitadelsSystem.Character;
using CitadelsSystem.Interface;
using CitadelsSystem.Utils;


namespace CitadelsSystem.Players
{
    public abstract class AbstractPlayer : INotifyPropertyChanged
    {

        public AbstractPlayer()
        {
            this.MyMessage = new Queue<string>();
            this.Buildings = new List<CitadelsSystem.Card.AbstractCard>(8);
            this.HandCards = new List<CitadelsSystem.Card.AbstractCard>(8);
            
            //手牌有四張
            this.DrawCardAll(4);
            //先拿2＄
            this.Money = 2;
            //設定選牌方式
        }

        #region Fields

        public Queue<string> MyMessage;
        public List<AbstractCard> HandCards;
        public List<AbstractCard> Buildings;
        
        protected int money = 0;


        protected string name ="";
        
        public Character.Character CurrentChar;


        /// <summary>
        /// 拿牌
        /// </summary>
        public IChooseCard ChooseCard;
        /// <summary>
        /// 選角色
        /// </summary>
        public IChooseCharacter ChooseCharacter;
        /// <summary>
        /// 選目標玩家
        /// </summary>
        public IChooseTarget ChooseTarget;
        /// <summary>
        /// 選擇摧毀目標
        /// </summary>
        public IChooseBuilding ChooseBuilding;

        public IChooseYesOrNo ChooseYesOrNo;

        public IChooseAction ChooseAction;

        //蓋滿8棟加分
        public int ExtraEndPoint = 0;

        //可抽的牌數
        public int DrawNewCard = 2;

        //可保留的新牌數目(可能增加為2)
        public int KeepNewCard = 1;

        //蓋過房子？
        public bool IsBuilt = false;

        // 可折底之建築價錢
        public int DiscountPoint=0;

        //可蓋房子的數目
        public int ToBuildNumber=1;

        //可拆此玩家的建築
        public bool IsAbleToDestruct = true;

        /// <summary>
        /// 建築此建築物所需額外＄
        /// </summary>
        public int ExtraBuildPoint=0;

        public bool FreeBuilt = false ;

        #endregion
     
        #region Property

        public int HandCardCount
        {
            get { return HandCards.Count; }
        }

        public string Name
        {
            get { return name; }
            set 
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public int Money
        {
            get { return money; }
            set 
            { 
                money = value;
                OnPropertyChanged("Money");
            }
        }

        public BitmapSource ImgCrown
        {
            get { return (this == GameSystem.TheStartPlayer) ? ResourceHelper.LoadBitmap("Crown") : null; }
        }

        public BitmapSource ImgFiveColor
        {
            get { return (DiffColorBuilding == 5) ? ResourceHelper.LoadBitmap("Rainbow") : null; }
        }

        #region Building Img Binding
        public BitmapSource ImgBuilding0 
        {
            get
            {
                return Buildings.Count > 0 ? Buildings[0].FrontImg : null;
            }
        }
        public BitmapSource ImgBuilding1
        {
            get
            {
                return Buildings.Count > 1 ? Buildings[1].FrontImg : null;
            }
        }
        public BitmapSource ImgBuilding2
        {
            get
            {
                return Buildings.Count > 2 ? Buildings[2].FrontImg : null;
            }
        }
        public BitmapSource ImgBuilding3
        {
            get
            {
                return Buildings.Count > 3 ? Buildings[3].FrontImg : null;
            }
        }
        public BitmapSource ImgBuilding4
        {
            get
            {
                return Buildings.Count > 4 ? Buildings[4].FrontImg : null;
            }
        }
        public BitmapSource ImgBuilding5
        {
            get
            {
                return Buildings.Count > 5 ? Buildings[5].FrontImg : null;
            }
        }
        public BitmapSource ImgBuilding6
        {
            get
            {
                return Buildings.Count > 6 ? Buildings[6].FrontImg : null;
            }
        }
        public BitmapSource ImgBuilding7
        {
            get
            {
                return Buildings.Count > 7 ? Buildings[7].FrontImg : null;
            }
        }

        #endregion


        //計算目前顏色種類
        public int DiffColorBuilding
        {
            get
            {
                if (Buildings.Count == 0)
                    return 0;

                int[] colors = new int[5];
                for (int i = 0; i < this.Buildings.Count; i++)
                {
                    if (Buildings[i].CardColor.Name == Color.Yellow.Name)
                    {
                        colors[0] = 1;
                    }
                    else if (Buildings[i].CardColor.Name == Color.Blue.Name)
                    {
                        colors[1] = 1;
                    }
                    else if (Buildings[i].CardColor.Name == Color.Green.Name)
                    {
                        colors[2] = 1;
                    }
                    else if (Buildings[i].CardColor.Name == Color.Red.Name)
                    {
                        colors[3] = 1;
                    }
                    else if (Buildings[i] is PurpleCard)
                    {
                        colors[4] = 1;
                    }
                }
                int count = 0;
                count = colors.Sum();
                //foreach (int i in colors)
                //    count += i;



                //int silverCount = (from c in Buildings where c.CardColor == Color.Silver select c).Count();
                bool hasSilver = false;
                foreach(AbstractCard card in Buildings)
                {
                    if(card.CardColor == Color.Silver)
                    {
                        hasSilver = true;
                        break;
                    }
                }

                if (hasSilver && colors[4] >= 2)
                    count++;

                if (count >= 5)
                    return 5;

                return count;
            }
        }

        //取得五色加分
        public int FiveColorPoint
        {
            get
            {

                if (DiffColorBuilding == 5)
                    return 3;

                return 0;
            }

        }

        //return the list of cards that the currentPlayer can build
        public List<AbstractCard> CanBuildList
        {
            get { return (from c in HandCards where c.CanBuild(this) select c).ToList<AbstractCard>(); }
        }

        /// <summary>
        /// 此玩家目前威脅程度
        /// </summary>
        public Dictionary<string, int> Threat
        {
            get
            {
                Dictionary<string, int> temp = new Dictionary<string, int>();

                int moneyThreat = 20 * this.Money;

                int buildingThreat = this.Buildings.Count > 4 ? 100 : (100 * this.Buildings.Count) / (9 - this.Buildings.Count);

                int pointThreat = CalculatePoints() * 4;

                int handCardThreat = (this.HandCards.Count >= 3 && this.Buildings.Count >= 4) ? 100 + (this.HandCards.Count) * 20 : (this.HandCards.Count) * 20;

                int taxThreat = 0;
                //Todo: calculate tax threat  , according to the count of building color 
                //var obj = (from bd in Buildings group bd by bd.CardColor.ToString() into g select new { a = g.Key, b = g }).Max(m => m.b);
                //if (obj!=null)
                //    taxThreat = (obj / 2) * 100;

                int total = buildingThreat + pointThreat + handCardThreat + taxThreat;


                temp.Add("moneyThreat", moneyThreat);
                temp.Add("buildingThreat", buildingThreat);
                temp.Add("pointThreat", pointThreat);
                temp.Add("taxThreat", taxThreat);
                temp.Add("total", total);



                return temp;
            }
        }

        #endregion
        
        #region Methods
        //check if currentPlayer can build the input card!
        public bool CanBuild(AbstractCard card)
        {
            if (card.CanBuild(this))
                return true;
            return false;
        }

        public void DoBuilding()
        {

            for (int i = 0; i < this.ToBuildNumber;  )
            {

                //1.顯示手牌給user 選 , 等待Player做出決定
                AbstractCard[] chooseCard = this.ChooseCard.Choose("請選擇想要的建築牌卡", 1, this.CanBuildList);

                //選擇不蓋
                if (chooseCard == null || chooseCard.Length == 0)
                {
                    break;
                }

                //是wizard 也許可以少1 $
                if (CurrentChar.Name == "Wizard"
                    && CurrentChar.IsSpecialFuncUsed
                    && chooseCard[0] != GameSystem.WizardDrawCard)
                {
                    int ans = this.ChooseYesOrNo.Choose("是否要折底剛剛的抽卡，節省一塊＄建築費用？", TwoChoiceEnum.Yes_No);
                    switch (ans)
                    {
                        case 0: //yes
                            this.HandCards.Remove(GameSystem.WizardDrawCard);
                            break;
                        case 1: //no 
                            DiscountPoint = 0;
                            break;
                        case -1: //cancel
                            DiscountPoint = 0;
                            break;
                    }
                }

                //2.選擇的牌可以蓋
                if (chooseCard[0].CanBuild(this))
                {
                                    
                    //3.move the abstractCard from  "HandCard" to "Buildings"
                    this.HandCards.Remove(chooseCard[0]);
                    //4.付錢    
                    chooseCard[0].DoBuilding(this, chooseCard[0].Point - DiscountPoint + ExtraBuildPoint);

                    //TexCollector 可多拿1$
                    if (ExtraBuildPoint !=0)
                    {
                        GameSystem.Characters[1].CurrentPlayer.Money += ExtraBuildPoint;
                    }

                    //Alchemist 可拿回蓋房子的＄
                    if (FreeBuilt)
                    {
                        this.Money += (chooseCard[0].Point - DiscountPoint);
                    }


                    //5.check if the player is the first one to end the game
                    //got 8 building
                    if (Buildings.Count >= 8)
                    {
                        //building can't be destructed!
                        this.IsAbleToDestruct = false;

                        //the round will be over
                        
                        GameSystem.IsGameOver = true;
                        //the first one to get it.
                        if (!GameSystem.IsFirstEnd)
                        {
                            ExtraEndPoint = 4;
                            GameSystem.IsFirstEnd = true;
                        }
                        else
                        {
                            ExtraEndPoint = (ExtraEndPoint > 2) ? ExtraEndPoint : 2;
                        }
                    }
                    i++;
                    GameSystem.AddSystemMessage(this+" 蓋了建築：" + chooseCard[0]);
                    IsBuilt = true;
                }
            }
            
            GameSystem.AddSystemMessage(this+" 目前分數"+this.CalculatePoints());
        }

        public int CalculatePoints()
        {
            int total = 0;
            foreach(Card.AbstractCard c in Buildings)
            {
                total += c.Point;
            }

            return total + FiveColorPoint + ExtraEndPoint;

        }

        //玩家抽牌，不放回
        public void DrawCardAll(int number)
        {
            
            for (int i = 0; i < number; i++)
            {
                if (GameSystem.CardStack.Count == 0)
                {
                    GameSystem.AddSystemMessage("牌堆被抽完了");
                    return;
                }

                this.HandCards.Add(GameSystem.CardStack.Last());
                GameSystem.CardStack.RemoveLast();
            }
        }

        public virtual void DrawCardPick(int n, int m)
        {

            //1.拿出 total 張牌給user 選
            List<AbstractCard> cards = new List<AbstractCard>();
            for (int i = 0; i < n; i++)
            {
                if (GameSystem.CardStack.Count == 0)
                {
                    GameSystem.AddSystemMessage("牌堆被抽完了");
                    return;
                }
                cards.Add(GameSystem.CardStack.Last());
                GameSystem.CardStack.RemoveLast();
            }
            //2.等待Player做出決定

            AbstractCard[] chooseCard = this.ChooseCard.Choose("請選擇想要的手牌", m, cards);
            
            //3.將Player選取的牌加入該Player手牌
            this.HandCards.AddRange(chooseCard);

            //4.將剩餘的牌放入牌堆底部
            foreach (AbstractCard c in cards.Except(chooseCard))
                GameSystem.CardStack.AddFirst(c);
            
        }

        internal void StartNewRound(Character.Character myCharacter)
        {
            //可保留的新牌數目(可能增加為2)
            //KeepNewCard = 1;
            
            DiscountPoint = 0;

            IsBuilt = false;

            IsAbleToDestruct = true;

            ExtraBuildPoint = 0;

            ToBuildNumber=1;

            FreeBuilt = false;

            //Reset purple card function
            foreach(PurpleCard purple in Buildings.Where(p=>p.CardColor == Color.Purple))
            {
                purple.Reset();
            }

            //設定 'player' and 'character'連結
            myCharacter.CurrentPlayer = this;
            myCharacter.CurrentState = StateEnum.Selected;
            this.CurrentChar = myCharacter;



            //設定 選項＆操作 character  方式

             myCharacter.ChooseAction = this.ChooseAction;

            //MyMessage.Enqueue("選取角色:"+myCharacter);
        }


        public void RefreshPlayerInfo() 
        {
            OnPropertyChanged("HandCardCount");
            OnPropertyChanged("ImgCrown");
            OnPropertyChanged("ImgFiveColor");

            OnPropertyChanged("ImgBuilding0");
            OnPropertyChanged("ImgBuilding1");
            OnPropertyChanged("ImgBuilding2");
            OnPropertyChanged("ImgBuilding3");
            OnPropertyChanged("ImgBuilding4");
            OnPropertyChanged("ImgBuilding5");
            OnPropertyChanged("ImgBuilding6");
            OnPropertyChanged("ImgBuilding7");

        }

        #endregion

        #region Override Method

        public override string ToString()
        {
            return this.Name;
        }

        public override bool Equals(object obj)
        {
            return this.ToString().Equals(obj.ToString());
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(
                    this, new PropertyChangedEventArgs(propName));
        }

        #endregion
    }


}