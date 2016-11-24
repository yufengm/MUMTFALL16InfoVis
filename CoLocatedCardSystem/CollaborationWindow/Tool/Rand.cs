using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.CollaborationWindow.Tool
{
    class Rand
    {
        static Random rand = new Random();
        internal static int Next(int upbound) {
            return rand.Next(upbound);
        }
    }
}
