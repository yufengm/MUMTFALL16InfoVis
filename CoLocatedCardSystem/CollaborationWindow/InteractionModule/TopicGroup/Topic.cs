using CoLocatedCardSystem.CollaborationWindow.DocumentModule;
using CoLocatedCardSystem.SecondaryWindow;
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
        internal void SetTopicWeight(User user, Token token, double weight)
        {           
            tokenAttr[token].SetUserWeight(user, (float)weight);
        }

        internal float GetTopicTokenWeight(Token tk)
        {
            float result = 15;
            float max = tokenAttr.Values.Max(t => t.GetWeight());
            float min = tokenAttr.Values.Min(t => t.GetWeight());
            float weight = tokenAttr[tk].GetWeight();
            result = (float)Calculator.Map(weight, min, max+0.001, 15, 20);
            return result;
        }
    }
}
