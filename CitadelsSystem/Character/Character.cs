using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Drawing;
using System.Windows.Media.Imaging;
using CitadelsSystem.Card;
using CitadelsSystem.Interface;
using CitadelsSystem.Players;
using CitadelsSystem.Utils;

namespace CitadelsSystem.Character
{
    public class Character
    {

        #region Fields

        private BitmapSource headImg;
        public BitmapSource HeadImg
        {
            get { return headImg; }
            set { headImg = value; }
        }

        public string Name;
        public string Desc;
        public int    Id; 
        public Color  TaxColor;

        public bool IsSpecialFuncUsed =false;
        public bool IsKilled = false;
        public bool IsStolen = false;
        public bool IsTaxed = false;
        public bool IsEnd = false;
        public bool IsCardOrMoneyTaken = false;
        public bool IsBuilt 
        {
            get
            {
                return CurrentPlayer.IsBuilt;
                
            }
            
        }

        public IChooseAction ChooseAction ;

        /// <summary>
        /// 目前操作此角色的玩家
        /// </summary>
        public AbstractPlayer CurrentPlayer;

        public string Function;
        public string SelectType;
        public StateEnum CurrentState;
        public ExcuteEnum ExcuteTiming;

        private CharacterSpecialFunction SpecialFunction = new CharacterSpecialFunction();


        #endregion

        //Print the tax message if success
        public int Tax()
        {
            
            int total = 0;

            if (IsTaxed == false)
            {
                IsTaxed = true;
                foreach (Card.AbstractCard c in CurrentPlayer.Buildings)
                {
                    //Gold is anyColor 
                    //Color.FromName()
                    if (c.CardColor == Color.Gold || c.CardColor == TaxColor)
                    {
                        total++;
                    }
                }
                if (total > 0)
                {
                    string str = string.Format(@"玩家{0}徵稅{1}$", CurrentPlayer.Name, total);
                    GameSystem.AddSystemMessage(str);
                }

                
            }

            return total;
        }


        public int TryTax()
        {
            int total = 0;
            foreach (Card.AbstractCard c in CurrentPlayer.Buildings)
            {
                //Gold is anyColor 
                //Color.FromName()
                if (c.CardColor == Color.Gold || c.CardColor == TaxColor)
                {
                    total++;
                }
            }
            return total;
        }

        /// <summary>
        /// reset all parameter when starting a new  round
        /// </summary>
        public void StartNewRound()
        {
            CurrentState = StateEnum.NotSelected;
            CurrentPlayer = null;
            IsSpecialFuncUsed =false;
            IsKilled = false;
            IsStolen = false;
            IsTaxed = false;
            IsEnd = false;
            IsCardOrMoneyTaken = false;
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            return this.ToString().Equals(obj.ToString());
        }

        //Perform the special activity
        public void DoAction()
        {
            if (!IsSpecialFuncUsed)
            {
                //try to invoke the Character Functin by Function name !
                Type actionType = typeof (CharacterSpecialFunction);
                MethodInfo method = actionType.GetMethod(Function);
                Character temp= null;
                switch (SelectType)
                {
                    case "Player":
                        {
                            if (Function == "Magician" && CurrentPlayer is RealPlayer)
                            {
                                string str = string.Format("是否要和牌庫換牌?\r\n(剩餘{0}張)", GameSystem.CardStack.Count);
                                int result = CurrentPlayer.ChooseYesOrNo.Choose(str, TwoChoiceEnum.Yes_No);

                                if (result == 0)//換
                                {
                                    //1.選擇手牌
                                    AbstractCard[] cards = CurrentPlayer.ChooseCard.Choose("請點選不要的手牌...",
                                                                    CurrentPlayer.HandCardCount, CurrentPlayer.HandCards);
                                    foreach(AbstractCard c in cards)
                                    {
                                        CurrentPlayer.HandCards.Remove(c);
                                    }
                                    //2.從牌堆抽牌
                                    CurrentPlayer.DrawCardAll(cards.Length);
                                    return;
                                }
                            }
                        

                        //選擇目標Player當參數

                            List<AbstractPlayer> playerList = GameSystem.Players.Where(p => p.ToString() != this.CurrentPlayer.Name).ToList();
                            temp = this.CurrentPlayer.ChooseTarget.Choose(playerList).CurrentChar;
                            try
                            {
                                method.Invoke(SpecialFunction, new object[] {this, temp});
                            }
                            catch(Exception e)
                            {
                                int i = 0;
                                i++;
                            }

                            break;
                        }
                    case "Char":
                        {
                            
                            //選擇目標Char當參數 
                            List<AbstractPlayer> playerList = GameSystem.Players.Where(p => (p.CurrentChar.Id!=1 && p.Name!=GameSystem.CurrentPlayer.Name && !p.CurrentChar.IsKilled)).OrderBy(p=>p.CurrentChar.Id).ToList();
                            temp = this.CurrentPlayer.ChooseTarget.Choose(playerList).CurrentChar;
                            

                            method.Invoke(SpecialFunction, new object[] {this, temp});
                            break;
                        }
                    case "None":
                        {
                            try
                            {
                                //不需參數
                                method.Invoke(SpecialFunction, new object[] {this, null});
                            }
                            catch(Exception ex)
                            {
                                Console.WriteLine(ex);
                            }
                            break;
                        }


                    default:
                        {
                            throw new Exception("DoAction parameter failure!");
                            break;
                        }
                }
            }

        }
    }



    

    
}