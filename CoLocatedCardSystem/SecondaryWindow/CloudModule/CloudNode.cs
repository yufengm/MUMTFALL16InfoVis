using CoLocatedCardSystem.SecondaryWindow.SemanticModule;

namespace CoLocatedCardSystem.SecondaryWindow.CloudModule
{
    class CloudNode
    {
       internal enum ACTIVELEVEL
        {
            INACTIVE, SEARCHED, ONTABLE
        }
        internal enum NODETYPE
        {
            PICTURE, WORD, DOC
        }
        string guid = "";
        NODETYPE type = NODETYPE.DOC;
        string cloudText = "test";
        string image = null;
        string stemmedText = "test";
        string owner = "";
        float x = 0;
        float y = 0;
        float vx = 0;
        float vy = 0;
        SemanticNode semanticNode = null;
        float weight = 15;
        float w = 15;
        float h = 15;
        byte alpha = 100;
        #region getter
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

        public byte Alpha
        {
            get
            {
                return alpha;
            }

            set
            {
                alpha = value;
            }
        }
        #endregion

        internal void SetActive(ACTIVELEVEL level)
        {
            switch (level)
            {
                case ACTIVELEVEL.INACTIVE:
                    this.alpha = 100;
                    break;
                case ACTIVELEVEL.SEARCHED:
                    this.alpha = 150;
                    break;
                case ACTIVELEVEL.ONTABLE:
                    this.alpha = 255;
                    break;
            }
        }
    }
}
