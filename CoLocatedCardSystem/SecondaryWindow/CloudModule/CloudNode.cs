using CoLocatedCardSystem.SecondaryWindow.SemanticModule;
using System.Collections.Generic;

namespace CoLocatedCardSystem.SecondaryWindow.CloudModule
{
    class CloudNode
    {
        internal enum NODETYPE
        {
            PICTURE, WORD, DOC
        }
        string guid = "";// for docs, the id is the doc id, for word, the id is doc+stemmedword
        NODETYPE type = NODETYPE.DOC;
        string cloudText = "test";
        string image = null;
        string stemmedText = "test";
        string owner = "";
        float x = 0;
        float y = 0;
        float vx = 0;
        float vy = 0;
        SemanticNode semanticNode = null;
        float weight = 15;
        float w = 15;
        float h = 15;
        Dictionary<User, bool> user_search = new Dictionary<User, bool>();
        Dictionary<User, bool> user_interact = new Dictionary<User, bool>();
        Dictionary<User, bool> user_highlight = new Dictionary<User, bool>();
        #region getter
        public string Guid
        {
            get
            {
                return guid;
            }

            set
            {
                guid = value;
            }
        }

        internal NODETYPE Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
            }
        }

        public string CloudText
        {
            get
            {
                return cloudText;
            }

            set
            {
                cloudText = value;
            }
        }

        public string Image
        {
            get
            {
                return image;
            }

            set
            {
                image = value;
            }
        }

        public string StemmedText
        {
            get
            {
                return stemmedText;
            }

            set
            {
                stemmedText = value;
            }
        }

        public string Owner
        {
            get
            {
                return owner;
            }

            set
            {
                owner = value;
            }
        }
        internal SemanticNode SemanticNode
        {
            get
            {
                return semanticNode;
            }

            set
            {
                semanticNode = value;
            }
        }
        public float X
        {
            get
            {
                return x;
            }

            set
            {
                x = value;
            }
        }
        public float Y
        {
            get
            {
                return y;
            }

            set
            {
                y = value;
            }
        }
        public float Vx
        {
            get
            {
                return vx;
            }

            set
            {
                vx = value;
            }
        }
        public float Vy
        {
            get
            {
                return vy;
            }

            set
            {
                vy = value;
            }
        }
        public float Weight
        {
            get
            {
                return weight;
            }

            set
            {
                weight = value;
            }
        }
        public float W
        {
            get
            {
                return w;
            }

            set
            {
                w = value;
            }
        }
        public float H
        {
            get
            {
                return h;
            }

            set
            {
                h = value;
            }
        }
        public Dictionary<User, bool> User_search
        {
            get
            {
                return user_search;
            }

            set
            {
                user_search = value;
            }
        }

        public Dictionary<User, bool> User_interact
        {
            get
            {
                return user_interact;
            }

            set
            {
                user_interact = value;
            }
        }

        public Dictionary<User, bool> User_highlight
        {
            get
            {
                return user_highlight;
            }

            set
            {
                user_highlight = value;
            }
        }
        #endregion
        internal CloudNode()
        {
            user_search.Add(User.ALEX, false);
            user_search.Add(User.BEN, false);
            user_search.Add(User.CHRIS, false);
            user_search.Add(User.DANNY, false);
            user_interact.Add(User.ALEX, false);
            user_interact.Add(User.BEN, false);
            user_interact.Add(User.CHRIS, false);
            user_interact.Add(User.DANNY, false);
            user_highlight.Add(User.ALEX, false);
            user_highlight.Add(User.BEN, false);
            user_highlight.Add(User.CHRIS, false);
            user_highlight.Add(User.DANNY, false);
        }
        internal void SetSearch(User user, bool active)
        {
            if (!user_search.ContainsKey(user))
            {
                user_search.Add(user, active);
            }
            else {
                user_search[user] = active;
            }
        }
    }
}
