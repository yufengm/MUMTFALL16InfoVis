using CoLocatedCardSystem.CollaborationWindow.DocumentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    class Topic
    {
        string id;
        List<Token> list=new List<Token>();
        Dictionary<Token, UserActionOnWord> tokenAttr = new Dictionary<Token, UserActionOnWord>();
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

        internal void AddToken(Token tk)
        {
            if (!list.Contains(tk))
            {
                list.Add(tk);
                tokenAttr.Add(tk, new UserActionOnWord());
            }
        }

        internal IEnumerable<Token> GetToken()
        {
            return list;
        }
    }
}
