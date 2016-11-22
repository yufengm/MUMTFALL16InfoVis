using CoLocatedCardSystem.CollaborationWindow.DocumentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.CollaborationWindow.MachineLearningModule
{
    class Topic
    {
        string id;
        List<Token> list=new List<Token>();

        public string Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        internal List<Token> List
        {
            get
            {
                return list;
            }

            set
            {
                list = value;
            }
        }

        internal void AddToken(Token tk)
        {
            if (!list.Contains(tk))
            {
                list.Add(tk);
            }
        }
    }
}
