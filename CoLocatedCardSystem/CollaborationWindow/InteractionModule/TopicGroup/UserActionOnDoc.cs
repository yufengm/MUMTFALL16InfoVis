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
        Dictionary<User, bool> active = new Dictionary<User, bool>();
        Dictionary<User, bool> touched = new Dictionary<User, bool>();
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

        public Dictionary<User, bool> Active
        {
            get
            {
                return active;
            }

            set
            {
                active = value;
            }
        }

        public Dictionary<User, bool> Touched
        {
            get
            {
                return touched;
            }

            set
            {
                touched = value;
            }
        }

        public UserActionOnDoc()
        {
            foreach (User user in Enum.GetValues(typeof(User)))
            {
                searched.Add(user, false);
                active.Add(user, false);
                touched.Add(user, false);
            }
        }

        internal bool EqualSearchAction(UserActionOnDoc action)
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
                action.Active[user] = active[user];
                action.Touched[user] = active[user];
            }
            return action;
        }
    }
}
