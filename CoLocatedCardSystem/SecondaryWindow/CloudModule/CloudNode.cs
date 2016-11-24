using CoLocatedCardSystem.CollaborationWindow;
using CoLocatedCardSystem.SecondaryWindow.SemanticModule;
using System.Collections.Generic;
using Windows.UI;
using System;

namespace CoLocatedCardSystem.SecondaryWindow.CloudModule
{
    class CloudNode
    {
        internal enum NODETYPE
        {
            PICTURE, WORD, DOC
        }
        internal class UserAction
        {
            internal bool select = false;
            internal bool highlight = false;
            internal bool searched = false;
            internal Color select_color = MyColor.DarkBlue;
            internal Color highlight_color = MyColor.DarkBlue;
            internal Color searched_color = MyColor.DarkBlue;
            internal Color default_color = MyColor.DarkBlue;
        }
        string guid = "";// for docs, the id is the doc id + user name, for word, the id is doc+stemmedword
        string docID = "";
        NODETYPE type = NODETYPE.DOC;
        string cloudText = "test";
        string image = null;
        string stemmedText = "test";
        float x = 0;
        float y = 0;
        float vx = 0;
        float vy = 0;
        SemanticNode semanticNode = null;
        SemanticNode topicNode = null;
        float weight = 22;
        float w = 22;
        float h = 22;
        Dictionary<User, UserAction> user_action = new Dictionary<User, UserAction>();
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

        internal Dictionary<User, UserAction> User_action
        {
            get
            {
                return user_action;
            }

            set
            {
                user_action = value;
            }
        }

        public string DocID
        {
            get
            {
                return docID;
            }

            set
            {
                docID = value;
            }
        }

        internal SemanticNode TopicNode
        {
            get
            {
                return topicNode;
            }

            set
            {
                topicNode = value;
            }
        }

        #endregion
        internal CloudNode()
        {
            user_action.Add(User.ALEX, new UserAction());
            user_action.Add(User.BEN, new UserAction());
            user_action.Add(User.CHRIS, new UserAction());
            user_action.Add(User.DANNY, new UserAction());
            user_action.Add(User.NONE, new UserAction());
        }

        internal void SetSearch(User user, bool active)
        {
            user_action[user].searched = active;
        }

        //Get the name of the the new semantic node that this node shoud be added to
        internal string GetSemanticNode_Searched()
        {
            string result = topicNode.Guid;
            foreach (KeyValuePair<User, UserAction> ua in user_action) {
                if (ua.Value.searched) {
                    result += ua.Key.ToString();
                }
            }
            return result;
        }

        internal void SetColor(double H, double S, double V)
        {
            user_action[User.ALEX].searched_color = UIHelper.HsvToRgb(H, S, 0.75);
            user_action[User.ALEX].default_color = UIHelper.HsvToRgb(H, S, 0.5);
            user_action[User.ALEX].highlight_color = UIHelper.HsvToRgb(H, S, 1);
            user_action[User.ALEX].select_color = UIHelper.HsvToRgb(H, S, 1);

            user_action[User.BEN].searched_color = UIHelper.HsvToRgb(H, S, 0.75);
            user_action[User.BEN].default_color = UIHelper.HsvToRgb(H, S, 0.5);
            user_action[User.BEN].highlight_color = UIHelper.HsvToRgb(H, S, 1);
            user_action[User.BEN].select_color = UIHelper.HsvToRgb(H, S, 1);

            user_action[User.CHRIS].searched_color = UIHelper.HsvToRgb(H, S, 0.75);
            user_action[User.CHRIS].default_color = UIHelper.HsvToRgb(H, S, 0.5);
            user_action[User.CHRIS].highlight_color = UIHelper.HsvToRgb(H, S, 1);
            user_action[User.CHRIS].select_color = UIHelper.HsvToRgb(H, S, 1);

            user_action[User.DANNY].searched_color = UIHelper.HsvToRgb(H, S, 0.75);
            user_action[User.DANNY].default_color = UIHelper.HsvToRgb(H, S, 0.5);
            user_action[User.DANNY].highlight_color = UIHelper.HsvToRgb(H, S, 1);
            user_action[User.DANNY].select_color = UIHelper.HsvToRgb(H, S, 1);

            user_action[User.NONE].searched_color = UIHelper.HsvToRgb(H, S, 0.75);
            user_action[User.NONE].default_color = UIHelper.HsvToRgb(H, S, 0.5);
            user_action[User.NONE].highlight_color = UIHelper.HsvToRgb(H, S, 1);
            user_action[User.NONE].select_color = UIHelper.HsvToRgb(H, S, 1);
        }
    }
}
