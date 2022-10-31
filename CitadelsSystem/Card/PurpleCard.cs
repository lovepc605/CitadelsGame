using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using CitadelsSystem.Players;
using CitadelsSystem.Utils;

namespace CitadelsSystem.Card
{
    public class PurpleCard:AbstractCard
    {
        /// <summary>
        /// 被動or主動發動
        /// </summary>
        public bool Excutable  ;

        /// <summary>
        /// 是否使用過了
        /// </summary>
        public bool IsUsed;

        /// <summary>
        /// 呼叫功能
        /// </summary>
        public string Function;

        /// <summary>
        /// 使用時機
        /// </summary>
        public PurpleCardAcviteEnum ActiveTiming = PurpleCardAcviteEnum.None;



        /*
        ExtraPointPlusTwo
        TaxAnyColor_Gold
        AbandomChange
        DrawTwoCard
        ChangeBuildingColor_Silver
        ExtraDestructionPoint
        RegainDistructedCard
        DrawThreeKeepOneCard
        ExtraPointPlusTwo
        CantBeDestructed
        PayThreeGoldDrawTwoCard

         */


        public void Reset()
        {
            IsUsed = false;

        }

        public void Invoke(Players.AbstractPlayer player)
        {
            //Todo: implement the purple card funciton
            switch (this.Function)
            { 
                case "ExtraPointPlusTwo":
                    {
                        this.ExtraPoint = 2;
                        GameSystem.AddSystemMessage(string.Format("紫卡[{0}]：分數+2", this.Name));
                    }
                    break ;
                           
                case "TaxAnyColor_Gold":
                    {
                        this.CardColor = Color.Gold;
                        GameSystem.AddSystemMessage(string.Format("紫卡[{0}]：可對『{0}』進行徵稅", this.Name));
                    }
                    break ;
                    
                case "AbandomChange":
                    //discard one card and get 1$
                    {
                        if(player.HandCardCount>0)
                        {
                            AbstractCard c = player.ChooseCard.Choose("請選擇不要的卡...", 1, player.HandCards)[0];
                            player.HandCards.Remove(c);
                            player.Money++;

                            GameSystem.AddSystemMessage(string.Format("紫卡[{0}]：捨棄『{1}』並獲得1$",this.Name, c.Name ));
                            VoiceHelper.AllPlayerSound("Coins", 100);
                        }
                    }
                    break ;
                    
                case "DrawTwoCard":
                    {
                        player.KeepNewCard = 2;
                        GameSystem.AddSystemMessage(string.Format("紫卡[{0}]：抽牌可保留兩張", this.Name ));
                    }
                    break ;
                    
                case "ChangeBuildingColor_Silver":
                    {
                        this.CardColor = Color.Silver;
                        GameSystem.AddSystemMessage(string.Format("紫卡[{0}]：最後算分可當任意顏色", this.Name));
                    }
                    break ;
                    
                case "ExtraDestructionPoint":
                    foreach (AbstractCard card in player.Buildings)
                    {
                        card.ExtraDestructionPoint = 1;
                        GameSystem.AddSystemMessage(string.Format("紫卡[{0}]：拆除{1}的費用 +1$", Name ,player.Name));
                    }
                    break ;
                    
                case "RegainDistructedCard":
                    if (player.Money > 1 && GameSystem.JunkStack.Count > 0)
                    {
                        player.HandCards.Add(GameSystem.JunkStack.Last());
                        GameSystem.JunkStack.RemoveLast();
                        GameSystem.AddSystemMessage(string.Format("紫卡[{0}]：可抽棄牌", Name, player.Name));
                    }
                    break ;
                    
                case "DrawThreeKeepOneCard":
                    {
                        player.DrawNewCard = 3;
                        GameSystem.AddSystemMessage(string.Format("紫卡[{0}]：抽牌時可抽三張保留一張", Name));
                    }
                    break ;
                    
                case "CantBeDestructed":
                    {
                        this.IsDestructible = false;
                        GameSystem.AddSystemMessage(string.Format("紫卡[{0}]：此牌不可被拆", Name));
                    }
                    break ;

                case "PayThreeGoldDrawTwoCard":
                    if (player.Money >= 3 && GameSystem.CardStack.Count >= 2)
                    {
                        player.Money -= 3;
                        player.DrawCardPick(2, 2);
                        GameSystem.AddSystemMessage(string.Format("紫卡[{0}]：付三＄抽兩張牌", Name));
                    }
                    break ;
                    
                default:
                    throw new Exception("Can't find the method " + this.Function);
                    break;

            }
            IsUsed = true;
        }

        public override void DoBuilding(AbstractPlayer player ,int cost)
        {
            base.DoBuilding(player ,cost);
            GameSystem.AddSystemMessage(string.Format("『{0}』擁有建築物『{1}』並可使用特殊功能", player.Name, this.Name));
            
            //自動執行的 purple card
            if (!Excutable)
                Invoke(player);
        }

        public override void RemoveBuilding(AbstractPlayer player)
        {

            GameSystem.AddSystemMessage(string.Format("『{0}』的紫色建築物『{1}』被移除了;特殊的功能也將被取消!", player.Name, this.Name));
            player.Buildings.Remove(this);

            switch (this.Function)
            {
                case "ExtraPointPlusTwo":
                    break;

                case "TaxAnyColor_Gold":
                    break;

                case "AbandomChange":
                    break;

                case "DrawTwoCard":
                    player.KeepNewCard = 1;
                    break;

                case "ChangeBuildingColor_Silver":
                    break;

                case "ExtraDestructionPoint":
                    foreach (AbstractCard card in player.Buildings)
                    {
                        card.ExtraDestructionPoint = 0;
                    }
                    break;

                case "RegainDistructedCard":
                    break;

                case "DrawThreeKeepOneCard":
                    break;

                case "CantBeDestructed":
                    break;

                case "PayThreeGoldDrawTwoCard":
                    break;

                default:
                    throw new Exception("Can't find the method " + this.Function);
                    break;

            }
        }



    }


}