using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CitadelsSystem.Players;
using CitadelsSystem.Card;
using CitadelsSystem.Utils;

namespace CitadelsSystem.Character
{
    public class CharacterSpecialFunction
    {
        #region Standard

        //指定角色殺人
        public void Assassin(Character player, Character target)
        {

            if (target.Id == 1)
                return;

            target.IsKilled = true ;
            string msg = string.Format("刺客：『{0}』要殺『{1}』",player.CurrentPlayer.Name,target.Name);
            VoiceHelper.AllPlayerSound("Gun", 1000);
            GameSystem.AddSystemMessage(msg);
            player.IsSpecialFuncUsed = true;
        }
        
        //指定角色偷＄
        public void Thief(Character player, Character target)
        {
            //if select char id != 1
            if (target.Id == 1 || target.Id == 2)
                return;

            target.IsStolen = true;
            string msg = string.Format("盜賊：『{0}』要偷『{1}』",player.CurrentPlayer.Name,target.Name);
            VoiceHelper.AllPlayerSound("YA", 1000);
            GameSystem.AddSystemMessage(msg);
            player.IsSpecialFuncUsed = true;
        }

        //指定人物換牌
        public void Magician(Character player, Character target)
        {
            if (player.CurrentPlayer is AiPlayer)
            {
                //手牌要換多
                if (player.CurrentPlayer.HandCards.Count > target.CurrentPlayer.HandCards.Count)
                    return;
            }


            var obj = player.CurrentPlayer.HandCards;
            player.CurrentPlayer.HandCards = target.CurrentPlayer.HandCards;
            target.CurrentPlayer.HandCards = obj;
            string msg = string.Format("魔法師：『{0}』要和『{1}』交換手牌", player.CurrentPlayer.Name, target.CurrentPlayer.Name);
            VoiceHelper.AllPlayerSound("No_1", 1500);
            GameSystem.AddSystemMessage(msg);
            player.IsSpecialFuncUsed = true;

        }

        //下次起使玩家
        public void King(Character player, Character target)
        {
            GameSystem.TheStartPlayer = player.CurrentPlayer;
            string msg = string.Format("國王：『{0}』擁有皇冠", player.CurrentPlayer.Name);
            GameSystem.AddSystemMessage(msg);
            player.IsSpecialFuncUsed = true;
        }

        //建物不可被拆
        public void Bishop(Character player, Character target)
        {
            player.CurrentPlayer.IsAbleToDestruct = false;
            string msg = string.Format("主教：『{0}』的建築物這回合不可拆", player.CurrentPlayer.Name);
            GameSystem.AddSystemMessage(msg);
            player.IsSpecialFuncUsed = true;
        }
        
        // ＄+1
        public void Merchant(Character player, Character target)
        {
            player.CurrentPlayer.Money += 1;
            string msg = string.Format("商人：『{0}』這回合多拿1＄", player.CurrentPlayer.Name);
            VoiceHelper.AllPlayerSound("Coins", 100);
            GameSystem.AddSystemMessage(msg);
            player.IsSpecialFuncUsed = true;
        }
        
        //多拿兩張牌 & 可蓋最多3個建物
        public void Architect(Character player, Character target)
        {
            player.CurrentPlayer.DrawCardAll(2);
            player.CurrentPlayer.ToBuildNumber = 3;
            VoiceHelper.AllPlayerSound("DealCard", 100);
            string msg = string.Format("建築師：『{0}』這回合多拿兩張牌，且最多蓋三棟建築", player.CurrentPlayer.Name);         
            GameSystem.AddSystemMessage(msg);
            player.IsSpecialFuncUsed = true;
        }

        //可拆除別人的建物
        public void Warlord(Character player, Character target)
        {
             
            //判斷能不能拆  金額&狀態!
            List<AbstractCard> buildings = WarLordCheck(player.CurrentPlayer, target.CurrentPlayer);
            if (buildings.Count == 0)
            {
                if (player.CurrentPlayer is RealPlayer)
                    GameSystem.AddSystemMessage("無法拆除此玩家的建築物");
                return;
            }
                
            AbstractCard building = player.CurrentPlayer.ChooseBuilding.ChooseBuilding_WarLord(buildings);
            if (building == null)
                return;

            player.CurrentPlayer.Money -= building.DestructionPoint;
            
            //invoke the remove function 
            building.RemoveBuilding(target.CurrentPlayer);

            VoiceHelper.AllPlayerSound("Destruction", 2000);
            string msg = string.Format("領主：『{0}』的建築物{1}將被拆除", target.CurrentPlayer.Name, building);
            GameSystem.AddSystemMessage(msg);
            player.IsSpecialFuncUsed = true;
        }

        //return the list of buildings which can be destructed!
        private List<AbstractCard> WarLordCheck(AbstractPlayer player, AbstractPlayer target)
        {
            List<AbstractCard> result = new List<AbstractCard>();

            //主教or八棟
            if (!target.IsAbleToDestruct)
                return result;

            //夠＄可拆
            result = (from b in target.Buildings
                      where (b.IsDestructible && b.DestructionPoint <= player.Money)
                      select b).ToList();


            return result;
        }

        #endregion

        #region Expansion
        
        //坐在國王隔壁+3 , ID==9
        public void Queen(Character player, Character target)
        {
			//操縱King之Player 的index 是否在目前玩家的左右兩邊，是的話直接 money +3
            int preIndex, postIndex;
            int currentIndex = GameSystem.Players.IndexOf(player.CurrentPlayer);
            preIndex  = (currentIndex - 1 + GameSystem.Players.Count)%GameSystem.Players.Count;
            postIndex = (currentIndex + 1) % GameSystem.Players.Count;

            int extraMoney = 0;
            if (GameSystem.Players[preIndex].CurrentChar.Id ==4 ||
                GameSystem.Players[postIndex].CurrentChar.Id == 4
                )
            {
                extraMoney = 3;
            }
            player.CurrentPlayer.Money += extraMoney;
            VoiceHelper.AllPlayerSound("Coins", 100);
            string msg = string.Format("王后：『{0}』在這回合多取得{1}枚金幣", player.CurrentPlayer.Name, extraMoney);
            GameSystem.AddSystemMessage(msg);
            player.IsSpecialFuncUsed = true;
        }

        //角色互換
        public void Witch(Character player, Character target)
        {
            if (target.Id == 1)
                return;

            player.CurrentPlayer.StartNewRound(target);

            string msg = string.Format("女巫：『{0}』這回合決定和『{1}』交換", player.CurrentPlayer.Name,target.Name);
            VoiceHelper.AllPlayerSound("Problem", 1000);
            GameSystem.AddSystemMessage(msg);


            GameSystem.MakeChoice(GameSystem.CurrentPlayer, 0);
        }

        //蓋房子要給此角色金幣
        public void TaxCollector(Character player, Character target)
        {
            //make use of ExtraBuildPoint in AbstractCard
            var pList = from p in GameSystem.Players
                        where p.Name != player.CurrentPlayer.Name
                        select p;
            
            foreach(AbstractPlayer p in pList)
            {
                p.ExtraBuildPoint = 1;
            }
            VoiceHelper.AllPlayerSound("Coins", 100);
            string msg = string.Format("稅務官：這回合蓋房子的人都要多付1$給『{0}』", player.CurrentPlayer.Name);
            GameSystem.AddSystemMessage(msg);

            player.IsSpecialFuncUsed = true;
        }

        //看牌 並且抽一張牌 ; 可以蓋重複
        public void Wizard(Character player, Character target)
        {
            if (target.CurrentPlayer.HandCards.Count == 0)
                return;

            AbstractCard card = player.CurrentPlayer.ChooseCard.Choose("請選擇想要的建築牌卡", 1,
                                                   target.CurrentPlayer.HandCards)[0];

            //特殊抽牌 可以拿來抵銷建築費用!
            GameSystem.WizardDrawCard = card;
            player.CurrentPlayer.DiscountPoint = 1;

            player.CurrentPlayer.HandCards.Add(card);
            target.CurrentPlayer.HandCards.Remove(card);

            player.IsSpecialFuncUsed = true;
            VoiceHelper.AllPlayerSound("Thank", 100);
            string msg = string.Format("巫師：『{0}』抽走『{1}』一張牌", player.CurrentPlayer.Name, target.Name);
            GameSystem.AddSystemMessage(msg);
        }

        //指定下次開始玩家
        public void Emperor(Character player, Character target)
        {
            GameSystem.TheStartPlayer = target.CurrentPlayer;
            player.IsSpecialFuncUsed = true;


            string msg = string.Format("帝王：『{0}』下回合決定讓『{1}』先發", player.CurrentPlayer.Name, target.CurrentPlayer.Name);
            GameSystem.AddSystemMessage(msg);
        }

        //從最多＄的Player拿一元
        public void Abbot(Character player, Character target)
        {
            //1.找出最多＄的人
            int maxMoney = GameSystem.Players.Max(m => m.Money);
            var players = from p in GameSystem.Players where p.Money == maxMoney select p;
            int count = 0;
            //2.扣他們的＄ ＆  加入自己口袋
            foreach(AbstractPlayer p in players)
            {
                if(p.Money>0)
                {
                    --p.Money;
                    ++count;
                    string msg = string.Format("修道院長：『{0}』從{1}拿走一枚金幣", player.CurrentPlayer.Name, p.Name);
                    GameSystem.AddSystemMessage(msg);
                    VoiceHelper.AllPlayerSound("Coins", 100);
                }
            }
            player.CurrentPlayer.Money += count;

            
            player.IsSpecialFuncUsed = true;

        }

        //把蓋房子的＄拿回來
        public void Alchemist(Character player, Character target)
        {
            player.CurrentPlayer.FreeBuilt = true;
            player.IsSpecialFuncUsed = true;

            string msg = string.Format("煉金術師：『{0}』這回合可以拿回蓋房子的＄", player.CurrentPlayer.Name);
            GameSystem.AddSystemMessage(msg);
        }

        //拿4＄ or 4張牌 , 但不能蓋房子
        public void Navigator(Character player, Character target)
        {
			//show , Choose 'Money' or 'Card'
            //end this round
            int result =player.CurrentPlayer.ChooseYesOrNo.Choose("選擇拿4卡片/4枚金幣", TwoChoiceEnum.Card_Money);
            string take = "";
            switch (result)
            {
                case 0:
                    {
                        take = "4張卡片";
                        player.CurrentPlayer.DrawCardAll(4);
                        VoiceHelper.AllPlayerSound("DealCard", 100);
                    }
                    break;
                case 1:
                    {
                        take = "4枚金幣";
                        player.CurrentPlayer.Money += 4;
                        VoiceHelper.AllPlayerSound("Conis", 100);
                    }
                    break;
                case -1: //-1
                    return;
            }

            player.CurrentPlayer.IsBuilt = true;
            player.IsSpecialFuncUsed = true;
            player.IsCardOrMoneyTaken = true;


            string msg = string.Format("航海家：『{0}』這回合拿了{1}", player.CurrentPlayer.Name,take);
            GameSystem.AddSystemMessage(msg);
            GameSystem.MakeChoice(GameSystem.CurrentPlayer, 0);

        }
        
        //交換建物　　並付差額
        public void Diplomat(Character player, Character target)
        {
            //1.選擇 Target 的  Building ,
            //2.選擇    自己的  Building
            //3.交換 ，並支付差額
            List<AbstractCard> targetBuilding, selfBuilding;
            bool canChange = DiplomatCheck(player.CurrentPlayer, target.CurrentPlayer , out selfBuilding ,out targetBuilding);
            if ( ! canChange)
            {
                if (player.CurrentPlayer is RealPlayer)
                    GameSystem.AddSystemMessage("無法交換此玩家的建築物");
                return;
            }

            AbstractCard[] result = player.CurrentPlayer.ChooseBuilding.ChooseBuilding_Diplomat(selfBuilding, targetBuilding);
            if (result==null || result.Length!=2 || result[0]==null || result[1]==null)
                return;


            int diffMoney = result[1].RealPoint - result[0].RealPoint;
            
            //invoke the remove function 
            result[0].RemoveBuilding(player.CurrentPlayer);
            result[1].RemoveBuilding(target.CurrentPlayer);

            result[1].DoBuilding(player.CurrentPlayer,diffMoney > 0 ? diffMoney : 0);
            result[0].DoBuilding(target.CurrentPlayer,diffMoney > 0 ? -diffMoney : 0);
            VoiceHelper.AllPlayerSound("Thank", 1000);
            string msg = string.Format("外交官：『{0}』利用{1}交換{2}", player.CurrentPlayer.Name, result[0], result[1]);
            GameSystem.AddSystemMessage(msg);
            player.IsSpecialFuncUsed = true;
        }


        //input the player , targetPlayer ; return the out parameter with changeBuilding
        private bool DiplomatCheck(AbstractPlayer player, AbstractPlayer target, out List<AbstractCard> selfBuilding, out List<AbstractCard> targetBuilding)
        { 
            selfBuilding = null;
            targetBuilding = null;

            //無建築物可換
            if (player.Buildings.Count == 0 || target.Buildings.Count == 0)
                return false;
            
            //夠＄可換?
            targetBuilding = (from tb in target.Buildings
                        from pb in player.Buildings
                        where
                            (pb.RealPoint + player.Money) >=
                            (tb.RealPoint)
                        select tb
                     ).GroupBy(tb=>tb).Select(tg=>tg.Key).ToList();

            selfBuilding = (from tb in target.Buildings
                            from pb in player.Buildings
                            where
                                (pb.RealPoint + player.Money) >=
                                (tb.RealPoint)
                            select pb
                     ).GroupBy(pb => pb).Select(pg => pg.Key).ToList();

            
            if (targetBuilding.Count == 0 || selfBuilding.Count == 0)
                return false;
            
            return true;
        }

        //可美化兩個建物　每個1＄　,增加價值
        public void Artist(Character player, Character target)
        {
            // 要有$ ,要有沒有美化過的building , 最多兩個
            var canBeautiful = from b in player.CurrentPlayer.Buildings where b.IsMoreBeautiful == false select b;
            int count = canBeautiful.Count();
            AbstractCard[] chooseCard = player.CurrentPlayer.ChooseCard.Choose
                ("請選擇想要美化的建築",
                Math.Min(player.CurrentPlayer.Money, Math.Min(count, 2)),
                canBeautiful.ToList() );

            VoiceHelper.AllPlayerSound("lookgood", 100);
            foreach (AbstractCard card in chooseCard)
            {
                card.IsMoreBeautiful = true;
                player.CurrentPlayer.Money--;

            }

            string msg = string.Format("藝術家：『{0}』美化了{1}個地區", player.CurrentPlayer.Name, chooseCard.Count());
            GameSystem.AddSystemMessage(msg);
            player.IsSpecialFuncUsed = true;

        }

        #endregion

    }

}