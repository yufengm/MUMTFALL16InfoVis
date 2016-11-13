using Attribute = CoLocatedCardSystem.CollaborationWindow.TableModule.DataAttribute;
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

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    /// <summary>
    /// Base card, basic form of the card
    /// </summary>
    class Card : Canvas
    {
        protected string cardID = "";
        protected CardController cardController;
        protected Point position = new Point(0, 0);//The position of the card on the screen
        protected double cardScale = 1;//The scale ratio of the card
        protected double rotation = 0;//The degree of the rotation
        protected User owner = User.ALEX;
        protected Rectangle backgroundRect = null;
        protected Point[] corners;
        protected double marginWidth = 10 * Screen.SCALE_FACTOR;
        protected Size maxSize = new Size(600 * Screen.SCALE_FACTOR, 450 * Screen.SCALE_FACTOR);//Max size a card can be zoomed. 3.75 cardscale
        protected Size minSize = new Size(80 * Screen.SCALE_FACTOR, 60 * Screen.SCALE_FACTOR);//Mim size a card can be zoomed. 0.5 cardscale
        internal Card(CardController cardController)
        {
            this.cardController = cardController;
        }
        internal Point Position
        {
            get
            {
                return position;
            }
        }

        internal double CardScale
        {
            get
            {
                return cardScale;
            }
        }

        internal double Rotation
        {
            get
            {
                return rotation;
            }
        }

        internal string CardID
        {
            get { return cardID; }
            set { cardID = value; }
        }
        internal User Owner
        {
            get
            {
                return owner;
            }

            set
            {
                owner = value;
            }
        }
        internal CardController CardController
        {
            get
            {
                return cardController;
            }

            set
            {
                cardController = value;
            }
        }

        internal Point[] Corners
        {
            get
            {
                return corners;
            }
        }


        /// <summary>
        /// Initialize the card infomation. Since there are too many cards, loading
        /// them all at once is slow. Only load the card UI when adding to the layers.
        /// </summary>
        /// <param name="cardID"></param>
        /// <param name="user"></param>
        protected virtual void Init(string cardID, User user)
        {
            this.cardID = cardID;
            this.owner = user;
            //Initialize the card setting
            CardInfo info = CardInfo.GetCardInfo(owner);
            this.position = info.CardPosition;
            this.cardScale = info.CardScale;
            this.rotation = info.CardRotation;
        }

        internal virtual async Task LoadUI()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                CardInfo info = CardInfo.GetCardInfo(owner);
                this.Width = info.CardSize.Width;
                this.Height = info.CardSize.Height;
                UpdateTransform();
                backgroundRect = new Rectangle();
                //Move the backgroud rectangle -1/2 width and -1/2 height to the center.
                UIHelper.InitializeUI(
                    new Point(-0.5 * (this.Width + marginWidth * 2), -0.5 * (this.Height + marginWidth * 2)), 0, 1,
                    new Size(this.Width + marginWidth * 2, this.Height + marginWidth * 2),
                    backgroundRect);
                backgroundRect.Fill = new SolidColorBrush(info.CardColor);
                this.Children.Add(backgroundRect);
                //Register the touch events
                this.PointerPressed += PointerDown;
                this.PointerMoved += PointerMove;
                this.PointerExited += PointerUp;
                this.PointerReleased += PointerUp;
                this.PointerCaptureLost += PointerUp;
                this.PointerCanceled += PointerUp;
                this.PointerWheelChanged += Card_PointerWheelChanged;
                //Manipulation
                this.ManipulationMode = ManipulationModes.All;
                this.ManipulationStarting += Card_ManipulationStarting;
                this.ManipulationDelta += Card_ManipulationDelta;
                this.ManipulationCompleted += Card_ManipulationCompleted;
            });
        }

        internal void Deinit()
        {
            this.PointerPressed -= PointerDown;
            this.PointerMoved -= PointerMove;
            this.PointerCanceled -= PointerUp;
            this.PointerReleased -= PointerUp;
            this.PointerExited -= PointerUp;
            this.PointerCaptureLost -= PointerUp;
            this.ManipulationStarting -= Card_ManipulationStarting;
            this.ManipulationDelta -= Card_ManipulationDelta;
            this.PointerWheelChanged -= Card_PointerWheelChanged;
        }
        /// <summary>
        /// Update the card size, detect if any new layers need to appear.
        /// </summary>
        internal virtual void UpdateSize()
        {

        }
        /// <summary>
        /// Move the card by the vector 
        /// </summary>
        /// <param name="point"></param>
        internal void MoveBy(Point vector)
        {
            this.position.X += vector.X;
            this.position.Y += vector.Y;
            UpdateTransform();
        }
        /// <summary>
        /// Move the card to the position
        /// </summary>
        /// <param name="position"></param>
        internal void MoveTo(Point position)
        {
            this.position = position;
            UpdateTransform();
        }
        /// <summary>
        /// Rotate the card by "angle" degree. Add to the current rotation
        /// </summary>
        /// <param name="angle"></param>
        internal void Rotate(double angle)
        {
            this.rotation += angle;
            UpdateTransform();
        }
        /// <summary>
        /// Scale the card to the "scale"
        /// </summary>
        /// <param name="scale"></param>
        internal void Scale(double scale)
        {
            this.cardScale = scale;
            UpdateTransform();
        }
        /// <summary>
        /// Move the card to the new position with new rotation and scale value
        /// </summary>
        /// <param name="point"></param>
        /// <param name="rotation"></param>
        /// <param name="scale"></param>
        internal void ApplyNewTransform(Point position, double rotation, double scale)
        {
            this.position = position;
            this.rotation = rotation;
            this.cardScale = scale;
            UpdateTransform();
        }
        protected async void SetBackground(Color color) {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                backgroundRect.Fill = new SolidColorBrush(color);
            });
        }
        /// <summary>
        /// Update the transform group
        /// </summary>
        protected async void UpdateTransform()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
             {
                 ScaleTransform st = new ScaleTransform();
                 st.ScaleX = cardScale;
                 st.ScaleY = cardScale;
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
                 UpdateSize();
                 if (backgroundRect != null)
                 {
                    corners = new Point[] {
                    new Point(-backgroundRect.Width/2, -backgroundRect.Height/2),
                    new Point(backgroundRect.Width/2, -backgroundRect.Height/2),
                    new Point(backgroundRect.Width/2, backgroundRect.Height/2),
                    new Point(-backgroundRect.Width/2, backgroundRect.Height/2) };
                     for (int i = 0; i < 4; i++)
                     {
                         corners[i] = transGroup.TransformPoint(corners[i]);
                     }
                 }
             });
        }
        /// <summary>
        /// Call back method for Pointer down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void PointerDown(object sender, PointerRoutedEventArgs e)
        {
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
            cardController.PointerMove(localPoint, globalPoint);
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
            cardController.PointerUp(localPoint, globalPoint);
        }
        /// <summary>
        /// Zoom the card when wheel changed. Designed for debugging with non-touch screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Card_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint point = e.GetCurrentPoint(this);
            int delta=point.Properties.MouseWheelDelta;
            if (delta > 0)
            {
                if (IsValideManipulation(new Point(0,0), 0, 1.2))
                    this.cardScale *= 1.2;
            }
            else
            {
                if (IsValideManipulation(new Point(0, 0), 0, 1/1.2))
                    this.cardScale /= 1.2;
            }
            UpdateTransform();
        }
        /// <summary>
        /// Check if the manipulation is valid. 
        /// Cancel the manipulation if the card larger or smaller than the bound, or moved out of the screen.
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="rotat"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        private bool IsValideManipulation(Point trans, double rotat, double scale)
        {
            bool isValid = true;
            if (scale * this.cardScale * this.Width > maxSize.Width ||
                scale * this.cardScale * this.Height > maxSize.Height ||
                scale * this.cardScale * this.Width < minSize.Width ||
                scale * this.cardScale * this.Height < minSize.Height)
            {
                isValid = false;
            }
            if (position.X + trans.X > Screen.WIDTH ||
                position.X + trans.X < 0 ||
                position.Y + trans.Y > Screen.HEIGHT ||
                position.Y + trans.Y < 0)
            {
                isValid = false;
            }
            return isValid;
        }
        /// <summary>
        /// Update the z index of the focused the card. Put it on the top of other cards.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Card_ManipulationStarting(object sender, ManipulationStartingRoutedEventArgs e)
        {
            cardController.MoveCardToTop(this);
        }
        /// <summary>
        /// Manipulate the card. Move if the manipulation is valid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Card_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (IsValideManipulation(e.Delta.Translation, e.Delta.Rotation, e.Delta.Scale))
            {
                this.position.X += e.Delta.Translation.X;
                this.position.Y += e.Delta.Translation.Y;
                this.rotation += e.Delta.Rotation;
                this.cardScale *= e.Delta.Scale;
                UpdateTransform();
            }
            else
            {
                e.Complete();
            }
        }
        protected virtual void Card_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
        }
    }
}
