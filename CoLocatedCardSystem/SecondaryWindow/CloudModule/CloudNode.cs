using CoLocatedCardSystem.ClusterModule;
using CoLocatedCardSystem.SecondaryWindow.SemanticModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace CoLocatedCardSystem.SecondaryWindow.CloudModule
{
    class CloudNode
    {
        string guid = "";
        NODETYPE type = NODETYPE.DOC;
        string cloudText = "test";
        string image = null;
        string stemmedText = "test";
        string owner = "";
        Color nodeColor = Colors.White;
        float x = 0;
        float y = 0;
        float vx = 0;
        float vy = 0;
        SemanticNode semanticNode = null;
        float weight = 15;
        float w = 15;
        float h = 15;

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

        internal NODETYPE Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
            }
        }

        public string CloudText
        {
            get
            {
                return cloudText;
            }

            set
            {
                cloudText = value;
            }
        }

        public string Image
        {
            get
            {
                return image;
            }

            set
            {
                image = value;
            }
        }

        public string StemmedText
        {
            get
            {
                return stemmedText;
            }

            set
            {
                stemmedText = value;
            }
        }

        public string Owner
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

        public Color NodeColor
        {
            get
            {
                return nodeColor;
            }

            set
            {
                nodeColor = value;
            }
        }



        internal SemanticNode SemanticNode
        {
            get
            {
                return semanticNode;
            }

            set
            {
                semanticNode = value;
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
                x = value;
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
                y = value;
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

        public float W
        {
            get
            {
                return w;
            }

            set
            {
                w = value;
            }
        }

        public float H
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
    }
}
