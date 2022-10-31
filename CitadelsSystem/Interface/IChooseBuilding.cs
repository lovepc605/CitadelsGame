using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CitadelsSystem.Card;
using CitadelsSystem.Players;

namespace CitadelsSystem.Interface
{
    public interface IChooseBuilding
    {
        /// <summary>
        /// 進行AI判斷 , 從可拆除的目標 挑出想拆除的Building
        /// </summary>
        /// <param name="buildings"></param>
        /// <returns></returns>
        AbstractCard ChooseBuilding_WarLord(List<AbstractCard> buildings);

        //return self , target building to exchange
        AbstractCard[] ChooseBuilding_Diplomat(List<AbstractCard> selfBuilding, List<AbstractCard> targetBuilding);
    }
}
