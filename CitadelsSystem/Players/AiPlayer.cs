using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using CitadelsSystem.Card;
using CitadelsSystem.Interface;
using CitadelsSystem.Utils;

namespace CitadelsSystem.Players
{
    public class AiPlayer:AbstractPlayer
    {
        
        public List<string> Words;

        public AiPlayer(string name)
        {
            this.Name = name;
            this.ChooseCard = new ChooseCardAI();
            this.ChooseCharacter = new ChooseCharacterAI();
            this.ChooseTarget = new ChooseTargetAI();
            this.ChooseBuilding = new ChooseBuildingAI();
            this.ChooseYesOrNo = new ChooseYesOrNoAI();
            this.ChooseAction = new ChooseActionAI();
        }

        public override void DrawCardPick(int n, int m)
        {

            //1.拿出 total 張牌 選
            List<AbstractCard> cards = new List<AbstractCard>();

            for (int i = 0; i < n; )
            {
                if (GameSystem.CardStack.Count == 0)
                {
                    GameSystem.AddSystemMessage("牌堆被抽完了");
                    return;
                }

                AbstractCard temp = GameSystem.CardStack.Last();
                GameSystem.CardStack.RemoveLast(); 

                if ( (Buildings.Contains(temp) || HandCards.Contains(temp))&& GameSystem.RandomNum.NextDouble()<0.5 )
                {
                    GameSystem.CardStack.AddFirst(temp);
                    continue;
                }
                cards.Add(temp);
                i++;
            }
            //2.等待Player做出決定
            AbstractCard[] chooseCard = this.ChooseCard.Choose("抽牌", m, cards);

            //3.將Player選取的牌加入該Player手牌
            this.HandCards.AddRange(chooseCard);

            //4.將剩餘的牌放入牌堆底部
            foreach (AbstractCard c in cards.Except(chooseCard))
                GameSystem.CardStack.AddFirst(c);
        }
    }

    public class ChooseTargetAI : IChooseTarget
    {
        public AbstractPlayer Choose(List<AbstractPlayer> input)
        {

            double val = GameSystem.RandomNum.NextDouble();
            AbstractPlayer result = null;

            #region Normal situation: select a target player
            //0~32 :30%
            if (val < 0.33)
            {
                AbstractPlayer[] temp =
                    (from p in input
                     orderby p.Threat["total"] descending
                     select p).ToArray();
                result = temp[(int)(GameSystem.RandomNum.NextDouble()*2)];
            }
            //33~64 :30%
            else if(val < 0.65)
            {
                AbstractPlayer[] temp =
                    (from p in input
                     orderby (p.Threat["pointThreat"] + p.Threat["pointThreat"]) descending
                     select p).ToArray();
                result = temp[(int)(GameSystem.RandomNum.NextDouble() * 2)];
            }
            //65~85 : 20%
            else if (val < 0.95)
            {

                result = input[(int)(GameSystem.RandomNum.NextDouble() * input.Count)];                

            }
            //15%
            else
            {
                //指定真正玩家
                if (input.Contains(GameSystem.TheRealPlayer))
                    result = GameSystem.TheRealPlayer;
                else
                    result = input[(int)(GameSystem.RandomNum.NextDouble() * input.Count)];
            }
            #endregion

            return result;
        }
    }

    public class ChooseCharacterAI : IChooseCharacter
    {
        //choose the character for this round
        public Character.Character Choose(List<Character.Character> input)
        {
            return input[(int)(GameSystem.RandomNum.NextDouble()*123)% (input.Count)];
        }
    }

    public class ChooseCardAI : IChooseCard
    {
        public AbstractCard[] Choose(string title, int m, List<AbstractCard> cards)
        {
            int n = cards.Count;

            if (n == 0)
                return null;

            Character.Character c = GameSystem.CurrentCharacter;
            AbstractPlayer p = c.CurrentPlayer;
            

            List<AbstractCard> candi = new List<AbstractCard>();
            

            foreach (AbstractCard card in (from card in cards orderby card.Point descending select card))
            {
                if (candi.Count == m)
                    break;

                //拿了可以蓋
                if (p.Money + c.TryTax() - card.Point > 0)
                {
                    candi.Add(card);
                    continue;
                }
                //紫色卡片
                if (card.CardColor == Color.Purple)
                {
                    candi.Add(card);
                    continue;
                }

                if(GameSystem.RandomNum.NextDouble()>0.5)
                {
                    candi.Add(card);
                    continue;
                }


            }



            return candi.ToArray();
            

        }
    }
     
    public class ChooseBuildingAI : IChooseBuilding
    {
        #region IChooseBuilding Members

        //select the target building to destruct
        public AbstractCard ChooseBuilding_WarLord(List<AbstractCard> buildings)
        {

            double val = GameSystem.RandomNum.NextDouble();
            AbstractCard temp = null;
            //價格高者
            if(val < 0.33)
            {
                temp = buildings.OrderByDescending(b => b.DestructionPoint).ToArray()[0];
            }
            //價格低者
            else if (val < 0.66)
            {
                temp = buildings.OrderBy(b => b.DestructionPoint).ToArray()[0];
            }
            //隨便選
            else
            {
                temp = buildings[ (int)(val * 100) % buildings.Count];
            }
            return temp;
        }

        //select the target building and own building to exchange
        //result[0], result[1] from self and target building
        public AbstractCard[] ChooseBuilding_Diplomat(List<AbstractCard> selfBuilding, List<AbstractCard> targetBuilding)
        {
            AbstractCard[] result = new AbstractCard[2];

            targetBuilding = targetBuilding.OrderByDescending(t => t.RealPoint).ToList();
            //選擇目標的最高分建築
            result[1] = targetBuilding[0];

            int money = GameSystem.CurrentCharacter.CurrentPlayer.Money;
            //選擇自己可以交換的最低分建築
            result[0] = selfBuilding.Where(sb => money + sb.RealPoint >= targetBuilding[0].RealPoint).OrderBy(o => o.RealPoint).ToArray()[0];
            
            return result;
        }

        #endregion
    }

    public class ChooseYesOrNoAI: IChooseYesOrNo
    {

        #region IChooseYesOrNo Members
        
        public int Choose(string title, TwoChoiceEnum choice)
        {
            int result = (int)GameSystem.RandomNum.NextDouble()*2;
            switch(choice)
            {
                case TwoChoiceEnum.Card_Money:
                    break;
                case TwoChoiceEnum.Yes_No:
                    break;
                default:
                    throw new Exception("Parameter not found!");
            }

            return result;
        }

        #endregion
    }

    public class ChooseActionAI : IChooseAction
    {
        Character.Character c;
        List<int> choiceList;

        #region IChooseAction Members
        // 0:End ,1:TakeMoney ,2:TakeCard ,3:Build ,4:Tax, 5:InvokeFunction 6: purple card
        public int ChooseActionList()
        {

            if (BasicRule() == 5)
                return 5;

            //todo:purple card implement

            //可蓋3
            if (choiceList.Contains(3))
                return 3;

            if (choiceList.Contains(1))
            {
                //2:TakeCard
                if (c.CurrentPlayer.Money > 5 || c.CurrentPlayer.HandCards.Count < 1)
                    return 2;
                //1:TakeMoney
                return 1;

            }

            return choiceList[choiceList.Count - 1];

        }

        protected int BasicRule()
        {

            c = GameSystem.CurrentCharacter;
            choiceList = new List<int>();

            #region Create Select list
            //0:end
            choiceList.Add(0);

            //1:TakeMoney ,2:TakeCard
            if (!c.IsCardOrMoneyTaken)
            {
                choiceList.Add(1);
                choiceList.Add(2);
            }

            //3:Build , 
            if (!c.IsBuilt)
            {
                if (c.CurrentPlayer.CanBuildList.Count > 0)
                {
                    choiceList.Add(3);
                }
            }
            //4:Tax , 白色不能徵稅
            if (!c.IsTaxed && c.TaxColor != Color.White)
            {
                choiceList.Add(4);
            }
            //5:InvokeFunction
            if (!c.IsSpecialFuncUsed)
            {
                choiceList.Add(5);
            }

            //6. 紫色卡片可以使用
            var purpleList = c.CurrentPlayer.Buildings.Where(b => b is PurpleCard).Select(p => (PurpleCard)p).ToList();
            if (purpleList.Where(p => p.Excutable && !p.IsUsed).Count() > 0)
                choiceList.Add(6);

            #endregion

            //1.角色是自動執行 2.角色是皇帝
            if ((choiceList.Contains(5) && c.ExcuteTiming == ExcuteEnum.AtBegining)
                || c.Name == "Emperor")
                return 5;

            return 0;
        }

        #endregion

        #region IChooseAction Members


        public void Reset()
        {
                
        }

        #endregion
    } 
}