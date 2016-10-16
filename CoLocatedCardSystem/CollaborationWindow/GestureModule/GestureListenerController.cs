using CoLocatedCardSystem.CollaborationWindow.InteractionModule;
using CoLocatedCardSystem.CollaborationWindow.Layers.SortingBox_Layer;
using System;

namespace CoLocatedCardSystem.CollaborationWindow.GestureModule
{
    class GestureListenerController
    {
        CentralControllers controllers;
        GestureListenerList list;

        public CentralControllers Controllers
        {
            get
            {
                return controllers;
            }
        }

        public GestureListenerController(CentralControllers ctrls) {
            this.controllers = ctrls;
        }
        /// <summary>
        /// Initialize the gesture listener controller
        /// </summary>
        public void Init() {
            list = new GestureListenerList(this);
        }
        public void Deinit() {
            list.Clear();
        }
        /// <summary>
        /// Remove the sorting box.
        /// </summary>
        /// <param name="box"></param>
        internal void RemoveSortingBox(SortingBox box)
        {
            controllers.SortingBoxController.DestroyBox(box);
        }

        /// <summary>
        /// Return a listener object. All same gestures share one listener object
        /// </summary>
        /// <returns></returns>
        public GestureListener GetListener(Type listenerType) {
            return list.GetListener(listenerType);
        }
    }
}
