using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Input;

namespace CoLocatedCardSystem.CollaborationWindow.Layers.Base_Layer
{
    /// <summary>
    /// Controller class for the invisible layer at the bottom
    /// </summary>
    class BaseLayerController
    {
        private BaseLayer baseLayer;
        private CentralControllers controllers;

        public BaseLayerController(CentralControllers ctrls)
        {
            controllers = ctrls;
        }
        /// <summary>
        /// Initialize the card layer
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void Init(int width, int height)
        {
            baseLayer = new BaseLayer(this);
            baseLayer.Init(width, height);
        }
        /// <summary>
        /// Destroy the base layer
        /// </summary>
        public void Deinit()
        {
            baseLayer.Deinit();
        }

        internal BaseLayer GetBaseLayer() {
            return baseLayer;
        }
        /// <summary>
        /// Pass the touch point from the base layer to the view controller
        /// </summary>
        /// <param name="p"></param>
        internal void PointerDown(PointerPoint localPoint, PointerPoint globalPoint)
        {
            controllers.TouchController.TouchDown(localPoint, globalPoint, baseLayer, typeof(BaseLayer));
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
        /// End the touch point
        /// </summary>
        /// <param name="p"></param>
        internal void PointerUp(PointerPoint localPoint, PointerPoint globalPoint)
        {
            controllers.TouchController.TouchUp(localPoint,globalPoint);
        }
    }
}
