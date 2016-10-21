using CoLocatedCardSystem.CollaborationWindow.TouchModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.CollaborationWindow.GestureModule
{
    class Gesture
    {
        protected GestureController gestureController;
        public Gesture(GestureController gtCtrler)
        {
            this.gestureController = gtCtrler;
        }
        internal virtual void Detect(Touch[] touchList, Touch[] targetList)
        {
        }
    }
}
