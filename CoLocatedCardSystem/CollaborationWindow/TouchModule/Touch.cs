using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml;

namespace CoLocatedCardSystem.CollaborationWindow.TouchModule
{
    public class Touch
    {
        uint touchID;
        Point currentLocalPoint;//The local coordination of the touch point
        Point currentGlobalPoint;//The global coordination of the touch point
        Point startPoint;//The position where the touch starts. Global cooridnation
        Point endPoint;//The position where the touch ends. Global cooridnation
        object sender;// The object which fire the touch
        Type type;// The type of the object
        DateTime startTime;//The start time stamp when the touch starts
        DateTime endTime;//The end time stamp when the touch ends
        TOUCH_STATUS touchStatus=TOUCH_STATUS.DEFAULT;//the status of the touch
        public uint TouchID
        {
            get
            {
                return touchID;
            }
        }

        public Point StartPoint
        {
            get
            {
                return startPoint;
            }
        }

        public Point EndPoint
        {
            get
            {
                return endPoint;
            }
        }

        public object Sender
        {
            get
            {
                return sender;
            }
        }

        public Type Type
        {
            get
            {
                return type;
            }
        }

        public DateTime StartTime
        {
            get
            {
                return startTime;
            }
        }

        public DateTime EndTime
        {
            get
            {
                return endTime;
            }
        }

        public Point CurrentLocalPoint
        {
            get
            {
                return currentLocalPoint;
            }
        }

        public Point CurrentGlobalPoint
        {
            get
            {
                return currentGlobalPoint;
            }
        }

        /// <summary>
        /// Construct the touch point
        /// </summary>
        /// <param name="localPoint"></param>
        /// <param name="sender"></param>
        /// <param name="type"></param>
        public void Init(PointerPoint localPoint, PointerPoint globalPoint, object sender, Type type)
        {
            this.touchID = localPoint.PointerId;
            this.sender = sender;
            this.type = type;
            this.currentLocalPoint = localPoint.Position;
            this.currentGlobalPoint = globalPoint.Position;
            this.startPoint = globalPoint.Position;
            this.startTime = DateTime.Now;
            this.touchStatus = TOUCH_STATUS.DOWN;
        }
        /// <summary>
        /// Generate a copy of the touch List
        /// </summary>
        /// <returns></returns>
        internal Touch Copy()
        {
            Touch newTouch = new Touch();
            newTouch.touchID = this.touchID;
            newTouch.startPoint = this.startPoint;
            newTouch.endPoint = this.endPoint;
            newTouch.sender = this.sender;
            newTouch.type = this.type;
            newTouch.startTime = this.startTime;
            newTouch.endTime = this.endTime;
            newTouch.currentLocalPoint = this.currentLocalPoint;
            newTouch.currentGlobalPoint = this.currentGlobalPoint;
            newTouch.touchStatus = this.touchStatus;
            return newTouch;
        }

        /// <summary>
        /// Update a touch point
        /// </summary>
        /// <param name="touchPoint"></param>
        internal void UpdateTouchPoint(PointerPoint localPoint, PointerPoint globalPoint) {
            currentLocalPoint = localPoint.Position;
            currentGlobalPoint = globalPoint.Position;
            this.touchStatus = TOUCH_STATUS.MOVE;
        }
        /// <summary>
        /// Call this method when the finger leave the screen.
        /// </summary>
        /// <param name="touchPoint"></param>
        internal Touch End(PointerPoint localPoint, PointerPoint globalPoint)
        {
            currentLocalPoint = localPoint.Position;
            currentGlobalPoint = globalPoint.Position;
            this.endPoint = localPoint.Position;
            this.endTime = DateTime.Now;
            this.touchStatus = TOUCH_STATUS.RELEASED;
            return this;
        }

        internal TOUCH_STATUS GetStatus() {
            return touchStatus;
        }
        /// <summary>
        /// Get how long the touch has been moved on the screen, in second.
        /// </summary>
        /// <returns></returns>
        internal double GetLife() {
            //To do
            return 0;
        }
        /// <summary>
        /// Get the distance the touch has moved. In global coordination. Unit is pixel.
        /// </summary>
        /// <returns></returns>
        internal double GetTouchDistance() {
            //To do
            return 0;
        }
    }
}
