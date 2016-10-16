using CoLocatedCardSystem.CollaborationWindow.TouchModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.CollaborationWindow.GestureModule
{
    class GestureEventArgs:EventArgs
    {
        private Touch[] touches;
        private object[] senders;
        private Type[] types;

        public Touch[] Touches
        {
            get
            {
                return touches;
            }

            set
            {
                touches = value;
            }
        }

        public object[] Senders
        {
            get
            {
                return senders;
            }

            set
            {
                senders = value;
            }
        }

        public Type[] Types
        {
            get
            {
                return types;
            }

            set
            {
                types = value;
            }
        }
    }
}
