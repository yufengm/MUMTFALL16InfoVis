using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.CollaborationWindow.GestureModule
{
    class GestureList
    {
        List<Gesture> list = new List<Gesture>();
        /// <summary>
        /// Add a gesture to the gesture list
        /// </summary>
        /// <param name="gesture"></param>
        internal void AddGesture(Gesture gesture)
        {
            if (!list.Contains(gesture))
            {
                list.Add(gesture);
            }
        }
        /// <summary>
        /// Remove a gesture from the gesture list
        /// </summary>
        /// <param name="gesture"></param>
        internal void RemoveGesture(Gesture gesture)
        {
            if (list.Contains(gesture))
            {
                list.Remove(gesture);
            }
        }
        /// <summary>
        /// Get all gestures
        /// </summary>
        /// <returns></returns>
        internal Gesture[] GetGesture() {
            return list.ToArray();
        }
        /// <summary>
        /// Remove all the gestures from the gesture list.
        /// </summary>
        internal void Clear() {
            list.Clear();
        }
    }
}
