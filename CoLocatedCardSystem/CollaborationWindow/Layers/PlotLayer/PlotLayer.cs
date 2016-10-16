using CoLocatedCardSystem.CollaborationWindow.InteractionModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace CoLocatedCardSystem.CollaborationWindow.Layers.Plot_Layer
{
    class PlotLayer:Canvas
    {
        PlotLayerController plotLayerController;

        public PlotLayer(PlotLayerController clctrl)
        {
            this.plotLayerController = clctrl;
        }
        /// <summary>
        /// Initialize the card layer
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        internal void Init(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        /// <summary>
        /// Destroy the card layer
        /// </summary>
        internal void Deinit()
        {
            this.Children.Clear();
        }

        /// <summary>
        /// Add a card to the card layer
        /// </summary>
        /// <param name="card"></param>
        internal async Task AddCard(Card card)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                this.Children.Add(card);
                DocumentCard sCard = card as DocumentCard;
            });
        }

        /// <summary>
        /// Set the z index of the card
        /// </summary>
        /// <param name="card"></param>
        /// <param name="zindex"></param>
        internal async Task SetZIndex(Card card, int zindex)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Canvas.SetZIndex(card, zindex);
            });
        }
    }
}
