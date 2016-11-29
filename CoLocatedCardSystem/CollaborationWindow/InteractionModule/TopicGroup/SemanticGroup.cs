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
        SemanticGroup leftChild = null;
        SemanticGroup rightChild = null;
        SemanticGroup parent;
        bool isLeaf=false;

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
        internal SemanticGroup LeftChild
        {
            get
            {
                return leftChild;
            }

            set
            {
                leftChild = value;
            }
        }
        internal SemanticGroup RightChild
        {
            get
            {
                return rightChild;
            }

            set
            {
                rightChild = value;
            }
        }
        internal SemanticGroup Parent
        {
            get
            {
                return parent;
            }

            set
            {
                parent = value;
            }
        }

        public bool IsLeaf
        {
            get
            {
                return isLeaf;
            }

            set
            {
                isLeaf = value;
            }
        }

        internal Topic Topic
        {
            get
            {
                return topic;
            }

            set
            {
                topic = value;
            }
        }

        internal async Task GenBinaryTree(string[] docs, MLController mlController, ConcurrentDictionary<string, SemanticGroup> list)
        {
            if (docs.Length <= 30)
            {
                this.isLeaf = true;
                return;
            }
            var topics = await mlController.GetTopicToken(docs, 2);

            KeyValuePair<Topic, List<string>> pair = topics.ElementAt(0);
            this.leftChild = new SemanticGroup();
            this.leftChild.SetTopic(pair.Key);
            this.leftChild.AddDoc(pair.Value);
            this.leftChild.parent = this;
            list.TryAdd(this.leftChild.id, this.leftChild);

            pair = topics.ElementAt(1);
            this.rightChild = new SemanticGroup();
            this.rightChild.SetTopic(pair.Key);
            this.rightChild.AddDoc(pair.Value);
            this.rightChild.parent = this;
            list.TryAdd(this.rightChild.id, this.rightChild);

            await leftChild.GenBinaryTree(this.leftChild.GetDocs().ToArray(), mlController, list);
            await rightChild.GenBinaryTree(this.rightChild.GetDocs().ToArray(), mlController, list);
        }
        internal void AddDoc(IEnumerable<string> docs)
        {
            foreach (string s in docs)
            {
                AddDoc(s);
            }
        }
        internal void AddDoc(string doc)
        {
            if (!docList.Keys.Contains(doc))
            {
                docList.TryAdd(doc, new UserActionOnDoc());
            }
        }
        internal void SetDocSearched(string docID, User user, bool searched)
        {
            if (docList.Keys.Contains(docID))
            {
                docList[docID].Searched[user] = searched;
            }
        }
        internal UserActionOnDoc RemoveDoc(string docID)
        {
            UserActionOnDoc action = new UserActionOnDoc();
            if (docList.Keys.Contains(docID))
            {
                docList.TryRemove(docID, out action);
            }
            return action;
        }
        internal ConcurrentDictionary<UserActionOnDoc, ConcurrentBag<string>> GetAllDocSubGroups()
        {
            ConcurrentDictionary<UserActionOnDoc, ConcurrentBag<string>> result = new ConcurrentDictionary<UserActionOnDoc, ConcurrentBag<string>>();
            foreach (KeyValuePair<string, UserActionOnDoc> pair in docList)
            {
                UserActionOnDoc actionKey = pair.Value;
                bool existed = false;
                foreach (UserActionOnDoc action in result.Keys)
                {
                    if (action.EqualAction(actionKey))
                    {
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
        internal string GetDescription()
        {
            return String.Join("", topic.GetToken().Select(a => a.OriginalWord));
        }
        internal IEnumerable<Token> GetToken()
        {
            return topic.GetToken();
        }
        internal bool ShareWord(SemanticGroup sg2)
        {
            if (this.leftChild == sg2 || this.rightChild == sg2 || this.parent == sg2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal void SetTopic(Topic tp)
        {
            this.id = tp.Id;
            this.topic = tp;
        }
    }
}
