using CoLocatedCardSystem.CollaborationWindow.DocumentModule;
using CoLocatedCardSystem.CollaborationWindow.InteractionModule;
using CoLocatedCardSystem.CollaborationWindow.Tool;
using CoLocatedCardSystem.SecondaryWindow.CloudModule;
using CoLocatedCardSystem.SecondaryWindow.Layers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;

namespace CoLocatedCardSystem.SecondaryWindow.SemanticModule
{
    class SemanticCloud
    {
        ConcurrentDictionary<string, SemanticNode> semanticNodes = new ConcurrentDictionary<string, SemanticNode>();
        double timeStep = 1 / 20.0;
        double moveStep = AnimationController.INITALSTEP;
        double progress = 0;
        double energy = 0;
        private AnimationController animationController;

        public SemanticCloud(AnimationController animationController)
        {
            this.animationController = animationController;
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

        internal ConcurrentDictionary<string, SemanticNode> GetSemanticNodes()
        {
            return semanticNodes;
        }
        internal async void UpdateTopicWord()
        {
            animationController.AwareCloud.RemoveWordFromSemantic();
            foreach (KeyValuePair<string, SemanticNode> pair in semanticNodes)
            {
                pair.Value.RemoveWordNode();
            }
            foreach (SemanticNode sn in semanticNodes.Values)
            {
                ConcurrentBag<CloudNode> docNodes = sn.GetDocNode();
                if (docNodes != null && docNodes.Count > 0)
                {
                    List<string> docList = new List<string>();
                    foreach (CloudNode cn in docNodes)
                    {
                        docList.Add(cn.DocID);
                    }
                    Topic topic = await animationController.AwareCloudController.GetSubTopicToken(docList.ToArray());
                    foreach (Token tk in topic.GetToken())
                    {
                        animationController.AwareCloud.CreateCloudNode(sn.Guid + tk.StemmedWord, CloudNode.NODETYPE.WORD, sn.Guid, User.NONE);
                        animationController.AwareCloud.SetCloudNodeText(sn.Guid + tk.StemmedWord, tk.OriginalWord, tk.StemmedWord);
                        animationController.AwareCloud.SetCloudNodeWeight(sn.Guid + tk.StemmedWord, 20);
                    }
                }
            }
        }

        internal SemanticNode FindNode(string id)
        {
            foreach (string key in semanticNodes.Keys)
            {
                if (id.Equals(key))
                {
                    return semanticNodes[key];
                }
            }
            return null;
        }
        internal void AddSemanticNode(String snid, String semantic)
        {
            SemanticNode semanticNode = FindNode(snid);
            if (semanticNode == null)
            {
                semanticNode = new SemanticNode();
                semanticNode.X = SecondaryScreen.WIDTH / 2 + Rand.Next(10) - 5;
                semanticNode.Y = SecondaryScreen.HEIGHT / 2 + Rand.Next(10) - 5;
                semanticNode.Semantic = semantic;
                semanticNode.Guid = snid;
                semanticNodes.TryAdd(semanticNode.Guid, semanticNode);
            }
        }

        internal void ConnectSemanticNode(string snid1, string snid2)
        {
            SemanticNode firstNode = FindNode(snid1);
            SemanticNode secondNode = FindNode(snid2);
            if (firstNode != null && secondNode != null)
            {
                firstNode.Connect(secondNode);
                secondNode.Connect(firstNode);
            }
        }

        internal void SetSemanticNodeColor(string snid, int h, int s, int v)
        {
            SemanticNode semanticNode = FindNode(snid);
            if (semanticNode != null)
            {
                semanticNode.H = h;
                semanticNode.S = s;
                semanticNode.V = v;
            }
        }


        internal void SetSemanticNodePosition(string sid, float x, float y)
        {
            SemanticNode semanticNode = FindNode(sid);
            if (semanticNode != null)
            {
                semanticNode.X = x;
                semanticNode.Y = y;
            }
        }

        internal void SetSemanticNodeOptimal(string id, int optimal)
        {
            SemanticNode semanticNode = FindNode(id);
            if (semanticNode != null)
            {
                semanticNode.Optimal = optimal;
            }
        }
        internal void Update()
        {
            double energy0 = this.energy;
            this.energy = 0;
            Point center = new Point();
            foreach (SemanticNode firstNode in this.semanticNodes.Values)
            {
                Point f = new Point();
                foreach (SemanticNode secondNode in this.semanticNodes.Values)
                {
                    if (firstNode != secondNode)
                    {
                        Point repel = this.CalRepel(firstNode, secondNode);
                        f.X += repel.X;
                        f.Y += repel.Y;
                    }
                }
                foreach (SemanticNode secondNode in firstNode.Connections)
                {
                    Point attraction = this.CalAttraction(firstNode, secondNode);
                    f.X += attraction.X;
                    f.Y += attraction.Y;
                }
                Point borderRepel = this.CalBorderRepel(firstNode);
                f.X += borderRepel.X;
                f.Y += borderRepel.Y;
                this.energy += f.X * f.X + f.Y * f.Y;
                Point acc = new Point();
                acc.X = f.X / firstNode.Weight;
                acc.Y = f.Y / firstNode.Weight;
                firstNode.Vx += (float)(acc.X / this.timeStep);
                firstNode.Vy += (float)(acc.Y / this.timeStep);
                var speed = Calculator.Distance(0, 0, firstNode.Vx, firstNode.Vy);
                if (speed > 100)
                {
                    firstNode.Vx = 100 * (float)(firstNode.Vx / speed);
                    firstNode.Vy = 100 * (float)(firstNode.Vy / speed);
                    speed = 100;
                }
                firstNode.X += (float)((this.timeStep * firstNode.Vx + acc.X * Math.Pow(this.timeStep, 2) / 2.0) * this.moveStep);
                center.X += firstNode.X;
                firstNode.Y += (float)((this.timeStep * firstNode.Vy + acc.Y * Math.Pow(this.timeStep, 2) / 2.0) * this.moveStep);
                center.Y += firstNode.Y;
            }
            center.X = SecondaryScreen.WIDTH / 2 - center.X / this.semanticNodes.Count();
            center.Y = SecondaryScreen.HEIGHT / 2 - center.Y / this.semanticNodes.Count();
            foreach (SemanticNode firstNode in this.semanticNodes.Values)
            {
                firstNode.X += (float)center.X;
                firstNode.Y += (float)center.Y;
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

        private Point CalBorderRepel(SemanticNode node)
        {
            double dist = Calculator.Distance(node.X, node.Y, SecondaryScreen.WIDTH / 2, SecondaryScreen.HEIGHT / 2) + 0.001;
            double atrc = 0;
            Point result = new Point();
            if (node.X < SecondaryScreen.WIDTH / 10 ||
                node.X > SecondaryScreen.WIDTH - SecondaryScreen.WIDTH / 10 ||
                node.Y < SecondaryScreen.HEIGHT / 10 ||
                node.Y > SecondaryScreen.HEIGHT - SecondaryScreen.HEIGHT / 10)
            {
                atrc = -100;
            }
            result.X = atrc * (node.X - SecondaryScreen.WIDTH / 2) / dist;
            result.Y = atrc * (node.Y - SecondaryScreen.HEIGHT / 2) / dist;
            return result;
        }

        private Point CalAttraction(SemanticNode firstNode, SemanticNode secondNode)
        {
            double dist = Calculator.Distance(firstNode.X, firstNode.Y, secondNode.X, secondNode.Y) + 0.001;
            double opt = firstNode.Optimal + secondNode.Optimal;
            double atrc = dist * dist / opt;
            Point result = new Point();
            result.X = atrc * (secondNode.X - firstNode.X) / dist;
            result.Y = atrc * (secondNode.Y - firstNode.Y) / dist;
            return result;
        }

        private Point CalRepel(SemanticNode firstNode, SemanticNode secondNode)
        {
            double dist = Calculator.Distance(firstNode.X, firstNode.Y, secondNode.X, secondNode.Y) + 0.001;
            double opt = firstNode.Optimal + secondNode.Optimal;
            double rpl = -2 * opt * opt / dist;
            Point result = new Point();
            result.X = rpl * (secondNode.X - firstNode.X) / dist;
            result.Y = rpl * (secondNode.Y - firstNode.Y) / dist;
            return result;
        }
    }
}
