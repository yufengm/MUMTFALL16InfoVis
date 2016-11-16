using CoLocatedCardSystem.CollaborationWindow.InteractionModule;
using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace CoLocatedCardSystem.CollaborationWindow.Layers.Card_Layer
{
    /// <summary>
    /// A layer to show all the active cards
    /// </summary>
    class CardLayer : Canvas
    {
        CardLayerController cardLayerController;

        public CardLayer(CardLayerController clctrl)
        {
            this.cardLayerController = clctrl;
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

        internal async void RemoveCard(Card card)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                this.Children.Remove(card);
            });
        }
    }
}
