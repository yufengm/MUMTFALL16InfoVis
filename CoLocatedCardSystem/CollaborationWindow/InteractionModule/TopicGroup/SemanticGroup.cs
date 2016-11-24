using CoLocatedCardSystem.CollaborationWindow.DocumentModule;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    class SemanticGroup
    {
        string id;//same with the topic id
        internal struct SemanticAttribute {
            internal bool isHighlighted;
        }
        ConcurrentDictionary<string, Semantic> list = new ConcurrentDictionary<string, Semantic>();//Key is the doc id.
        Dictionary<Token, SemanticAttribute> coolElements=new Dictionary<Token, SemanticAttribute>();
        /// <summary>
        /// The id of the semantic group. Always the same with topic id.
        /// </summary>
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

        internal Dictionary<Token, SemanticAttribute> CoolElements
        {
            get
            {
                return coolElements;
            }

            set
            {
                coolElements = value;
            }
        }

        internal void AddToken(Token tk, SemanticAttribute sa)
        {
            if (!coolElements.Keys.Contains(tk))
            {
                coolElements.Add(tk, sa);
            }
        }

        internal void AddSemantic(string id, Semantic semantic)
        {
            if (!list.Keys.Contains(id)) {
                list.TryAdd(id, semantic);
            }
        }

        internal string GetDescription() {
            return String.Join("", coolElements.Keys.Select(a=>a.OriginalWord));
        }
        internal IEnumerable<Token> GetToken() {
            return coolElements.Keys;
        }
        internal IEnumerable<Semantic> GetSemantics()
        {
            return list.Values;
        }

        internal bool ShareWord(SemanticGroup sg2)
        {
            int count = 0;
            foreach (Token tk1 in this.coolElements.Keys) {
                foreach (Token tk2 in sg2.CoolElements.Keys) {
                    if (tk1.EqualContent(tk2)) {
                        count++;
                    }
                }
            }
            return count>3;
        }
    }
}
