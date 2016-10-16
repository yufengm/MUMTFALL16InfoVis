using CoLocatedCardSystem.CollaborationWindow.InteractionModule;
using CoLocatedCardSystem.CollaborationWindow.TouchModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.CollaborationWindow.GestureModule
{
    class AttachingGesture: Gesture
    {
        public AttachingGesture(GestureController gtCtrler) : base(gtCtrler)
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
            AttachingGesture gesture = null;
            foreach (Touch touch in touchList)
            {
                if (touch.Sender is Card && !usedTouches.Contains(touch))
                {
                    Card card = touch.Sender as Card;
                    if (card == null)
                    {
                        return;
                    }
                    GlowGroup[] glowGroup = controllers.GlowController.GetIntersectedGroup(card);
                    if (glowGroup != null&&glowGroup.Length>0)
                    {
                        foreach (Touch otherTouches in touchList)
                        {
                            if (touch.Sender == otherTouches.Sender && !usedTouches.Contains(otherTouches))
                            {
                                usedTouches.Add(otherTouches);
                            }
                        }
                        gesture = new AttachingGesture(controllers.GestureController);
                        gesture.AssociatedTouches = usedTouches;
                        gesture.AssociatedObjects = new List<object>() { card, glowGroup };
                        gesture.AssociatedObjectTypes = new List<Type>() { typeof(Card), typeof(GlowGroup[]) };
                        AttachingListener listener = controllers.ListenerController.GetListener(typeof(AttachingListener)) as AttachingListener;
                        RegisterListener(gesture, listener);// Register the gesture and add the gesture to the gesture list.
                        return;
                    }                 
                }
            }
            foreach (Touch touch in usedTouches)
            {
                touchList.Remove(touch);
            }
        }

        /// <summary>
        /// Update the glow when pointer up
        /// </summary>
        /// <param name="cardID"></param>
        internal void PointerUpCardGlow(string cardID)
        {
            
        }
        /// <summary>
        /// Check the touches to decide whether the gesture enters a terminated or failed status.
        /// </summary>
        /// <returns>true: continue, false: terminated or failed</returns>
        protected override async Task<bool> CheckContinue()
        {
            if (AssociatedTouches != null || AssociatedTouches.Count() > 0)
            {
                GlowGroup[] glowList = AssociatedObjects[1] as GlowGroup[];
                Card card = AssociatedObjects[0] as Card;
                foreach (GlowGroup gg in glowList)
                {
                    if (!gg.IsIntersect(card))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// Check the status to decide if the gesture is terminated or failed
        /// </summary>
        /// <returns>true: terminate, false: failed</returns>
        protected override bool CheckTerminate()
        {
            if (AssociatedTouches == null || AssociatedTouches.Count() == 0)
            {
                GlowGroup[] glowList = AssociatedObjects[1] as GlowGroup[];
                Card card = AssociatedObjects[0] as Card;
                foreach (GlowGroup gg in glowList)
                {
                    if (!gg.IsIntersect(card))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
    }
}
