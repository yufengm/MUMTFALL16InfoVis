using System;
using System.Threading.Tasks;
using CoLocatedCardSystem.CollaborationWindow.InteractionModule;
using System.Collections.Generic;
using Windows.UI.Input;

namespace CoLocatedCardSystem.CollaborationWindow.Layers.Glow_Layer
{
    class GlowLayerController
    {
        GlowLayer glowLayer;
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

        public GlowLayerController(CentralControllers ctrls)
        {
            this.controllers = ctrls;
        }
        /// <summary>
        /// Initialize the glow controller and the menu layer.
        /// </summary>
        /// <param name="width">width of screen</param>
        /// <param name="height">height of screen</param>
        internal void Init(int width, int height)
        {
            glowLayer = new GlowLayer(this);
            glowLayer.Init(width, height);
        }
        /// <summary>
        /// Destroy the menu layer
        /// </summary>
        internal void Deinit()
        {
            glowLayer.Deinit();
        }

        internal GlowLayer GetGlowLayer()
        {
            return glowLayer;
        }
        /// <summary>
        /// Remove a glow from the glow layer
        /// </summary>
        /// <param name="glow"></param>
        internal async void RemoveGlowEffect(string cardID)
        {
            await glowLayer.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
            {
                lock (glowLayer)
                {
                    List<Glow> list = new List<Glow>();
                    foreach (Glow glow in glowLayer.Children) {
                        if (glow.CardID.Equals(cardID)) {
                            list.Add(glow);
                        }
                    }
                    foreach (Glow glow in list) {
                        glowLayer.RemoveGlow(glow);
                    }
                }
            });
        }
        /// <summary>
        /// Add glow to layer
        /// </summary>
        /// <param name="cardID">the id of the card</param>
        /// <param name="colorIndex">color of the glow</param>
        /// <param name="status">card status</param>
        /// <param name="controller"></param>
        /// <returns></returns>
        internal async Task<Glow> AddGlow(CardStatus status, int colorIndex,  GlowLayerController controller)
        {
            Glow glow = await glowLayer.AddGlow(status, colorIndex,  controller);
            return glow;
        }

        #region Pointer methods
        /// <summary>
        /// Create a touch and pass it to the interaction controller.
        /// </summary>
        /// <param name="p"></param>
        internal void PointerDown(PointerPoint localPoint, PointerPoint globalPoint, Glow glow, Type type)
        {
            controllers.TouchController.TouchDown(localPoint, globalPoint, glow, type);
        }

        /// <summary>
        /// Update the touch point
        /// </summary>
        /// <param name="p"></param>
        internal void PointerMove(PointerPoint localPoint, PointerPoint globalPoint)
        {
            controllers.TouchController.TouchMove(localPoint, globalPoint);
        }
        /// <summary>
        /// Lift the touch layer
        /// </summary>
        /// <param name="p"></param>
        internal void PointerUp(PointerPoint localPoint, PointerPoint globalPoint)
        {
            controllers.TouchController.TouchUp(localPoint, globalPoint);
        }
        #endregion
    }
}
