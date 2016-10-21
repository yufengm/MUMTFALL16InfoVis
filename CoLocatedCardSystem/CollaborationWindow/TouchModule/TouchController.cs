using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Input;

namespace CoLocatedCardSystem.CollaborationWindow.TouchModule
{
    class TouchController
    {
        CentralControllers controllers;
        TouchList list;
        bool isMouseEnabled = true;
        bool isPenEnabled = true;
        public TouchController(CentralControllers ctrls)
        {
            this.controllers = ctrls;
        }
        /// <summary>
        /// Initialize the TouchController
        /// </summary>
        public void Init()
        {
            list = new TouchList();
        }
        /// <summary>
        /// Deinitialize the TouchController
        /// </summary>
        public void Deinit()
        {
            if (list != null)
            {
                list.Clear();
            }
        }
        /// <summary>
        /// Add a new touch to the touch list. Two parameters control the pointertye. 
        /// Touch will always be captured. 
        /// Mouse and Pen can be configured.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="sender"></param>
        /// <param name="type"></param>
        public void TouchDown(PointerPoint localPoint, PointerPoint globalPoint, object sender, Type type)
        {
            Touch newTouch = null;
            switch (localPoint.PointerDevice.PointerDeviceType)
            {
                case Windows.Devices.Input.PointerDeviceType.Touch:
                    newTouch=list.AddTouchPoint(localPoint, globalPoint, sender, type);
                    break;
                case Windows.Devices.Input.PointerDeviceType.Mouse:
                    if (isMouseEnabled)
                        newTouch=list.AddTouchPoint(localPoint, globalPoint, sender, type);
                    break;
                case Windows.Devices.Input.PointerDeviceType.Pen:
                    if (isPenEnabled)
                        newTouch=list.AddTouchPoint(localPoint, globalPoint, sender, type);
                    break;
            }
            DetectTouchDownGesture(newTouch);
            //For debug, output all touched items
            //if(newTouch!=null)
            //System.Diagnostics.Debug.WriteLine(type.Name+ "\t" + newTouch.StartPoint.ToString()+"\t"+ newTouch.StartTime.ToString());
        }

        private void DetectTouchDownGesture(Touch newTouch)
        {
            Touch[] touchList = list.GetTouch();
            Touch[] newTouchList = new Touch[] { newTouch };
            controllers.GestureController.RemoveAttachingGesture.Detect(touchList, newTouchList);
        }

        /// <summary>
        /// Update the touch points
        /// </summary>
        /// <param name="point"></param>
        public void TouchMove(PointerPoint localPoint, PointerPoint globalPoint)
        {
            lock (list.List)
            {
                switch (localPoint.PointerDevice.PointerDeviceType)
                {
                    case Windows.Devices.Input.PointerDeviceType.Touch:
                        list.UpdateTouchPoint(localPoint, globalPoint);
                        break;
                    case Windows.Devices.Input.PointerDeviceType.Mouse:
                        if (isMouseEnabled)
                            list.UpdateTouchPoint(localPoint, globalPoint);
                        break;
                    case Windows.Devices.Input.PointerDeviceType.Pen:
                        if (isPenEnabled)
                            list.UpdateTouchPoint(localPoint, globalPoint);
                        break;
                }
            }
        }
        /// <summary>
        /// Release the touch points
        /// </summary>
        /// <param name="point"></param>
        public void TouchUp(PointerPoint localPoint, PointerPoint globalPoint)
        {
            Touch removedTouch=null;
            switch (localPoint.PointerDevice.PointerDeviceType)
            {
                case Windows.Devices.Input.PointerDeviceType.Touch:
                    removedTouch=list.RemoveTouchPoint(localPoint, globalPoint);
                    break;
                case Windows.Devices.Input.PointerDeviceType.Mouse:
                    if (isMouseEnabled)
                        removedTouch=list.RemoveTouchPoint(localPoint, globalPoint);
                    break;
                case Windows.Devices.Input.PointerDeviceType.Pen:
                    if (isPenEnabled)
                        removedTouch=list.RemoveTouchPoint(localPoint, globalPoint);
                    break;
            }
            if(removedTouch!=null)
                DetectTouchUpGesture(removedTouch);
        }

        private void DetectTouchUpGesture(Touch removedTouch)
        {
            Touch[] touchList = list.GetTouch();
            Touch[] removedTouchList = new Touch[] { removedTouch };
            controllers.GestureController.AttachingGesture.Detect(touchList, removedTouchList);
        }
    }
}
