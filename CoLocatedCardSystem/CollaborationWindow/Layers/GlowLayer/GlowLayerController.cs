using System;
using System.Threading.Tasks;
using CoLocatedCardSystem.CollaborationWindow.Layers.Card_Layer;
using CoLocatedCardSystem.CollaborationWindow.InteractionModule;

namespace CoLocatedCardSystem.CollaborationWindow.Layers.Glow_Layer
{
    class GlowLayerController
    {
        GlowLayer glowLayer;
        CentralControllers controllers;

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
        internal void RemoveGlowEffect(Glow glow)
        {
            glowLayer.RemoveGlow(glow);
        }
        /// <summary>
        /// Add glow to layer
        /// </summary>
        /// <param name="cardID">the id of the card</param>
        /// <param name="colorIndex">color of the glow</param>
        /// <param name="status">card status</param>
        /// <param name="controller"></param>
        /// <returns></returns>
        internal async Task<Glow> AddGlow(CardStatus status, int colorIndex,  GlowController controller)
        {

            Glow glow = await glowLayer.AddGlow(status, colorIndex,  controller);
            return glow;
        }
    }
}
