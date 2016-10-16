using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Core;
using CoLocatedCardSystem.CollaborationWindow.InteractionModule;

namespace CoLocatedCardSystem.CollaborationWindow.Layers.SortingBox_Layer
{
    class SortingBoxLayer : Canvas
    {
        private SortingBoxLayerController sortingBoxLayerController;

        public SortingBoxLayer(SortingBoxLayerController sortingBoxLayerController)
        {
            this.sortingBoxLayerController = sortingBoxLayerController;
        }
        /// <summary>
        /// Initialize the sorting box layer
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        internal void Init(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }
        /// <summary>
        /// Destroy the sorting box layer
        /// </summary>
        internal void Deinit()
        {
            this.Children.Clear();
        }
        /// <summary>
        /// Add a sorting box to the layer
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        internal async void AddBox(SortingBox box)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                this.Children.Add(box);
            });
        }


        /// <summary>
        /// Set the z index of the sorting box
        /// </summary>
        /// <param name="card"></param>
        /// <param name="zindex"></param>
        internal async void SetZIndex(SortingBox box, int zindex)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Canvas.SetZIndex(box, zindex);
            });
        }

        internal async void RemoveSortingBox(SortingBox box)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                this.Children.Remove(box);
            });
        }
    }
}
