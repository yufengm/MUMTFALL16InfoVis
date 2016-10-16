using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace CoLocatedCardSystem.CollaborationWindow.Layers.Glow_Layer
{
    class Glow : Canvas
    {
        Rectangle rectangle;
        double scale = 1.3;
        Point position = new Point(0, 0);
        double rotation = 0;
        GlowController glowController;
        int colorIndex = 0;
        GlowInfo glowInfo;
        string cardID;
        public int ColorIndex
        {
            get
            {
                return colorIndex;
            }

            set
            {
                colorIndex = value;
                UpdateColor();
            }
        }

        internal Glow(GlowController controller)
        {
            this.glowController = controller;
        }
        /// <summary>
        /// Initialize the glow
        /// </summary>
        /// <param name="cardID"></param>
        /// <param name="colorIndex"></param>
        internal void Init(string cardID, int colorIndex)
        {
            this.colorIndex = colorIndex;
            this.cardID = cardID;
            glowInfo = GlowInfo.GetGlowInfo();
            this.Width = glowInfo.GlowSize.Width;
            this.Height = glowInfo.GlowSize.Height;
            rectangle = new Rectangle();      
            rectangle.Fill = new SolidColorBrush(glowInfo.GlowColors[colorIndex]);          
            UIHelper.InitializeUI(
                    new Point(-0.5 * this.Width, -0.5 * this.Height), 0, 1,
                    new Size(this.Width, this.Height),
                    rectangle);
            this.Children.Add(rectangle);
            //Register the touch events
            this.PointerPressed += PointerDown;
            this.PointerMoved += PointerMove;
            this.PointerExited += PointerUp;
            this.PointerReleased += PointerUp;
            this.PointerCaptureLost += PointerUp;
            this.PointerCanceled += PointerUp;
            this.DoubleTapped += Glow_DoubleTapped;
            //Manipulation
            this.ManipulationMode = ManipulationModes.All;
            this.ManipulationDelta += Glow_ManipulationDelta;
            this.ManipulationCompleted += Glow_ManipulationComplete;
        }

        internal void Deinit()
        {
            this.PointerPressed -= PointerDown;
            this.PointerMoved -= PointerMove;
            this.PointerCanceled -= PointerUp;
            this.PointerReleased -= PointerUp;
            this.PointerExited -= PointerUp;
            this.PointerCaptureLost -= PointerUp;
            this.ManipulationDelta -= Glow_ManipulationDelta;
            this.ManipulationCompleted -= Glow_ManipulationComplete;
        }
        //Update the color index
        private async void UpdateColor()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                rectangle.Fill = new SolidColorBrush(glowInfo.GlowColors[colorIndex]);
            });
        }

        /// <summary>
        /// Move the glow by the vector 
        /// </summary>
        /// <param name="point"></param>
        internal void MoveBy(Point vector)
        {
            this.position.X += vector.X;
            this.position.Y += vector.Y;
            UpdateTransform();
        }
        /// <summary>
        /// Move the glow to the new position with new rotation and scale value
        /// </summary>
        /// <param name="point"></param>
        /// <param name="rotation"></param>
        /// <param name="scale"></param>
        internal void ApplyNewTransform(Point position, double rotation, double scale)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale + 0.5;
            UpdateTransform();
        }

        /// <summary>
        /// Update the transform group
        /// </summary>
        protected async void UpdateTransform()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ScaleTransform st = new ScaleTransform();
                st.ScaleX = scale;
                st.ScaleY = scale;
                RotateTransform rt = new RotateTransform();
                rt.Angle = rotation;
                TranslateTransform tt = new TranslateTransform();
                tt.X = position.X;
                tt.Y = position.Y;
                TransformGroup transGroup = new TransformGroup();
                transGroup.Children.Add(st);
                transGroup.Children.Add(rt);
                transGroup.Children.Add(tt);
                this.RenderTransform = transGroup;
            });
        }
        /// <summary>
        /// Call back method for Pointer down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void PointerDown(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint localPoint = e.GetCurrentPoint(this);
            PointerPoint globalPoint = e.GetCurrentPoint(Coordination.Baselayer);
            glowController.PointerDown(localPoint, globalPoint, this, typeof(Glow));
        }
        /// <summary>
        /// Call back method for Pointer move
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void PointerMove(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint localPoint = e.GetCurrentPoint(this);
            PointerPoint globalPoint = e.GetCurrentPoint(Coordination.Baselayer);
            glowController.PointerMove(localPoint, globalPoint);
        }
        /// <summary>
        /// Call back method for pointer up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void PointerUp(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint localPoint = e.GetCurrentPoint(this);
            PointerPoint globalPoint = e.GetCurrentPoint(Coordination.Baselayer);
            glowController.PointerUp(localPoint, globalPoint);
        }
        /// <summary>
        /// Manipulate the card. Move if the manipulation is valid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Glow_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            Point vector = e.Delta.Translation;
            glowController.UpdateConnectedPosition(cardID, vector);
        }
        private void Glow_ManipulationComplete(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            glowController.ConnectGroupWithGroups(cardID);
        }
        /// <summary>
        /// When double tapped the glow, change the color
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Glow_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            colorIndex = (colorIndex + 1) % glowInfo.GlowColors.Length;
            glowController.UpdateConnectedColor(cardID, colorIndex);
        }
    }
}
