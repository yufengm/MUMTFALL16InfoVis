using CoLocatedCardSystem.CollaborationWindow.DocumentModule;
using CoLocatedCardSystem.CollaborationWindow.InteractionModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace CoLocatedCardSystem.ClusterModule
{
    class ClusterDoc: Document 
    {
        CardStatus status = null;

        internal CardStatus Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
            }
        }
    }
}
