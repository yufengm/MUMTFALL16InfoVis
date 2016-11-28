using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    class UserActionOnDoc
    {
        Dictionary<User, bool> searched = new Dictionary<User, bool>();

        public Dictionary<User, bool> Searched
        {
            get
            {
                return searched;
            }

            set
            {
                searched = value;
            }
        }

        public UserActionOnDoc() {
            foreach (User user in Enum.GetValues(typeof(User)))
            {
                searched.Add(user, false);
            }
        }

        internal bool EqualAction(UserActionOnDoc action)
        {
            foreach (User user in Enum.GetValues(typeof(User)))
            {
                if (action.Searched[user] != searched[user]) {
                    return false;
                }
            }
            return true;
        }

        internal UserActionOnDoc Copy()
        {
            UserActionOnDoc action = new UserActionOnDoc();
            foreach (User user in Enum.GetValues(typeof(User)))
            {
                action.Searched[user] = searched[user];
            }
            return action;
        }
    }
}
