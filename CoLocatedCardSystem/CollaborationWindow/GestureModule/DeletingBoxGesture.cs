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
    class DeletingBoxGesture:Gesture
    {
        public DeletingBoxGesture(GestureController gtCtrler) : base(gtCtrler)
        {
        }

        /// <summary>
        /// Detect the sortingbox deleting gesture (Drag and drop the box to the delete box on the menubar). 
        /// The touches in the sorting gesture will be removed from the touchList
        /// </summary>
        /// <param name="touchList"></param>
        /// <returns></returns>
        internal override async void Detect(Touch[] touchList, Touch[] targetList)
        {
            List<Touch> usedTouches = new List<Touch>();
            foreach (Touch touch in touchList)
            {
                if (touch.Type == typeof(SortingBox) && !usedTouches.Contains(touch))
                {
                    SortingBox box = touch.Sender as SortingBox;
                    MenuBar[] bars = gestureController.Controllers.MenuLayerController.GetAllMenuBars();
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
                        }
                    }
                }
            }
        }
    }
}
