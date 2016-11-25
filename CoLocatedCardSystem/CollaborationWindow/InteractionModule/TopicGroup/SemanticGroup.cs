using CoLocatedCardSystem.CollaborationWindow.DocumentModule;
using CoLocatedCardSystem.CollaborationWindow.MachineLearningModule;
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
        ConcurrentBag<string> docList = new ConcurrentBag<string>();//Key is the doc id.
        Topic topic;
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

        internal void AddDoc(string doc)
        {
            if (!docList.Contains(doc)) {
                docList.Add(doc);
            }
        }

        internal string GetDescription() {
            return String.Join("", topic.GetToken().Select(a=>a.OriginalWord));
        }
        internal IEnumerable<Token> GetToken() {
            return topic.GetToken();
        }
        internal IEnumerable<string> GetDocs()
        {
            return docList.ToArray();
        }

        internal bool ShareWord(SemanticGroup sg2)
        {
            int count = 0;
            foreach (Token tk1 in this.topic.GetToken()) {
                foreach (Token tk2 in sg2.topic.GetToken()) {
                    if (tk1.EqualContent(tk2)) {
                        count++;
                    }
                }
            }
            return count>3;
        }

        internal void AddToken(Token tk, Topic.SemanticAttribute sa)
        {
            topic.AddToken(tk, sa);
        }

        internal void SetTopic(Topic tp)
        {
            this.id = tp.Id;
            this.topic = tp;
        }
    }
}
