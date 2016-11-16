using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    class CardStatus
    {
        internal Type type;
        internal CardPositionStatus status;
        internal Point position;
        internal double rotation;
        internal double scale;
        internal string cardID;
        internal Point[] corners;
        internal User owner;
    }
}
