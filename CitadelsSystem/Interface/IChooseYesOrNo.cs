using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CitadelsSystem.Utils;

namespace CitadelsSystem.Interface
{
    public interface IChooseYesOrNo
    {
        int Choose(string title , TwoChoiceEnum choice);
    }
}
