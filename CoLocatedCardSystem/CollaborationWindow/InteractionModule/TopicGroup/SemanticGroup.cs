using CoLocatedCardSystem.CollaborationWindow.DocumentModule;
using CoLocatedCardSystem.CollaborationWindow.MachineLearningModule;
using CoLocatedCardSystem.SecondaryWindow;
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
        int index = 0;
        int hue = ColorPicker.GetColorHue();
        SemanticGroupController semanticGroupController;

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
        internal SemanticGroup(SemanticGroupController ctrls) {
            this.semanticGroupController = ctrls;
        }
        internal ConcurrentDictionary<string, UserActionOnDoc> DocList
        {
            get
            {
                return docList;
            }

            set
            {
                docList = value;
            }
        }

        public int Index
        {
            get
            {
                return index;
            }

            set
            {
                index = value;
            }
        }

        internal void Deinit()
        {
            this.leftChild = null;
            this.rightChild = null;
            this.parent = null;
        }

        internal ConcurrentDictionary<string, UserActionOnDoc> GetSubDocList(List<string> docs)
        {
            ConcurrentDictionary<string, UserActionOnDoc> result = new ConcurrentDictionary<string, UserActionOnDoc>();
            foreach (KeyValuePair<string, UserActionOnDoc> docPair in docList)
            {
                if (docs.Contains(docPair.Key))
                {
                    result.TryAdd(docPair.Key, docPair.Value);
                }
            }
            return result;
        }

        private ConcurrentDictionary<string, UserActionOnDoc> GetDocList()
        {
            return docList;
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
            return String.Join(", ", topic.GetToken().Select(a => a.OriginalWord));
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
        internal void AddDoc(ConcurrentDictionary<string, UserActionOnDoc> docIDs)
        {
            this.docList = docIDs;
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
        internal void SetTopic(Topic tp)
        {
            this.id = tp.Id;
            this.topic = tp;
        }
        /// <summary>
        /// Recursive method to generate the topic tree
        /// </summary>
        /// <param name="docs"></param>
        /// <param name="mlController"></param>
        /// <param name="list"></param>
        /// <param name="maxSize"></param>
        /// <returns></returns>
        internal async Task GenBinaryTree(ConcurrentDictionary<string, UserActionOnDoc> docs, int maxSize)
        {
            if (docs.Count <= maxSize)
            {
                this.isLeaf = true;
                return;
            }
            var topics = await semanticGroupController.Controllers.MlController.GetTopicToken(docs.Keys.ToArray(), 2);
            KeyValuePair<Topic, List<string>> pair = topics.ElementAt(0);
            this.leftChild = new SemanticGroup(semanticGroupController);
            this.leftChild.SetTopic(pair.Key);
            ConcurrentDictionary<string, UserActionOnDoc> leftList = this.GetSubDocList(pair.Value);
            this.leftChild.AddDoc(leftList);
            this.leftChild.parent = this;
            semanticGroupController.SemanticList.AddSemanticGroup(this.leftChild.id, this.leftChild);

            pair = topics.ElementAt(1);
            this.rightChild = new SemanticGroup(semanticGroupController);
            this.rightChild.SetTopic(pair.Key);
            ConcurrentDictionary<string, UserActionOnDoc> rightList = this.GetSubDocList(pair.Value);
            this.rightChild.AddDoc(rightList);
            this.rightChild.parent = this;
            semanticGroupController.SemanticList.AddSemanticGroup(this.rightChild.id, this.rightChild);

            await leftChild.GenBinaryTree(this.leftChild.GetDocList(), maxSize);
            await rightChild.GenBinaryTree(this.rightChild.GetDocList(), maxSize);
        }
        /// <summary>
        /// Based on the card group, split the semantic group. Use the mean vector as the center.
        /// </summary>
        /// <param name="values"></param>
        internal async Task<bool> TrySplit(IEnumerable<CardGroup> cardGroup)
        {
            bool changed = false;
            //Find how many sub groups to split
            ConcurrentDictionary<CardGroup, List<string>> splitList = new ConcurrentDictionary<CardGroup, List<string>>();
            foreach (CardGroup cg in cardGroup.Where(c=>c.Count()>1))
            {
                foreach (string cardID in cg.GetCardID())
                {
                    Document doc = semanticGroupController.Controllers.CardController.DocumentCardController.GetDocumentCardById(cardID).Document;
                    if (docList.Keys.Contains(doc.DocID))
                    {
                        if (!splitList.Keys.Contains(cg))
                        {
                            splitList.TryAdd(cg, new List<string>());
                        }
                        if (!splitList[cg].Contains(doc.DocID))
                        {
                            splitList[cg].Add(doc.DocID);
                        }
                    }
                }
            }
            //Split the group
            if (splitList.Count > 1)
            {
                this.isLeaf = false;
                changed = await SplitSemanticGroup(splitList);
            }
            return changed;
        }
        /// <summary>
        /// Recurive method to split the group
        /// </summary>
        /// <param name="semanticGroup">the semantic group</param>
        /// <param name="splitList">the group to split into</param>
        /// <param name="centers">the centers of all the subgroups</param>
        /// <param name="docVector">the mapping of the doc and vector</param>
        private async Task<bool> SplitSemanticGroup(
            ConcurrentDictionary<CardGroup, List<string>> splitList)
        {
            if (splitList.Count <= 1)
            {
                return false;
            }
            CardGroup sigGroup = splitList.First().Key;

            double[] leftCenter;
            double[] rightCenter;

            Document[] leftDocs = semanticGroupController.Controllers.DocumentController.GetDocument(splitList[sigGroup].ToArray());
            leftCenter = Calculator.CalAvgVector(leftDocs.Select(d => d.GetVector()));
            List<string> restDocs = new List<string>();
            foreach (KeyValuePair<CardGroup, List<string>> pair in splitList) {
                if (pair.Key != sigGroup) {
                    foreach(string id in pair.Value)
                    {
                        if (!restDocs.Contains(id)) {
                            restDocs.Add(id);
                        }
                    }
                }
            }
            Document[] rightDocs = semanticGroupController.Controllers.DocumentController.GetDocument(restDocs.ToArray());
            rightCenter = Calculator.CalAvgVector(rightDocs.Select(d => d.GetVector()));
            List<string> leftChildDocs = new List<string>();
            List<string> rightChildDocs = new List<string>();
            foreach (string docID in docList.Keys)
            {
                Document doc = semanticGroupController.Controllers.DocumentController.GetDocument(docID);
                if (leftDocs.Contains(doc))
                {
                    leftChildDocs.Add(docID);
                }
                else if (rightDocs.Contains(doc))
                {
                    rightChildDocs.Add(docID);
                }
                else
                {
                    double leftDist = Calculator.CalDistance(doc.GetVector(), leftCenter);
                    double rightDist = Calculator.CalDistance(doc.GetVector(), rightCenter);
                    if (leftDist < rightDist)
                    {
                        leftChildDocs.Add(docID);
                    }
                    else
                    {
                        rightChildDocs.Add(docID);
                    }
                }
            }

            this.leftChild = new SemanticGroup(semanticGroupController);

            ConcurrentDictionary<string, UserActionOnDoc> subGroup = this.GetSubDocList(leftChildDocs);
            this.leftChild.AddDoc(subGroup);
            var topics = await semanticGroupController.Controllers.MlController.GetTopicToken(leftChildDocs.ToArray(), 1);
            KeyValuePair<Topic, List<string>> topic = topics.ElementAt(0);
            this.leftChild.SetTopic(topic.Key);
            this.leftChild.Parent = this;
            semanticGroupController.SemanticList.AddSemanticGroup(this.leftChild.Id, this.leftChild);
            this.leftChild.IsLeaf = true;
            List<string> trash;
            splitList.TryRemove(sigGroup, out trash);
            subGroup = this.GetSubDocList(rightChildDocs);
            this.rightChild = new SemanticGroup(semanticGroupController);
            this.rightChild.AddDoc(subGroup);
            topics = await semanticGroupController.Controllers.MlController.GetTopicToken(rightChildDocs.ToArray(), 1);
            topic = topics.ElementAt(0);
            this.rightChild.SetTopic(topic.Key);
            this.rightChild.Parent = this;
            semanticGroupController.SemanticList.AddSemanticGroup(this.rightChild.Id, this.rightChild);
            if (splitList.Count == 1)
            {
                this.rightChild.IsLeaf = true;
                return true;
            }
            else {
                return await this.rightChild.SplitSemanticGroup(splitList);
            }
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
        
        /// <summary>
        /// Get the user action nodes on each semantic group
        /// The keys are the unique user search actions on the docs
        /// </summary>
        /// <returns></returns>
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

        internal string[] GetKeyImage()
        {
            string[] result;
            //List<string> result = new List<string>();
            ConcurrentDictionary<string, double> imagesShown = new ConcurrentDictionary<string, double>();

            var tklist = this.topic.GetToken();
            foreach (Token tk in tklist)
            {
                foreach (string docID in docList.Keys)
                {
                    Document doc = semanticGroupController.Controllers.DocumentController.GetDocument(docID);
                    List<ImageVector> vectors = doc.GetImageVector(semanticGroupController.Controllers.DocumentController);
                    foreach (ImageVector iv in vectors)
                    {
                        //iv.Id;//jpg name
                        foreach (KeyValuePair<string, double> pair in iv.List)
                        {
                            if (pair.Key.Contains(tk.StemmedWord))
                            {
                                imagesShown.TryAdd(iv.Id, pair.Value);
                            }
                            //pair.Key;
                            //pair.Value;
                        }
                    }
                }

            }
            var imageListOrdered = imagesShown.OrderByDescending(e => e.Value).ToList();
            if (imageListOrdered.Count > 0)
            {
                int thr = docList.Keys.Count / 10;
                int imagenum;
                // Determine number of images to be shown
                if (thr > 3)
                {
                    if (imageListOrdered.Count >= 3)
                    {
                        imagenum = 3;
                    }
                    else
                    {
                        imagenum = imageListOrdered.Count;
                    }
                }
                else
                {
                    if (imageListOrdered.Count >= 3)
                    {
                        imagenum = thr;
                    }
                    else
                    {
                        if (thr < imageListOrdered.Count)
                        {
                            imagenum = thr;
                        }
                        else
                        {
                            imagenum = imageListOrdered.Count;
                        }
                    }
                }

                result = new string[imagenum];
                for (int i = 0; i < imagenum; i++)
                {
                    result[i] = imageListOrdered[i].Key;
                }
            }
            else
            {
                result = null;
            }
            return result;
        }
    }
}
