using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using CitadelsSystem.Players;

namespace CitadelsSystem.Card
{
    public abstract class AbstractCard
    {
        #region Fields

        /// <summary>
        /// 分數
        /// </summary>
        public int Point;

        /// <summary>
        /// 加分
        /// </summary>
        public int ExtraPoint;

        /// <summary>
        /// 是否有美化
        /// </summary>
        public bool IsMoreBeautiful;
        /// <summary>
        /// 卡片名稱
        /// </summary>
        public string Name;

        /// <summary>
        /// 描述
        /// </summary>
        public string Desc;

        /// <summary>
        /// 顏色 ,Gold is anyColor
        /// </summary>
        public Color CardColor; 

        /// <summary>
        /// 卡片內容
        /// </summary>
        private BitmapSource frontImg;

        /// <summary>
        /// 卡片背面
        /// </summary>
        public BitmapSource BackImg;

        /// <summary>
        /// 建築是否可以破壞(堡壘特殊功能)
        /// </summary>
        public bool IsDestructible = true;

        /// <summary>
        /// 建築此卡片的回合數
        /// </summary>
        public int BuildRound;

        /// <summary>
        /// 拆除此建築物所需額外的＄
        /// </summary>
        public int ExtraDestructionPoint;

        #endregion

        #region Property
        
        public BitmapSource FrontImg
        {
            get { return frontImg; }
            set { frontImg = value; }
        }

        /// <summary>
        /// 真正的分數
        /// </summary>
        public int RealPoint
        {

            get { return Point + ExtraPoint + (IsMoreBeautiful?1:0); }
        }

        public int DestructionPoint
        {
            get { return RealPoint + ExtraDestructionPoint - 1; }
        }

        #endregion

        public override string ToString()
        {
            return string.Format("{0}_{1}_{2}",this.Name,this.CardColor,this.Point);
        }

        public override bool Equals(object obj)
        {
            return this.ToString().Equals(obj.ToString());
        }
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }


        public bool CanBuild(AbstractPlayer player)
        {

            //check if the player has enough money to build this building.
            //player can't build the same building , the wizard is an exception!
            if ((player.Money - player.ExtraBuildPoint >= (this.Point - player.DiscountPoint))
              && (!player.Buildings.Contains(this) || player.CurrentChar.Name =="Wizard" ))
            {
                return true;
            }
            else
                return false;
        }

        public abstract void RemoveBuilding(AbstractPlayer player);


        public virtual void DoBuilding(AbstractPlayer player,int CostMoney)
        {
       
            player.Money -= CostMoney;
            this.BuildRound = GameSystem.CurrentRound;
            player.Buildings.Add(this);
            
        }

    }
}