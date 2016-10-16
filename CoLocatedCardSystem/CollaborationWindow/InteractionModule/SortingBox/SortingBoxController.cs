using System;
using System.Collections.Generic;
using Windows.UI.Input;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    public class SortingBoxController
    {
        SortingBoxList list;
        CentralControllers controllers;

        public CentralControllers Controllers
        {
            get
            {
                return controllers;
            }

            set
            {
                controllers = value;
            }
        }

        public SortingBoxController(CentralControllers ctrls)
        {
            this.controllers = ctrls;
        }

        /// <summary>
        /// Initialize the sorting box controller
        /// </summary>
        public void Init()
        {
            list = new SortingBoxList();
        }
        /// <summary>
        /// Destroy all sorting boxes
        /// </summary>
        public void Deinit()
        {
            list.Clear();
        }
        /// <summary>
        /// Based on the user, create a sorting box.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="user"></param>
        public SortingBox CreateSortingBox(User user, string name)
        {
            SortingBox box = list.AddBox(user, name, this);
            controllers.SortingBoxLayerController.LoadBoxes(new SortingBox[] {box});
            return box;
        }
        /// <summary>
        /// Add a card to the sortingbox
        /// </summary>
        /// <param name="card"></param>
        /// <param name="box"></param>
        public void AddCardToSortingBox(Card card, SortingBox box)
        {
            box.AddCard(card);
        }
        /// <summary>
        /// Get all sorting box by card
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        SortingBox[] FindAllSortingBoxesByCard(Card card)
        {
            List<SortingBox> boxes = new List<SortingBox>();
            foreach (SortingBox box in list.GetAllSortingBoxes())
            {
                foreach (string cd in box.CardList)
                {
                    if (card.CardID == cd)
                    {
                        boxes.Add(box);
                    }
                }
            }
            return boxes.ToArray();
        }
        /// <summary>
        /// Get all sorting boxes in the list
        /// </summary>
        /// <returns></returns>
        internal SortingBox[] GetAllSortingBoxes()
        {
            return list.GetAllSortingBoxes();
        }

        bool ContainCard(SortingBox box, Card card)
        {
            foreach (string cd in box.CardList)
            {
                if (card.CardID == cd)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Delete a sorting box. Remove it both from the sorting box list and the sorting box layer.
        /// </summary>
        /// <param name="box"></param>
        internal void DestroyBox(SortingBox box)
        {
            list.RemoveSortingBox(box);
            controllers.SortingBoxLayerController.RemoveSortingBox(box);
        }
        /// <summary>
        /// Update the touch point
        /// </summary>
        /// <param name="localPoint"></param>
        /// <param name="globalPoint"></param>
        /// <param name="box"></param>
        /// <param name="type"></param>
        internal void PointerDown(PointerPoint localPoint, PointerPoint globalPoint, SortingBox box, Type type)
        {
            controllers.TouchController.TouchDown(localPoint, globalPoint, box, type);
        }
        /// <summary>
        /// update the touch point
        /// </summary>
        /// <param name="localPoint"></param>
        /// <param name="globalPoint"></param>
        internal void PointerMove(PointerPoint localPoint, PointerPoint globalPoint)
        {
            controllers.TouchController.TouchMove(localPoint, globalPoint);
        }
        /// <summary>
        /// Release a touch point
        /// </summary>
        /// <param name="localPoint"></param>
        /// <param name="globalPoint"></param>

        internal void PointerUp(PointerPoint localPoint, PointerPoint globalPoint)
        {
            controllers.TouchController.TouchUp(localPoint, globalPoint);
        }

        /// <summary>
        /// Update the ZIndex of the card. Move the card to the top.
        /// </summary>
        /// <param name="card"></param>
        internal void MoveSortingBoxToTop(SortingBox box)
        {
            controllers.SortingBoxLayerController.MoveSortingBoxToTop(box);
        }
    }
}
