using CoLocatedCardSystem.SecondaryWindow.CloudModule;
using System.Collections.Concurrent;
using Windows.UI;

namespace CoLocatedCardSystem.SecondaryWindow.SemanticModule
{
    class SemanticNode
    {
        string guid = "";
        ConcurrentBag<SemanticNode> connections = new ConcurrentBag<SemanticNode>();
        ConcurrentBag<CloudNode> cloudNodes = new ConcurrentBag<CloudNode>();
        string semantic = "";
        User owner = User.NONE;
        Color color = Colors.White;
        float x = 0;
        float y = 0;
        float vx = 0;
        float vy = 0;
        float weight = 10;

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

        internal ConcurrentBag<CloudNode> CloudNodes
        {
            get
            {
                return cloudNodes;
            }

            set
            {
                cloudNodes = value;
            }
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

        public Color Color
        {
            get
            {
                return color;
            }

            set
            {
                color = value;
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
                if (value > 0 && value < SecondaryScreen.WIDTH) {
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

        internal void RemoveCloudNode(CloudNode node)
        {
            this.cloudNodes.TryTake(out node);
        }

        public void Connect(SemanticNode node) {
            this.connections.Add(node);
        }

        public void AddCloudNode(CloudNode node) {
            this.cloudNodes.Add(node);
        }
    }
}
