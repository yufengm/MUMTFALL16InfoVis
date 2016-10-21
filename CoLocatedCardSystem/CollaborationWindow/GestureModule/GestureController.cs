using CoLocatedCardSystem.CollaborationWindow.TouchModule;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System.Threading;

namespace CoLocatedCardSystem.CollaborationWindow.GestureModule
{
    class GestureController
    {
        CentralControllers controllers;
        AttachingGesture attachingGesture;
        RemoveAttachingGesture removeAttachingGesture;

        public CentralControllers Controllers
        {
            get
            {
                return controllers;
            }

            set
            {
                controllers = value;
            }
        }

        internal AttachingGesture AttachingGesture
        {
            get
            {
                return attachingGesture;
            }

            set
            {
                attachingGesture = value;
            }
        }

        internal RemoveAttachingGesture RemoveAttachingGesture
        {
            get
            {
                return removeAttachingGesture;
            }

            set
            {
                removeAttachingGesture = value;
            }
        }

        public GestureController(CentralControllers ctrls)
        {
            this.controllers = ctrls;
        }
        public void Init()
        {
            attachingGesture = new AttachingGesture(this);
            removeAttachingGesture = new RemoveAttachingGesture(this);
        }

        public void Deinit()
        {
        }
    }
}
