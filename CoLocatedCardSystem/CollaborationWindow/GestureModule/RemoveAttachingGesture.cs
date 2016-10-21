using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoLocatedCardSystem.CollaborationWindow.TouchModule;
using CoLocatedCardSystem.CollaborationWindow.InteractionModule;

namespace CoLocatedCardSystem.CollaborationWindow.GestureModule
{
    class RemoveAttachingGesture : Gesture
    {
        public RemoveAttachingGesture(GestureController gtCtrler) : base(gtCtrler)
        {
        }
        internal override void Detect(Touch[] touchList, Touch[] targetList)
        {
            base.Detect(touchList, targetList);
            try
            {
                Touch newTouch = targetList[0];
                if (newTouch.Sender is DocumentCard)
                {
                    //If some other touch on the same card, don't perform the action
                    for (int i = 0, size = touchList.Length; i < size; i++)
                    {
                        if (touchList[i].Sender.Equals(newTouch.Sender)&&touchList[i].TouchID!=newTouch.TouchID)
                        {
                            return;
                        }
                    }
                    Card card = newTouch.Sender as Card;
                    gestureController.Controllers.GlowController.DisconnectOneCardWithGroups(card.CardID);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + "\n" + ex.StackTrace.ToString());
            }
        }
    }
}
