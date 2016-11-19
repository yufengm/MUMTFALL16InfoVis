using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Core;
using CoLocatedCardSystem.CollaborationWindow.Layers.Card_Layer;
using CoLocatedCardSystem.CollaborationWindow.InteractionModule;
using CoLocatedCardSystem.CollaborationWindow.InteractionModule;

namespace CoLocatedCardSystem.CollaborationWindow.Layers.Glow_Layer
{
    class GlowLayer:Canvas
    {
        GlowLayerController glowLayerController;
        internal GlowLayer(GlowLayerController glctrl)
        {
            this.glowLayerController = glctrl;
        }
        /// <summary>
        /// Initialize the glow layer.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        internal void Init(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }
        internal async void Deinit()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                this.Children.Clear();
            });
        }

        internal async void RemoveGlow(Glow glow)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (this.Children.Contains(glow)) {
                    this.Children.Remove(glow);
                }
            });
        }
        /// <summary>
        /// Add a glow to the layer
        /// </summary>
        /// <param name="cardStatus"></param>
        /// <param name="colorIndex"></param>
        /// <param name="controller"></param>
        /// <returns></returns>
        internal async Task<Glow> AddGlow(CardStatus cardStatus, int colorIndex, GlowController controller)
        {
            Glow glow = null;
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                glow = new Glow(controller);
                glow.Init(cardStatus.cardID, colorIndex);
                glow.ApplyNewTransform(cardStatus.position,
                    cardStatus.rotation,
                    cardStatus.scale);
                this.Children.Add(glow);
            });
            return glow;
        }
    }
}
