using CoLocatedCardSystem.CollaborationWindow;
using CoLocatedCardSystem.SecondaryWindow.CloudModule;
using System.Collections.Concurrent;
using Windows.UI;
using System;

namespace CoLocatedCardSystem.SecondaryWindow.SemanticModule
{
    class SemanticNode
    {
        string guid = "";
        ConcurrentBag<SemanticNode> connections = new ConcurrentBag<SemanticNode>();
        ConcurrentDictionary<CloudNode.NODETYPE, ConcurrentBag<CloudNode>> cloudNodes = new ConcurrentDictionary<CloudNode.NODETYPE, ConcurrentBag<CloudNode>>();
        string semantic = "";
        User owner = User.NONE;
        int h = 0, s = 0, v = 0;
        float x = 0;
        float y = 0;
        float vx = 0;
        float vy = 0;
        float weight = 30;
        float optimal = 50;

        public string Guid
        {
            get
            {
                return guid;
            }

            set
            {
                guid = value;
            }
        }

        internal ConcurrentBag<SemanticNode> Connections
        {
            get
            {
                return connections;
            }

            set
            {
                connections = value;
            }
        }

        internal ConcurrentBag<CloudNode> GetDocNode()
        {
            if (!cloudNodes.Keys.Contains(CloudNode.NODETYPE.DOC)) {
                return null;
            }
            return cloudNodes[CloudNode.NODETYPE.DOC];
        }
        internal ConcurrentBag<CloudNode> GetWordNode()
        {
            if (!cloudNodes.Keys.Contains(CloudNode.NODETYPE.WORD))
            {
                return null;
            }
            return cloudNodes[CloudNode.NODETYPE.WORD];
        }
        public string Semantic
        {
            get
            {
                return semantic;
            }

            set
            {
                semantic = value;
            }
        }

        public User Owner
        {
            get
            {
                return owner;
            }

            set
            {
                owner = value;
            }
        }

        public float X
        {
            get
            {
                return x;
            }

            set
            {
                if (value > 0 && value < SecondaryScreen.WIDTH)
                {
                    x = value;
                }
            }
        }

        public float Y
        {
            get
            {
                return y;
            }

            set
            {
                if (value > 0 && value < SecondaryScreen.HEIGHT)
                {
                    y = value;
                }
            }
        }

        public float Vx
        {
            get
            {
                return vx;
            }

            set
            {
                vx = value;
            }
        }

        public float Vy
        {
            get
            {
                return vy;
            }

            set
            {
                vy = value;
            }
        }

        public float Weight
        {
            get
            {
                return weight;
            }

            set
            {
                weight = value;
            }
        }

        public int H
        {
            get
            {
                return h;
            }

            set
            {
                h = value;
            }
        }

        public int S
        {
            get
            {
                return s;
            }

            set
            {
                s = value;
            }
        }

        public int V
        {
            get
            {
                return v;
            }

            set
            {
                v = value;
            }
        }

        public float Optimal
        {
            get
            {
                return optimal;
            }

            set
            {
                optimal = value;
            }
        }
        internal void RemoveWordNode()
        {
            ConcurrentBag<CloudNode> delete = new ConcurrentBag<CloudNode>();
            cloudNodes.TryRemove(CloudNode.NODETYPE.WORD, out delete);
        }

        internal void RemoveCloudNode(CloudNode node)
        {
            this.cloudNodes[node.Type].TryTake(out node);
        }

        public void Connect(SemanticNode node)
        {
            this.connections.Add(node);
        }

        public void AddCloudNode(CloudNode node)
        {
            if (!cloudNodes.Keys.Contains(node.Type))
            {
                cloudNodes.TryAdd(node.Type, new ConcurrentBag<CloudNode>());
            }
            this.cloudNodes[node.Type].Add(node);
        }
    }
}
