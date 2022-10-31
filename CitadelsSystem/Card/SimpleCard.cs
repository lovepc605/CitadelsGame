using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CitadelsSystem.Players;

namespace CitadelsSystem.Card
{
    public class SimpleCard: AbstractCard
    {
        public override void RemoveBuilding(AbstractPlayer player)
        {
            player.MyMessage.Enqueue(string.Format("『{0}』的建築物『{1}』被移除了",player.Name,this.Name));
            player.Buildings.Remove(this);
        }

        public override void DoBuilding(AbstractPlayer player ,int cost)
        {
            base.DoBuilding(player,cost);
            player.MyMessage.Enqueue(string.Format("『{0}』擁有建築物『{1}』", player.Name, this.Name));
        }
    }
}