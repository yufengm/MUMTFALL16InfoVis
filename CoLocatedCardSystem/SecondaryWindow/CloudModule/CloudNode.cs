using CoLocatedCardSystem.CollaborationWindow;
using CoLocatedCardSystem.SecondaryWindow.SemanticModule;
using System.Collections.Generic;
using Windows.UI;
using System;
using CoLocatedCardSystem.CollaborationWindow.InteractionModule;

namespace CoLocatedCardSystem.SecondaryWindow.CloudModule
{
    class CloudNode
    {
        internal enum NODETYPE
        {
            PICTURE, WORD, DOC
        }
        string guid = "";// for docs, the id is the doc id + user name, for word, the id is doc+stemmedword
        string docID = "";
        NODETYPE type = NODETYPE.DOC;
        string cloudText = "test";
        string image = null;
        string stemmedText = "test";
        float x = 0;
        float y = 0;
        float vx = 0;
        float vy = 0;
        SemanticNode semanticNode = null;
        float weight = 10;
        float w = 20;
        float h = 20;
        UserActionOnDoc userActionOnDoc;
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

        public string DocID
        {
            get
            {
                return docID;
            }

            set
            {
                docID = value;
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
        #endregion
    }
}
