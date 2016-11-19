using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.ClusterModule
{
    class ClusterWord
    {
        int type = 0;//0 for text, 1 for pic
        string owner = "";
        string color = "";
        string text="text";
        string stemmedText;
        double weight =10;
        double x = 0, y = 0;
        bool highlight=false;
        string group="";
        string[] connections;
        public int Type
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

        public string Group
        {
            get
            {
                return group;
            }

            set
            {
                group = value;
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
    }
}
