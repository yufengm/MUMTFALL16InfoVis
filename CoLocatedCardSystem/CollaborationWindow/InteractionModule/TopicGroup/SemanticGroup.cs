using CoLocatedCardSystem.CollaborationWindow.DocumentModule;
using CoLocatedCardSystem.CollaborationWindow.MachineLearningModule;
using CoLocatedCardSystem.SecondaryWindow.Tool;
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
        bool isLeaf = false;
        int hue = ColorPicker.GetColorHue();

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
        public int Hue
        {
            get
            {
                return hue;
            }

            set
            {
                hue = value;
            }
        }

        internal void Deinit()
        {
            this.leftChild = null;
            this.rightChild = null;
            this.parent = null;
            this.docList.Clear();
        }
        /// <summary>
        /// Recursive method to generate the topic tree
        /// </summary>
        /// <param name="docs"></param>
        /// <param name="mlController"></param>
        /// <param name="list"></param>
        /// <param name="maxSize"></param>
        /// <returns></returns>
        internal async Task GenBinaryTree(string[] docs, MLController mlController, ConcurrentDictionary<string, SemanticGroup> list, int maxSize)
        {
            if (docs.Length <= maxSize)
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

            await leftChild.GenBinaryTree(this.leftChild.GetDocs().ToArray(), mlController, list, maxSize);
            await rightChild.GenBinaryTree(this.rightChild.GetDocs().ToArray(), mlController, list, maxSize);
        }

        internal SemanticGroup FindCommonParent(string[] docs)
        {
            SemanticGroup result = this;
            if (!this.isLeaf)
            {
                if (leftChild.HasDoc(docs) && leftChild.HasDoc(docs))
                {
                    result = leftChild.FindCommonParent(docs);
                }
                else if (rightChild.HasDoc(docs) && rightChild.HasDoc(docs))
                {
                    result = rightChild.FindCommonParent(docs);
                }
            }
            return result;
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
        internal void SetDocSearched(string docID, User user, bool value)
        {
            if (docList.Keys.Contains(docID))
            {
                docList[docID].Searched[user] = value;
            }
        }

        internal void SetDocActive(string docID, User user, bool value)
        {
            if (docList.Keys.Contains(docID))
            {
                docList[docID].Active[user] = value;
            }
        }
        internal void SetDocTouched(string docID, User user, bool value)
        {
            if (docList.Keys.Contains(docID))
            {
                docList[docID].Touched[user] = value;
            }
        }
        internal bool HasDoc(string docID)
        {
            return docList.Keys.Contains(docID);
        }
        private bool HasDoc(string[] docs)
        {
            foreach (string doc in docs)
            {
                if (!HasDoc(doc))
                {
                    return false;
                }
            }
            return true;
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
                    if (action.EqualSearchAction(actionKey))
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

        internal UserActionOnDoc GetUserActionOnDoc(string docID)
        {
            return docList.Keys.Contains(docID) ? docList[docID] : null;
        }

        internal IEnumerable<string> GetDocs()
        {
            return docList.Keys.ToArray();
        }
        internal string GetDescription()
        {
            return String.Join("", topic.GetToken().Select(a => a.OriginalWord));
        }
        internal bool CheckConnection(SemanticGroup sg2)
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
