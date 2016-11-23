using CoLocatedCardSystem.CollaborationWindow;
using CoLocatedCardSystem.SecondaryWindow.CloudModule;
using CoLocatedCardSystem.SecondaryWindow.Layers;
using CoLocatedCardSystem.SecondaryWindow.SemanticModule;
using System;
using System.Collections.Concurrent;
using Windows.Foundation;

namespace CoLocatedCardSystem.SecondaryWindow.CloudModule
{
    class AwareCloud
    {
        ConcurrentDictionary<string, CloudNode> cloudNodes = new ConcurrentDictionary<string, CloudNode>();
        double timeStep = 1 / 30.0;
        double progress = 0;
        double moveStep = AnimationController.INITALSTEP;
        double energy = 0;
        SemanticCloud semanticCloud;
        Random rand = new Random();

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

        internal void Init(SemanticCloud sc)
        {
            this.semanticCloud = sc;
        }

        internal ConcurrentDictionary<string, CloudNode> GetCloudNodes()
        {
            return cloudNodes;
        }

        internal void Push(CloudNode node)
        {
            cloudNodes.TryAdd(node.Guid, node);
        }

        internal CloudNode FindNode(string id)
        {
            if (cloudNodes.Keys.Contains(id))
            {
                return cloudNodes[id];
            }
            return null;
        }
        internal CloudNode CreateCloudNode(string id, CloudNode.NODETYPE type, string sid)
        {
            CloudNode node = FindNode(id);
            if (node == null)
            {
                node = new CloudNode();
                node.Guid = id;
                node.Type = type;
                Push(node);
                AddCloudNodeToGroup(id, sid);
            }
            return node;
        }
        internal void SetCloudNodeText(string id, string cloudText, string stemmedText)
        {
            var node = FindNode(id);
            if (node != null && node.Type == CloudNode.NODETYPE.WORD)
            {
                node.CloudText = cloudText;
                node.StemmedText = stemmedText;
                Size tsize = UIHelper.GetBoundingSize(node.CloudText, node.Weight);
                node.W = (float)tsize.Width;
                node.H = (float)tsize.Height;
            }
        }

        internal void SetCloudNodeWeight(string id, double weight)
        {
            var node = FindNode(id);
            if (node != null)
            {
                node.Weight = (float)weight;
                Size tsize = UIHelper.GetBoundingSize(node.CloudText, node.Weight);
                node.W = (float)tsize.Width;
                node.H = (float)tsize.Height;
            }
        }

        internal void SetCloudNodeActive(string[] ids, User user, bool active) {
            if (ids != null) {
                foreach(string id in ids)
                {
                    CloudNode node = FindNode(id);
                    if (node != null) {
                        node.SetSearch(user, active);
                    }
                }
            }
        }

        internal void UpdateCloudNodePosition(string id, double xposi, double yposi)
        {
            CloudNode node = FindNode(id);
            if (node != null)
            {
                node.X = (float)xposi;
                node.Y = (float)yposi;
            }
        }

        internal void AddCloudNodeToGroup(string id, string snid)
        {
            SemanticNode semanticNode = semanticCloud.FindNode(snid);
            if (semanticNode != null)
            {
                CloudNode node = FindNode(id);
                if (node != null)
                {
                    var previousSNode = node.SemanticNode;
                    if (previousSNode != null)
                    {
                        previousSNode.RemoveCloudNode(node);
                    }
                    node.SemanticNode = semanticNode;
                    semanticNode.AddCloudNode(node);
                    node.X = (float)semanticNode.X + rand.Next(20) - 10;
                    node.Y = (float)semanticNode.Y + rand.Next(20) - 10;
                }
            }
        }
        internal void Update()
        {
            foreach (CloudNode firstNode in cloudNodes.Values)
            {
                Point f = new Point();
                foreach (CloudNode secondNode in cloudNodes.Values)
                {
                    if (firstNode != secondNode)
                    {
                        Point repel = this.CalRepel(firstNode, secondNode);
                        f.X += repel.X;
                        f.Y += repel.Y;
                    }
                }
                Point attraction = this.CalCenterAttraction(firstNode);
                f.X += attraction.X;
                f.Y += attraction.Y;
                Point acc = new Point();
                acc.X = f.X / firstNode.Weight;
                acc.Y = f.Y / firstNode.Weight;
                firstNode.Vx += (float)acc.X;
                firstNode.Vy += (float)acc.Y;
                double speed = Calculator.Distance(0, 0, firstNode.Vx, firstNode.Vy);
                if (speed > 150)
                {
                    firstNode.Vx = (float)(100 * firstNode.Vx / speed);
                    firstNode.Vy = (float)(100 * firstNode.Vy / speed);
                    speed = 150;
                }
                firstNode.X += (float)((this.timeStep * firstNode.Vx + acc.X * Math.Pow(this.timeStep, 2) / 2.0) * this.moveStep);
                firstNode.Y += (float)((this.timeStep * firstNode.Vy + acc.Y * Math.Pow(this.timeStep, 2) / 2.0) * this.moveStep);
            }
            this.UpdateEnergy();
        }

        private void UpdateEnergy()
        {
            double energy0 = this.energy;
            this.energy = 0;
            foreach (CloudNode firstNode in this.cloudNodes.Values)
            {
                foreach (CloudNode secondNode in this.cloudNodes.Values)
                {
                    if (firstNode != secondNode)
                    {
                        double dist = 0;
                        if (firstNode.Type == CloudNode.NODETYPE.DOC && secondNode.Type == CloudNode.NODETYPE.DOC)
                        {
                            dist = Calculator.Distance(firstNode.X, firstNode.Y, secondNode.X, secondNode.Y);
                        }
                        else
                        {
                            dist = 1000 * Calculator.Distance(firstNode.X + firstNode.W / 2,
                                    firstNode.Y + firstNode.H / 2,
                                    secondNode.X + secondNode.W / 2,
                                    secondNode.Y + secondNode.H / 2);
                        }
                        this.energy += dist;
                    }
                }
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
            double dist = Calculator.Distance(node.X + node.W / 2, node.Y + node.H / 2, node.SemanticNode.X, node.SemanticNode.Y) + 0.001;
            double atrc = 0;
            Point result = new Point();
            if (node.Type == CloudNode.NODETYPE.DOC)
            {
                atrc = -30 * dist;
            }
            else
            {
                atrc = -500 * dist;
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
                    rpl = -5000 * Math.Max(deltaXY.X, deltaXY.Y);
                }
                else if (!(node1.Type == CloudNode.NODETYPE.DOC && node2.Type != CloudNode.NODETYPE.DOC))
                {
                    if (node1.SemanticNode == node2.SemanticNode)
                    {
                        rpl = -50000 * Math.Max(deltaXY.X, deltaXY.Y);
                    }
                    else
                    {
                        rpl = -5000 * Math.Max(deltaXY.X, deltaXY.Y);
                    }
                }
                result.X = rpl * (node2.X + node2.W / 2 - node1.X - node1.W / 2) / dist;
                result.Y = rpl * (node2.Y + node2.H / 2 - node1.Y - node1.H / 2) / dist;
            }
            return result;
        }
    }
}
