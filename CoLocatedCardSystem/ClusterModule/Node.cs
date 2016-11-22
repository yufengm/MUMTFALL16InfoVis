using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.ClusterModule
{
    class Node
    {
        NODETYPE type = NODETYPE.WORD;
        string owner = "";
        string color = "";
        string text="text";
        string stemmedText;
        double weight =10;
        double x = 0, y = 0;
        bool highlight=false;
        SemanticCluster cluster;
        string[] connections;

        public string Text
        {
            get
            {
                return text;
            }

            set
            {
                text = value;
            }
        }

        public double Weight
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

        public double X
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

        public double Y
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

        public bool Highlight
        {
            get
            {
                return highlight;
            }

            set
            {
                highlight = value;
            }
        }



        public string[] Connections
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

        public string Color
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

        internal SemanticCluster Cluster
        {
            get
            {
                return cluster;
            }

            set
            {
                cluster = value;
            }
        }
    }
}
