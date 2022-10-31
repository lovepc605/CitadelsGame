using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CitadelsSystem.Players;

namespace CitadelsSystem.Interface
{
    public interface IChooseTarget
    {
        //根據輸入,顯示可選擇的角色,並傳回Player的決定
        AbstractPlayer Choose(List<AbstractPlayer> input);
    }
}
