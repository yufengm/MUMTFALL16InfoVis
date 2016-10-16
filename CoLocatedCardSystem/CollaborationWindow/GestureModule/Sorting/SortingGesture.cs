using CoLocatedCardSystem.CollaborationWindow.InteractionModule;
using CoLocatedCardSystem.CollaborationWindow.Layers.Menu_Layer;
using CoLocatedCardSystem.CollaborationWindow.TouchModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.CollaborationWindow.GestureModule
{
    class SortingGesture : Gesture
    {
        public SortingGesture(GestureController gtCtrler) : base(gtCtrler)
        {
        }

        /// <summary>
        /// Detect the sorting gesture (Drag and drop the card to the sorting box). 
        /// The touches in the sorting gesture will be removed from the touchList
        /// </summary>
        /// <param name="touchList"></param>
        /// <returns></returns>
        internal static async Task Detect(List<Touch> touchList, CentralControllers controllers)
        {
            List<Touch> usedTouches = new List<Touch>();
            SortingGesture gesture = null;
            foreach (Touch touch in touchList)
            {
                if (touch.Sender is Card && touch.Type != typeof(ResultCard) && !usedTouches.Contains(touch))
                {
                    Card card = touch.Sender as Card;
                    SortingBox[] boxes = controllers.SortingBoxController.GetAllSortingBoxes();
                    foreach (SortingBox box in boxes)
                    {
                        bool isIntersect  = Coordination.IsIntersect(card.Position, box.Corners, true);
                        if (isIntersect)
                        {
                            foreach (Touch otherTouches in touchList)
                            {
                                if (touch.Sender == otherTouches.Sender && !usedTouches.Contains(otherTouches))
                                {
                                    usedTouches.Add(otherTouches);
                                }
                            }
                            gesture = new SortingGesture(controllers.GestureController);
                            gesture.AssociatedTouches = usedTouches;
                            gesture.AssociatedObjects = new List<object>() { card, box };
                            gesture.AssociatedObjectTypes = new List<Type>() { typeof(Card), typeof(SortingBox) };
                            SortingListener listener = controllers.ListenerController.GetListener(typeof(SortingListener)) as SortingListener;
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
            bool isIntersect = Coordination.IsIntersect((AssociatedObjects[0] as Card).Position,
                (AssociatedObjects[1] as SortingBox).Corners, true);
            return isIntersect;
        }
        /// <summary>
        /// Check the status to decide if the gesture is terminated or failed
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
