using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CitadelsSystem.Character;

namespace CitadelsSystem.Interface
{
    public interface IChooseCharacter
    { 
        
        // 顯示可選取的角色，並傳回使用者的 決定角色
        Character.Character Choose(List<Character.Character> input);
        
    }
}
