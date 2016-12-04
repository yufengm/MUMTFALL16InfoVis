using CoLocatedCardSystem.CollaborationWindow;
using CoLocatedCardSystem.CollaborationWindow.Layers.Menu_Layer;
using CoLocatedCardSystem.CollaborationWindow.Tool;
using CoLocatedCardSystem.SecondaryWindow.CloudModule;
using CoLocatedCardSystem.SecondaryWindow.Layers;
using CoLocatedCardSystem.SecondaryWindow.SemanticModule;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Windows.Foundation;
using CoLocatedCardSystem.CollaborationWindow.InteractionModule;
using CoLocatedCardSystem.CollaborationWindow.DocumentModule;
using Microsoft.Graphics.Canvas;
using Windows.UI.Xaml.Media.Imaging;

namespace CoLocatedCardSystem.SecondaryWindow.CloudModule
{
    class AwareCloud
    {
        ConcurrentDictionary<string, CloudNode> cloudNodes = new ConcurrentDictionary<string, CloudNode>();
        double timeStep = 1 / 50.0;
        double progress = 0;
        double moveStep = AnimationController.INITALSTEP;
        double energy = 0;
        AnimationController animationController;
        public AwareCloud(AnimationController ctrls)
        {
            this.animationController = ctrls;
        }
        public double MoveStep
        {
            get
            {
                return moveStep;
            }

            set
            {
                moveStep = value;
            }
        }
        internal void Init()
        {
        }
        internal ConcurrentDictionary<string, CloudNode> GetCloudNodes()
        {
            return cloudNodes;
        }
        internal CloudNode FindNode(string id)
        {
            if (cloudNodes.Keys.Contains(id))
            {
                return cloudNodes[id];
            }
            return null;
        }
        internal CloudNode CreateCloudNode(string id, CloudNode.NODETYPE type)
        {
            CloudNode node = FindNode(id);
            if (node == null)
            {
                node = new CloudNode();
                node.Guid = id;
                node.Type = type;
                cloudNodes.TryAdd(node.Guid, node);
            }
            return node;
        }
        internal void RemoveNonDocNodes()
        {
            List<string> tobeRemoved = new List<string>();
            foreach (KeyValuePair<string, CloudNode> pair in cloudNodes)
            {
                if (pair.Value.Type == CloudNode.NODETYPE.WORD || pair.Value.Type == CloudNode.NODETYPE.PICTURE)
                {
                    tobeRemoved.Add(pair.Key);
                }
            }
            foreach (string key in tobeRemoved)
            {
                RemoveCloudNode(key);
            }
        }

        internal void SetCloudNodeText(string id, string cloudText, string stemmedText)
        {
            var node = FindNode(id);
            if (node != null && node.Type == CloudNode.NODETYPE.WORD)
            {
                node.CloudText = cloudText;
                node.StemmedText = stemmedText;
                Size tsize = Calculator.GetBoundingSize(node.CloudText, node.Weight);
                node.W = (float)tsize.Width;
                node.H = (float)tsize.Height;
            }
        }
        internal void SetCloudNodeDoc(string id, string docID)
        {
            var node = FindNode(id);
            if (node != null && node.Type == CloudNode.NODETYPE.DOC)
            {
                node.DocID = docID;
            }
        }

        private void SetCloudNodePicture(string id, string img)
        {
            var node = FindNode(id);
            if (node != null && node.Type == CloudNode.NODETYPE.PICTURE)
            {

                node.Image = img;
            }
        }
        private void SetCloudNodeUserAction(string id, UserActionOnDoc action)
        {
            var node = FindNode(id);
            if (node != null)
            {
                node.UserActionOnDoc=action;
            }
        }
        internal void SetCloudNodeWeight(string id, float weight)
        {
            var node = FindNode(id);
            if (node != null)
            {
                node.Weight = (float)weight;
                if (node.Type == CloudNode.NODETYPE.DOC)
                {
                    node.W = node.Weight;
                    node.H = node.Weight;
                }
                else if (node.Type == CloudNode.NODETYPE.WORD)
                {
                    Size tsize = Calculator.GetBoundingSize(node.CloudText, node.Weight);
                    node.W = (float)tsize.Width;
                    node.H = (float)tsize.Height;
                }
            }
        }
        private void RemoveCloudNode(string id)
        {
            if (cloudNodes.ContainsKey(id))
            {
                CloudNode node;
                cloudNodes.TryRemove(id, out node);
            }
        }

        internal void UpdateCloudNode(IEnumerable<SemanticGroup> sgroups)
        {
            RemoveNonDocNodes();
            foreach (SemanticGroup sg in sgroups)
            {
                if (sg.IsLeaf)
                {
                    ConcurrentDictionary<UserActionOnDoc, ConcurrentBag<string>> subgroups = sg.GetAllDocSubGroups();
                    SemanticNode rootNode=null;
                    int nodeNum = 0;
                    foreach (KeyValuePair<UserActionOnDoc, ConcurrentBag<string>> pair in subgroups)
                    {
                        SemanticNode sn = animationController.SemanticCloud.FindNode(sg.Id, pair.Key);
                        foreach (string docID in pair.Value)
                        {
                            CloudNode node = FindNode(docID);
                            if (node != null && sn != null)//update node group
                            {
                                node.SemanticNode = sn;
                                SetCloudNodeUserAction(docID, sg.GetUserActionOnDoc(docID));
                            }
                            else//Add new node to the group
                            {
                                CreateCloudNode(docID, CloudNode.NODETYPE.DOC);
                                InitCloudNodeToGroup(docID, sn.Guid);
                                SetCloudNodeDoc(docID, docID);
                                SetCloudNodeUserAction(docID, sg.GetUserActionOnDoc(docID));
                                SetCloudNodePosition(docID, sn.X + Rand.Next(20) - 10, sn.Y + Rand.Next(20) - 10);
                                SetCloudNodeWeight(docID, 20);
                            }
                        }
                        if (rootNode == null)
                        {
                            rootNode = sn;
                            nodeNum = pair.Value.Count;
                        }
                        else
                        {
                            if (nodeNum < pair.Value.Count) {
                                rootNode = sn;
                                nodeNum = pair.Value.Count;
                            }
                        }
                    }
                    foreach (Token tk in sg.Topic.GetToken())
                    {
                        string newID = rootNode.Guid + tk.StemmedWord;
                        CreateCloudNode(newID, CloudNode.NODETYPE.WORD);
                        InitCloudNodeToGroup(newID, rootNode.Guid);
                        SetCloudNodeText(newID, tk.OriginalWord, tk.StemmedWord);
                        SetCloudNodeWeight(newID, sg.Topic.GetTopicTokenWeight(tk));
                        SetCloudNodePosition(newID, rootNode.X + Rand.Next(20) - 10, rootNode.Y + Rand.Next(20) - 10);
                    }
                    var imgs = sg.GetKeyImage();
                    if (imgs != null)
                    {
                        foreach (string img in imgs)
                        {
                            string newID = rootNode.Guid + img;
                            CreateCloudNode(newID, CloudNode.NODETYPE.PICTURE);
                            InitCloudNodeToGroup(newID, rootNode.Guid);
                            SetCloudNodePicture(newID, img);
                            SetCloudNodeWeight(newID, 20);
                            SetCloudNodePosition(newID, rootNode.X + Rand.Next(20) - 10, rootNode.Y + Rand.Next(20) - 10);
                        }
                    }
                }
            }
        }

        internal void SetCloudNodePosition(string id, double xposi, double yposi)
        {
            CloudNode node = FindNode(id);
            if (node != null)
            {
                node.X = (float)xposi;
                node.Y = (float)yposi;
            }
        }
        internal void InitCloudNodeToGroup(string id, string snid)
        {
            SemanticNode semanticNode = animationController.SemanticCloud.FindNode(snid);
            if (semanticNode != null)
            {
                CloudNode node = FindNode(id);
                if (node != null)
                {
                    node.SemanticNode = semanticNode;
                    node.X = (float)semanticNode.X + Rand.Next(50) - 25;
                    node.Y = (float)semanticNode.Y + Rand.Next(50) - 25;
                }
            }
        }
        internal void Update()
        {
            double energy0 = this.energy;
            foreach (CloudNode firstNode in cloudNodes.Values)
            {
                if (firstNode.SemanticNode != null)
                {
                    Point f = new Point();
                    Point replEng = new Point();
                    int replCount = 0;
                    foreach (CloudNode secondNode in cloudNodes.Values)
                    {
                        if (firstNode != secondNode && Calculator.Distance(firstNode, secondNode) < SecondaryScreen.WIDTH / 10)
                        {
                            replCount += 1;
                            Point repel = this.CalRepel(firstNode, secondNode);
                            f.X += repel.X;
                            f.Y += repel.Y;
                            replEng.X += repel.X;
                            replEng.Y += repel.Y;
                        }
                    }
                    this.energy += replEng.X * replEng.X + replEng.Y * replEng.Y;
                    Point attraction = this.CalCenterAttraction(firstNode);
                    f.X += attraction.X;
                    f.Y += attraction.Y;
                    this.energy += attraction.X * attraction.X + attraction.Y * attraction.Y;
                    Point boarderRepel = this.CalBorderRepel(firstNode);
                    f.X += boarderRepel.X;
                    f.Y += boarderRepel.Y;
                    this.energy += boarderRepel.X * boarderRepel.X + boarderRepel.Y * boarderRepel.Y;
                    Point acc = new Point();
                    acc.X = f.X / firstNode.Weight;
                    acc.Y = f.Y / firstNode.Weight;
                    firstNode.Vx += (float)acc.X;
                    firstNode.Vy += (float)acc.Y;
                    double speed = Calculator.Distance(0, 0, firstNode.Vx, firstNode.Vy);
                    if (speed > 120)
                    {
                        firstNode.Vx = (float)(100 * firstNode.Vx / speed);
                        firstNode.Vy = (float)(100 * firstNode.Vy / speed);
                        speed = 120;
                    }
                    float deltaX = (float)((this.timeStep * firstNode.Vx + acc.X * Math.Pow(this.timeStep, 2) / 2.0) * this.moveStep);
                    float deltaY = (float)((this.timeStep * firstNode.Vy + acc.Y * Math.Pow(this.timeStep, 2) / 2.0) * this.moveStep);
                    firstNode.X += deltaX;
                    firstNode.Y += deltaY;
                }
            }
            UpdateStrengthLength(energy0);
        }
        private void UpdateStrengthLength(double energy0)
        {
            if (this.energy < energy0)
            {
                this.progress += 1;
                if (this.progress >= 5)
                {
                    this.progress = 0;
                    this.moveStep /= 0.9;
                }
            }
            else
            {
                this.progress = 0;
                this.moveStep *= 0.9;
            }
        }
        private Point CalBorderRepel(CloudNode node)
        {
            if (node.Type == CloudNode.NODETYPE.DOC)
            {
                double dist = Calculator.Distance(node.X + node.W / 2,
                    node.Y + node.H / 2,
                    node.SemanticNode.X,
                    node.SemanticNode.Y) + 0.001;
                double atrc = 0;
                Point result = new Point();
                if (node.X < SecondaryScreen.WIDTH / 10 ||
                    node.X > SecondaryScreen.WIDTH - SecondaryScreen.WIDTH / 10 ||
                    node.Y < SecondaryScreen.HEIGHT / 10 ||
                    node.Y > SecondaryScreen.HEIGHT - SecondaryScreen.HEIGHT / 10)
                {
                    atrc = -10000;
                }
                result.X = atrc * (node.X + node.W / 2 - node.SemanticNode.X) / dist;
                result.Y = atrc * (node.Y + node.H / 2 - node.SemanticNode.Y) / dist;
                return result;
            }
            return new Point();
        }
        private Point CalCenterAttraction(CloudNode node)
        {
            if (node.SemanticNode == null)
            {
                return new Point();
            }
            double dist = Calculator.Distance(node.X + node.W / 2, node.Y + node.H / 2, node.SemanticNode.X, node.SemanticNode.Y) + 0.001;
            double atrc = 0;
            Point result = new Point();
            if (node.Type == CloudNode.NODETYPE.DOC)
            {
                atrc = -1200 * dist;
            }
            else
            {
                atrc = -1200 * dist;
            }
            result.X = atrc * (node.X + node.W / 2 - node.SemanticNode.X) / dist;
            result.Y = atrc * (node.Y + node.H / 2 - node.SemanticNode.Y) / dist;
            return result;
        }

        private Point CalRepel(CloudNode node1, CloudNode node2)
        {
            double dist = Calculator.Distance(node1, node2) + 0.0001;
            Point deltaXY = Calculator.NodeNodeIntersect(node1, node2);
            double rpl = 0;
            Point result = new Point();
            if (deltaXY.X > 0 && deltaXY.Y > 0)
            {
                if (node1.Type == CloudNode.NODETYPE.DOC && node2.Type == CloudNode.NODETYPE.DOC)
                {
                    rpl = -60000 * Math.Min(deltaXY.X, deltaXY.Y);
                }
                else if (!(node1.Type == CloudNode.NODETYPE.DOC && node2.Type != CloudNode.NODETYPE.DOC))//Text don't repel doc
                {
                    if (node1.SemanticNode == node2.SemanticNode)
                    {
                        rpl = -20000 * Math.Max(deltaXY.X, deltaXY.Y);
                    }
                    else
                    {
                        rpl = -20000 * Math.Max(deltaXY.X, deltaXY.Y);
                    }
                }
                result.X = rpl * (node2.X + node2.W / 2 - node1.X - node1.W / 2) / dist;
                result.Y = rpl * (node2.Y + node2.H / 2 - node1.Y - node1.H / 2) / dist;
            }
            return result;
        }
    }
}
