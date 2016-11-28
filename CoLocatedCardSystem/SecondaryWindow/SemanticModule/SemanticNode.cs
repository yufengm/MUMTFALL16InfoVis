using CoLocatedCardSystem.CollaborationWindow;
using CoLocatedCardSystem.SecondaryWindow.CloudModule;
using System.Collections.Concurrent;
using Windows.UI;
using System;
using CoLocatedCardSystem.CollaborationWindow.InteractionModule;

namespace CoLocatedCardSystem.SecondaryWindow.SemanticModule
{
    class SemanticNode
    {
        string guid = "";
        ConcurrentBag<SemanticNode> connections = new ConcurrentBag<SemanticNode>();
        string semantic = "";
        int h = 0, s = 0, v = 0;
        float x = 0;
        float y = 0;
        float vx = 0;
        float vy = 0;
        float weight = 30;
        float optimal = 50;
        bool isRoot = false;
        UserActionOnDoc userActionOnDoc;
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

        public bool IsRoot
        {
            get
            {
                return isRoot;
            }

            set
            {
                isRoot = value;
            }
        }

        internal UserActionOnDoc UserActionOnDoc
        {
            get
            {
                return userActionOnDoc;
            }

            set
            {
                userActionOnDoc = value;
            }
        }

        public void Connect(SemanticNode node)
        {
            this.connections.Add(node);
        }
    }
}
