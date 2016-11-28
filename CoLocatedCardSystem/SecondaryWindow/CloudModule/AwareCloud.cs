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
                if (pair.Value.Type == CloudNode.NODETYPE.WORD|| pair.Value.Type == CloudNode.NODETYPE.PICTURE)
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
        internal void SetCloudNodeWeight(string id, double weight)
        {
            var node = FindNode(id);
            if (node != null)
            {
                node.Weight = (float)weight;
                Size tsize = Calculator.GetBoundingSize(node.CloudText, node.Weight);
                node.W = (float)tsize.Width;
                node.H = (float)tsize.Height;
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

        internal async void UpdateCloudNode(IEnumerable<SemanticGroup> sgroups)
        {
            RemoveNonDocNodes();
            foreach (SemanticGroup sg in sgroups)
            {
                ConcurrentDictionary<UserActionOnDoc, ConcurrentBag<string>> subgroups = sg.GetAllDocSubGroups();
                foreach (KeyValuePair<UserActionOnDoc, ConcurrentBag<string>> pair in subgroups)
                {
                    SemanticNode sn = animationController.SemanticCloud.FindNode(sg.Id, pair.Key);
                    foreach (string docID in pair.Value)
                    {
                        CloudNode node = FindNode(docID);
                        if (node != null)
                        {
                            node.SemanticNode = sn;
                        }
                        else
                        {
                            CreateCloudNode(docID, CloudNode.NODETYPE.DOC);
                            InitCloudNodeToGroup(docID, sn.Guid);
                            SetCloudNodeDoc(docID, docID);
                            SetCloudNodePosition(docID, sn.X + Rand.Next(20) - 10, sn.Y + Rand.Next(20) - 10);
                        }
                    }
                    Topic topic = await animationController.AwareCloudController.Controllers.MlController.GetTopicToken(pair.Value.ToArray());
                    foreach (Token tk in topic.GetToken())
                    {
                        CreateCloudNode(sn.Guid + tk.StemmedWord, CloudNode.NODETYPE.WORD);
                        InitCloudNodeToGroup(sn.Guid + tk.StemmedWord, sn.Guid);
                        SetCloudNodeText(sn.Guid + tk.StemmedWord, tk.OriginalWord, tk.StemmedWord);
                        SetCloudNodeWeight(sn.Guid + tk.StemmedWord, 15);
                        SetCloudNodePosition(sn.Guid + tk.StemmedWord, sn.X + Rand.Next(20) - 10, sn.Y + Rand.Next(20) - 10);
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
                firstNode.X += (float)((this.timeStep * firstNode.Vx + acc.X * Math.Pow(this.timeStep, 2) / 2.0) * this.moveStep);
                firstNode.Y += (float)((this.timeStep * firstNode.Vy + acc.Y * Math.Pow(this.timeStep, 2) / 2.0) * this.moveStep);
            }
            this.UpdateStrengthLength(energy0);
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
                atrc = -160 * dist;
            }
            else
            {
                atrc = -100 * dist;
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
                else if (!(node1.Type == CloudNode.NODETYPE.DOC && node2.Type != CloudNode.NODETYPE.DOC))
                {
                    if (node1.SemanticNode == node2.SemanticNode)
                    {
                        rpl = -9000 * Math.Max(deltaXY.X, deltaXY.Y);
                    }
                    else
                    {
                        rpl = -9000 * Math.Max(deltaXY.X, deltaXY.Y);
                    }
                }
                result.X = rpl * (node2.X + node2.W / 2 - node1.X - node1.W / 2) / dist;
                result.Y = rpl * (node2.Y + node2.H / 2 - node1.Y - node1.H / 2) / dist;
            }
            return result;
        }
    }
}
