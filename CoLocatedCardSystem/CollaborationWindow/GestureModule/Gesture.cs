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
        private List<Touch> associatedTouches;
        private List<object> associatedObjects;
        private List<Type> associatedObjectTypes;
        internal List<Touch> AssociatedTouches
        {
            get
            {
                return associatedTouches;
            }

            set
            {
                associatedTouches = value;
            }
        }
        internal List<object> AssociatedObjects
        {
            get
            {
                return associatedObjects;
            }

            set
            {
                associatedObjects = value;
            }
        }
        public List<Type> AssociatedObjectTypes
        {
            get
            {
                return associatedObjectTypes;
            }

            set
            {
                associatedObjectTypes = value;
            }
        }
        public GestureStatus Status
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
        GestureController gestureController;
        //Gesture Events.
        public event EventHandler<GestureEventArgs> GestureRegistered;
        public event EventHandler<GestureEventArgs> GestureContinued;
        public event EventHandler<GestureEventArgs> GestureTerminated;
        public event EventHandler<GestureEventArgs> GestureFailed;
        GestureStatus status = GestureStatus.DEFAULT;
        public Gesture(GestureController gtCtrler)
        {
            this.gestureController = gtCtrler;
        }
      
        protected virtual void OnRegistered(GestureEventArgs e)
        {
            GestureRegistered?.Invoke(this, e);
            this.status = GestureStatus.REGISTERED;
        }

        protected virtual void OnContinued(GestureEventArgs e)
        {
            GestureContinued?.Invoke(this, e);
            this.status = GestureStatus.CONTINUE;
        }

        protected virtual void OnTerminated(GestureEventArgs e)
        {
            GestureTerminated?.Invoke(this, e);
            this.status = GestureStatus.TERMINATED;
        }

        protected virtual void OnFailed(GestureEventArgs e)
        {
            GestureFailed?.Invoke(this, e);
            this.status = GestureStatus.FAILED;
        }
        /// <summary>
        /// Based on the new touch list, update the touches associated with the gesture.
        /// The associated touch points will be removed from the touch list
        /// </summary>
        /// <param name="touchList"></param>
        internal void UpdateAssociatedTouches(List<Touch> touchList)
        {
            List<Touch> removedTouches = new List<Touch>();
            for (int aSize = associatedTouches.Count(), i = 0; i < aSize; i++) {
                bool isLive = false;
                for (int nSize = touchList.Count(), j = 0; j < nSize; j++)
                {
                    //if the associated touch is in the new touch list, update the associated touch
                    if (associatedTouches[i].TouchID == touchList[j].TouchID) {
                        isLive = true;
                        associatedTouches[i] = touchList[j];
                    }
                }
                if (!isLive) {
                    removedTouches.Add(associatedTouches[i]);
                }
            }
            //Remove the touch from the associated touch list if not exist in the touchList
            foreach (Touch t in removedTouches) {
                associatedTouches.Remove(t);
            }
            //Remove the touch used by this gesture from the associated touchlist
            List<Touch> touchToRemove = new List<Touch>();
            foreach (Touch touch in touchList) {
                var usedTouch= associatedTouches.Where(t => t.TouchID == touch.TouchID);
                if (usedTouch.Count() != 0) {
                    touchToRemove.Add(touch);
                }
            }
            foreach (Touch touch in touchToRemove) {
                touchList.Remove(touch);
            }
        }
        /// <summary>
        /// Register a listener to the gesture. And add the gesture to the gesture list
        /// </summary>
        /// <param name="gesture"></param>
        /// <param name="listener"></param>
        protected static void RegisterListener(Gesture gesture, GestureListener listener) {
            gesture.GestureRegistered += listener.RegisterGesture;
            gesture.GestureContinued += listener.ContinueGesture;
            gesture.GestureTerminated += listener.TerminateGesture;
            gesture.GestureFailed += listener.FailGesture;
            GestureEventArgs args = new GestureEventArgs();
            args.Touches = gesture.AssociatedTouches.ToArray();
            args.Senders = gesture.AssociatedObjects.ToArray();
            args.Types = gesture.AssociatedObjectTypes.ToArray();
            gesture.gestureController.AddGesture(gesture);
            gesture.OnRegistered(args);
        }
        /// <summary>
        /// Update the gesture. Check the touch status to detect continue, terminate and fail status
        /// </summary>
        public virtual async void Update()
        {
            GestureEventArgs args = new GestureEventArgs();
            args.Touches = associatedTouches.ToArray();
            args.Senders = associatedObjects.ToArray();
            bool isContinue = await CheckContinue();
            if (isContinue)
            {
                OnContinued(args);
            }
            else {
                if (CheckTerminate())
                {
                    OnTerminated(args);
                }
                else {
                    OnFailed(args);
                }
                gestureController.RemoveGesture(this);
            }
        }
        /// <summary>
        /// Check the touches to decide whether the gesture enters a terminated or failed status.
        /// </summary>
        /// <returns>true: continue, false: terminated or failed</returns>
        protected virtual async Task<bool> CheckContinue() {
            return true;
        }
        /// <summary>
        /// Check the status to decide if the gesture is terminated or failed
        /// </summary>
        /// <returns>true: terminate, false: failed</returns>
        protected virtual bool CheckTerminate() {
            return true;
        }
    }
}
