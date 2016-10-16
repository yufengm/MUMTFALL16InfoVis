using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.CollaborationWindow.GestureModule
{
    class GestureListenerList
    {
        Dictionary<Type, GestureListener> list = new Dictionary<Type, GestureListener>();
        GestureListenerController gestureListenerController = null;
        internal GestureListenerList(GestureListenerController controller) {
            this.gestureListenerController = controller;
        }

        /// <summary>
        /// Get the listener instance. If the listener object does not exist, create one and add it to the list.
        /// All same gestures share one gesture listener.
        /// </summary>
        /// <param name="listenerType"></param>
        /// <returns></returns>
        internal GestureListener GetListener(Type listenerType)
        {
            if (!list.Keys.Contains(listenerType))
            {
                GestureListener listener = null;
                if (listenerType == typeof(SortingListener))
                {
                    listener = new SortingListener(gestureListenerController);
                }
                else if (listenerType == typeof(DeletingBoxListener))
                {
                    listener = new DeletingBoxListener(gestureListenerController);
                }
                else if (listenerType == typeof(AttachingListener))
                {
                    listener = new AttachingListener(gestureListenerController);
                }
                list.Add(listenerType, listener);
            }
            return list[listenerType];
        }
        /// <summary>
        /// Remove all gesture listeners.
        /// </summary>
        internal void Clear()
        {
            list.Clear();
        }
    }
}
