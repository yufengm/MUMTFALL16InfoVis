using CoLocatedCardSystem.CollaborationWindow.InteractionModule;
using CoLocatedCardSystem.CollaborationWindow.Layers;
using CoLocatedCardSystem.CollaborationWindow.Layers.Menu_Layer;
using CoLocatedCardSystem.CollaborationWindow.Layers.SortingBox_Layer;
using CoLocatedCardSystem.CollaborationWindow.TouchModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.CollaborationWindow.GestureModule
{
    class DeletingBoxGesture : Gesture
    {
        private GestureController gestureController;

        public DeletingBoxGesture(GestureController gestureController) : base(gestureController)
        {
            this.gestureController = gestureController;
        }
        /// <summary>
        /// Detect the sortingbox deleting gesture (Drag and drop the box to the delete box on the menubar). 
        /// The touches in the sorting gesture will be removed from the touchList
        /// </summary>
        /// <param name="touchList"></param>
        /// <returns></returns>
        internal static async Task Detect(List<Touch> touchList, CentralControllers controllers)
        {
            List<Touch> usedTouches = new List<Touch>();
            DeletingBoxGesture gesture = null;
            foreach (Touch touch in touchList)
            {
                if (touch.Type == typeof(SortingBox) && !usedTouches.Contains(touch))
                {
                    SortingBox box = touch.Sender as SortingBox;
                    MenuBar[] bars = controllers.MenuLayerController.GetAllMenuBars();
                    foreach (MenuBar bar in bars)
                    {
                        bool isIntersect = await bar.IsIntersectWithDelete(box.Position);
                        if (isIntersect)
                        {
                            foreach (Touch otherTouches in touchList)
                            {
                                if (touch.Sender == otherTouches.Sender && !usedTouches.Contains(otherTouches))
                                {
                                    usedTouches.Add(otherTouches);
                                }
                            }
                            gesture = new DeletingBoxGesture(controllers.GestureController);
                            gesture.AssociatedTouches = usedTouches;
                            gesture.AssociatedObjects = new List<object>() { box, bar };
                            gesture.AssociatedObjectTypes = new List<Type>() { typeof(SortingBox), typeof(MenuBar) };
                            DeletingBoxListener listener = controllers.ListenerController.GetListener(typeof(DeletingBoxListener)) as DeletingBoxListener;
                            RegisterListener(gesture, listener);// Register the gesture and add the gesture to the gesture list.
                        }
                    }
                }
            }
            foreach (Touch touch in usedTouches)
            {
                touchList.Remove(touch);
            }
        }

        /// <summary>
        /// Check the touches to decide whether the gesture enters a terminated or failed status.
        /// </summary>
        /// <returns>true: continue, false: terminated or failed</returns>
        protected override async Task<bool> CheckContinue()
        {
            if (AssociatedTouches == null || AssociatedTouches.Count() == 0)
            {
                return false;
            }
            bool isIntersect = await (AssociatedObjects[1] as MenuBar).IsIntersectWithDelete((AssociatedObjects[0] as SortingBox).Position);
            return isIntersect;
        }
        /// <summary>
        /// Check the status to decide if the gesture is terminated or failed.
        /// Terminate when all fingers on the object is released.
        /// </summary>
        /// <returns>true: terminate, false: failed</returns>
        protected override bool CheckTerminate()
        {
            if (AssociatedTouches == null || AssociatedTouches.Count() == 0)
            {
                return true;
            }
            return false;
        }
    }
}
