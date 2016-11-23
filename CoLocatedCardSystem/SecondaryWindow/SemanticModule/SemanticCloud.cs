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
        double optimal = 60;
        double energy = 0;
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

        internal ConcurrentDictionary<string, SemanticNode> GetSemanticNodes() {
            return semanticNodes;
        }
        internal void Push(SemanticNode node)
        {
            semanticNodes.TryAdd(node.Guid, node);
        }

        internal SemanticNode FindNode(string id)
        {
            if (semanticNodes.Keys.Contains(id))
            {
                return semanticNodes[id];
            }
            return null;
        }
        internal void AddSemanticNode(String snid, String semantic)
        {
            SemanticNode semanticNode = FindNode(snid);
            if (semanticNode == null)
            {
                semanticNode = new SemanticNode();
                semanticNode.X = rand.Next(SecondaryScreen.WIDTH);
                semanticNode.Y = rand.Next(SecondaryScreen.HEIGHT);
                semanticNode.Semantic = semantic;
                semanticNode.Guid = snid;
                this.Push(semanticNode);
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
                Point acc = new Point();
                acc.X = f.X / firstNode.Weight;
                acc.Y = f.Y / firstNode.Weight;
                firstNode.Vx += (float)(acc.X / this.timeStep);
                firstNode.Vy += (float)(acc.Y / this.timeStep);
                var speed = Calculator.Distance(0, 0, firstNode.Vy, firstNode.Vy);
                if (speed > 10)
                {
                    firstNode.Vx = 10 * (float)(firstNode.Vx / speed);
                    firstNode.Vy = 10 * (float)(firstNode.Vy / speed);
                    speed = 10;
                }
                firstNode.X += (float)((this.timeStep * firstNode.Vx + acc.X * Math.Pow(this.timeStep, 2) / 2.0) * this.moveStep);
                center.X += firstNode.X;
                firstNode.Y += (float)((this.timeStep * firstNode.Vy + acc.Y * Math.Pow(this.timeStep, 2) / 2.0) * this.moveStep);
                center.Y += firstNode.Y;
                var fLength = Calculator.Distance(0, 0, f.X, f.Y);
                this.energy += fLength * fLength;
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
                node.X > SecondaryScreen.WIDTH - 100 ||
                node.Y < SecondaryScreen.HEIGHT / 10 ||
                node.Y > SecondaryScreen.HEIGHT - 100)
            {
                atrc = -30000;
            }
            result.X = atrc * (node.X - SecondaryScreen.WIDTH / 2) / dist;
            result.Y = atrc * (node.Y - SecondaryScreen.HEIGHT / 2) / dist;
            return result;
        }

        private Point CalAttraction(SemanticNode firstNode, SemanticNode secondNode)
        {
            double dist = Calculator.Distance(firstNode.X, firstNode.Y, secondNode.X, secondNode.Y);
            double atrc = dist * dist / this.optimal;
            Point result = new Point();
            result.X = atrc * (secondNode.X - firstNode.X) / dist;
            result.Y = atrc * (secondNode.Y - firstNode.Y) / dist;
            return result;
        }

        private Point CalRepel(SemanticNode firstNode, SemanticNode secondNode)
        {
            double dist = Calculator.Distance(firstNode.X, firstNode.Y, secondNode.X, secondNode.Y);
            double rpl = -10 * this.optimal * this.optimal / dist;
            Point result = new Point();
            result.X = rpl * (secondNode.X - firstNode.X) / dist;
            result.Y = rpl * (secondNode.Y - firstNode.Y) / dist;
            return result;
        }
    }
}
