using System;
using System.Collections.Generic;
using System.Linq;
using CoLocatedCardSystem.CollaborationWindow.InteractionModule;

namespace CoLocatedCardSystem.CollaborationWindow.Layers.SortingBox_Layer
{
    class SortingBoxLayerController
    {
        SortingBoxLayer sortingBoxLayer;
        CentralControllers controllers;
        Dictionary<SortingBox, int> zIndexList = new Dictionary<SortingBox, int>();

        internal SortingBoxLayerController(CentralControllers ctrls) {
            this.controllers = ctrls;
        }

        internal void Init(int width, int height) {
            sortingBoxLayer = new SortingBoxLayer(this);
            sortingBoxLayer.Init(width, height);
        }

        internal void Deinit() {
            sortingBoxLayer.Deinit();
        }

        internal SortingBoxLayer GetSortingBoxLayer() {
            return sortingBoxLayer;
        }
        /// <summary>
        /// Add sorting boxes to the sorting box layer.
        /// </summary>
        /// <param name="boxes"></param>
        /// <returns></returns>
        internal void LoadBoxes(SortingBox[] boxes) {
            int index = zIndexList.Count();
            foreach (SortingBox box in boxes)
            {
                zIndexList.Add(box, index++);
                sortingBoxLayer.AddBox(box);
                sortingBoxLayer.SetZIndex(box, zIndexList[box]);
            }
        }

        /// <summary>
        /// Update the card zindex in the cardlayer
        /// </summary>
        /// <param name="card"></param>
        internal void MoveSortingBoxToTop(SortingBox box)
        {           
            if (zIndexList.Keys.Contains(box))
            {
                int currentIndex = zIndexList[box];
                foreach (SortingBox bx in zIndexList.Keys.ToList())
                {
                    if (zIndexList[bx] > currentIndex)
                    {
                        zIndexList[bx]--;
                        sortingBoxLayer.SetZIndex(bx, zIndexList[bx]);
                    }
                }
                zIndexList[box] = zIndexList.Count - 1;
                sortingBoxLayer.SetZIndex(box, zIndexList[box]);
            }
        }
        /// <summary>
        /// Remove a sorting box from the sorting box layer
        /// </summary>
        /// <param name="box"></param>
        internal void RemoveSortingBox(SortingBox box)
        {
            MoveSortingBoxToTop(box);
            zIndexList.Remove(box);
            sortingBoxLayer.RemoveSortingBox(box);
        }
    }
}
