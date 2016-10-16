using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.CollaborationWindow.GestureModule
{
    class GestureListener
    {
        protected GestureListenerController gestureListenerController;
        public GestureListener(GestureListenerController gCtrlers)
        {
            this.gestureListenerController = gCtrlers;
        }
        /// <summary>
        /// Callback method for the onRegistered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void RegisterGesture(object sender, GestureEventArgs e) {

        }
        /// <summary>
        /// Callback method for the onContinued
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void ContinueGesture(object sender, GestureEventArgs e)
        {

        }
        /// <summary>
        /// Callback method for the onTerminated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void TerminateGesture(object sender, GestureEventArgs e)
        {
            
        }
        /// <summary>
        /// Callback method for the onFailed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void FailGesture(object sender, GestureEventArgs e)
        {

        }
    }
}
