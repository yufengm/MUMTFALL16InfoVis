using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Input;

namespace CoLocatedCardSystem.CollaborationWindow.TouchModule
{
    class TouchList
    {
        Dictionary<uint, Touch> list = new Dictionary<uint, Touch>();

        public Dictionary<uint, Touch> List
        {
            get
            {
                return list;
            }
        }

        /// <summary>
        /// Add a touch point to the list;
        /// </summary>
        /// <param name="position"></param>
        /// <param name="sender"></param>
        /// <param name="type"></param>
        internal Touch AddTouchPoint(PointerPoint localPoint, PointerPoint globalPoint, object sender, Type type)
        {
            uint touchID = localPoint.PointerId;
            Touch touch = null;
            if (!list.Keys.Contains(touchID))
            {
                touch = new Touch();
                touch.Init(localPoint, globalPoint, sender, type);
                list.Add(localPoint.PointerId, touch);
                //Debug.WriteLine(localPoint.Position.ToString() + "\t" + globalPoint.Position.ToString() + "\t" + type.ToString());
            }
            return touch;
        }
        /// <summary>
        /// Update a touch point
        /// </summary>
        /// <param name="point"></param>
        internal void UpdateTouchPoint(PointerPoint localPoint, PointerPoint globalPoint)
        {
            uint touchID = localPoint.PointerId;
            if (list.Keys.Contains(touchID))
            {
                list[touchID].UpdateTouchPoint(localPoint, globalPoint);
            }
        }
        /// <summary>
        /// Remove a Touchpoint from the list
        /// </summary>
        /// <param name="touchID"></param>
        /// <returns></returns>
        internal Touch RemoveTouchPoint(PointerPoint localPoint, PointerPoint globalPoint)
        {
            uint touchID = localPoint.PointerId;
            Touch removedTouch = null;
            if (list.Keys.Contains(touchID))
            {
                removedTouch = list[touchID].End(localPoint, globalPoint);
                list.Remove(touchID);
            }
            return removedTouch;
        }

        /// <summary>
        /// Delete all the touches from the list.
        /// </summary>
        internal void Clear()
        {
            list.Clear();
        }

        internal Touch[] GetTouch()
        {
            return list.Values.ToArray();
        }
    }
}
