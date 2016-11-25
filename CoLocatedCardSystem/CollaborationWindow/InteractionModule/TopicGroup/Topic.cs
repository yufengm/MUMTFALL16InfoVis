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
        internal class SemanticAttribute
        {
            internal bool isHighlighted = false;
        }
        string id;
        List<Token> list=new List<Token>();
        Dictionary<Token, SemanticAttribute> tokenAttr = new Dictionary<Token, SemanticAttribute>();
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
                tokenAttr.Add(tk, new SemanticAttribute());
            }
        }

        internal void AddToken(Token tk, SemanticAttribute sa)
        {
            if (!list.Contains(tk))
            {
                list.Add(tk);
                tokenAttr.Add(tk, sa);
            }
        }

        internal IEnumerable<Token> GetToken()
        {
            return list;
        }
    }
}
