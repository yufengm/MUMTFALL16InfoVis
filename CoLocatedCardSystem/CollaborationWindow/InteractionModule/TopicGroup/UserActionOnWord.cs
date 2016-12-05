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
        Dictionary<User, float> weight = new Dictionary<User, float>();
        public UserActionOnWord()
        {
            foreach (User user in Enum.GetValues(typeof(User)))
            {
                isHighLight.Add(user, false);
                weight.Add(user, -1);
            }
            weight[User.NONE]= -1;
        }

        internal void SetUserWeight(User user, float w)
        {
            weight[user] = w;
        }

        internal float GetWeight()
        {
            return weight.Values.Max();
        }
    }
}
