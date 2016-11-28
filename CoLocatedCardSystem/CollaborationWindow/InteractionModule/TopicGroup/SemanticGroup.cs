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
        ConcurrentDictionary<string, UserActionOnDoc> docList = new ConcurrentDictionary<string, UserActionOnDoc>();//Key is the doc id.
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
            if (!docList.Keys.Contains(doc)) {
                docList.TryAdd(doc,new UserActionOnDoc());
            }
        }
        internal void SetDocSearched(string docID, User user, bool searched) {
            if (docList.Keys.Contains(docID)) {
                docList[docID].Searched[user] = searched;
            }
        }
        internal UserActionOnDoc RemoveDoc(string docID) {
            UserActionOnDoc action=new UserActionOnDoc();
            if (docList.Keys.Contains(docID))
            {
                docList.TryRemove(docID, out action);
            }
            return action;
        }
        internal ConcurrentDictionary<UserActionOnDoc, ConcurrentBag<string>> GetAllDocSubGroups() {
            ConcurrentDictionary<UserActionOnDoc, ConcurrentBag<string>> result = new ConcurrentDictionary<UserActionOnDoc, ConcurrentBag<string>>();
            foreach (KeyValuePair<string, UserActionOnDoc> pair in docList)
            {
                UserActionOnDoc actionKey = pair.Value;
                bool existed = false;
                foreach (UserActionOnDoc action in result.Keys) {
                    if (action.EqualAction(actionKey)) {
                        actionKey = action;
                        existed = true;
                        break;
                    }
                }
                if (!existed)
                {
                    actionKey = actionKey.Copy();
                    result.TryAdd(actionKey, new ConcurrentBag<string>());
                    result[actionKey].Add(pair.Key);
                }
                else
                {
                    result[actionKey].Add(pair.Key);
                }
            }
            return result;
        }
        internal IEnumerable<string> GetDocs()
        {
            return docList.Keys.ToArray();
        }

        internal string GetDescription() {
            return String.Join("", topic.GetToken().Select(a=>a.OriginalWord));
        }
        internal IEnumerable<Token> GetToken() {
            return topic.GetToken();
        }

        internal bool ShareWord(SemanticGroup sg2)
        {
            int count = 0;
            foreach (Token tk1 in this.topic.GetToken())
            {
                foreach (Token tk2 in sg2.topic.GetToken())
                {
                    if (tk1.EqualContent(tk2))
                    {
                        count++;
                    }
                }
            }
            return count > 3;
        }

        internal void AddToken(Token tk, UserActionOnWord sa)
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
