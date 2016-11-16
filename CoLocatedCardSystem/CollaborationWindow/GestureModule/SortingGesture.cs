using CoLocatedCardSystem.CollaborationWindow.InteractionModule;
using CoLocatedCardSystem.CollaborationWindow.Layers.Menu_Layer;
using CoLocatedCardSystem.CollaborationWindow.TouchModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;

namespace CoLocatedCardSystem.CollaborationWindow.GestureModule
{
    class SortingGesture:Gesture
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
        internal override void Detect(Touch[] touchList, Touch[] targetList)
        {
            List<Touch> usedTouches = new List<Touch>();
            foreach (Touch touch in touchList)
            {
                if (touch.Sender is Card && touch.Type != typeof(ResultCard) && !usedTouches.Contains(touch))
                {
                    Card card = touch.Sender as Card;
                    SortingBox[] boxes = gestureController.Controllers.SortingBoxController.GetAllSortingBoxes();
                    foreach (SortingBox box in boxes)
                    {
                        bool isIntersect = Coordination.IsIntersect(card.Position, box.Corners);
                        if (isIntersect)
                        {
                            foreach (Touch otherTouches in touchList)
                            {
                                if (touch.Sender == otherTouches.Sender && !usedTouches.Contains(otherTouches))
                                {
                                    usedTouches.Add(otherTouches);
                                }
                            }
                            Point vector = new Point(usedTouches[0].StartPoint.X - usedTouches[0].CurrentGlobalPoint.X,
                                usedTouches[0].StartPoint.Y - usedTouches[0].CurrentGlobalPoint.Y);
                            double dist = 50;
                            double vLength = Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
                            vector.X = vector.X * dist / vLength;
                            vector.Y = vector.Y * dist / vLength;
                            card.MoveBy(vector);
                            gestureController.Controllers.SortingBoxController.AddCardToSortingBox(card, box);
                        }
                    }
                }
            }
        }
    }
}
