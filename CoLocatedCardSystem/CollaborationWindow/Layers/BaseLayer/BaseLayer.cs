using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace CoLocatedCardSystem.CollaborationWindow.Layers.Base_Layer
{
    /// <summary>
    /// The invisible layer stay at the bottom
    /// </summary>
    class BaseLayer : Canvas
    {
        /// <summary>
        /// Control the BaseLayer, initialized with the layer
        /// </summary>
        BaseLayerController baseLayerController;
        Rectangle baseRect;
        internal BaseLayerController BaseLayerController
        {
            get
            {
                return baseLayerController;
            }

            set
            {
                baseLayerController = value;
            }
        }
        public BaseLayer(BaseLayerController baseLyrCtrl)
        {
            this.baseLayerController = baseLyrCtrl;
        }
        /// <summary>
        /// Initialize the view and add listener
        /// </summary>
        internal void Init(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            baseRect = new Rectangle();
            baseRect.Width = this.Width;
            baseRect.Height = this.Height;
            baseRect.Fill = new SolidColorBrush(MyColor.DarkBlue);
            this.Children.Add(baseRect);
            this.PointerPressed += PointerDown;
            this.PointerMoved += PointerMove;
            this.PointerExited += PointerUp;
            this.PointerReleased += PointerUp;
            this.PointerCaptureLost += PointerUp;
            this.PointerCanceled += PointerUp;
        }
        /// <summary>
        /// Destroy the view and remove listener
        /// </summary>
        internal void Deinit()
        {
            baseRect.PointerPressed -= PointerDown;
            baseRect.PointerMoved -= PointerMove;
            baseRect.PointerCanceled -= PointerUp;
            baseRect.PointerReleased -= PointerUp;
            baseRect.PointerExited -= PointerUp;
            baseRect.PointerCaptureLost -= PointerUp;
        }
        /// <summary>
        /// Call back method for Pointer down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PointerDown(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint localPoint = e.GetCurrentPoint(this);
            PointerPoint globalPoint = e.GetCurrentPoint(Coordination.Baselayer);
            baseLayerController.PointerDown(localPoint, globalPoint);
        }
        /// <summary>
        /// Call back method for Pointer move
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PointerMove(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint localPoint = e.GetCurrentPoint(this);
            PointerPoint globalPoint = e.GetCurrentPoint(Coordination.Baselayer);
            baseLayerController.PointerMove(localPoint, globalPoint);
        }
        /// <summary>
        /// Call back method for pointer up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PointerUp(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint localPoint = e.GetCurrentPoint(this);
            PointerPoint globalPoint = e.GetCurrentPoint(Coordination.Baselayer);
            baseLayerController.PointerUp(localPoint, globalPoint);
        }
    }
}
