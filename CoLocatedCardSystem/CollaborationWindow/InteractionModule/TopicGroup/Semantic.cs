using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    class Semantic//The dots
    {
        bool created = false;
        string cardID;
        string docID;
        User owner;

        public bool Created
        {
            get
            {
                return created;
            }

            set
            {
                created = value;
            }
        }

        public string CardID
        {
            get
            {
                return cardID;
            }

            set
            {
                cardID = value;
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
    }
}
