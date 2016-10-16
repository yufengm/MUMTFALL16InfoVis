using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    enum CardPositionStatus
    {
        INACTIVE,//card is not on table
        DELETED,//card is in delete zone
        ON_TABLE// card is on table
    }
}
