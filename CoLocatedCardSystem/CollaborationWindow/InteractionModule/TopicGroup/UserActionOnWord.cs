using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    class UserActionOnWord
    {
        Dictionary<User, bool> isHighLight = new Dictionary<User, bool>();
        public UserActionOnWord()
        {
            foreach (User user in Enum.GetValues(typeof(User)))
            {
                isHighLight.Add(user, false);
            }
        }
    }
}
