using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Input;
using Windows.UI.Input;
using Windows.UI.Core;
using Windows.UI.Text;
using Windows.UI.Xaml;
using CoLocatedCardSystem.CollaborationWindow.Layers.InteractionModule;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    public class SortingBox : Canvas
    {
        string sortingBoxID;
        string name;
        double sortingBoxScale;
        double rotation;

        User owner;
        List<string> cardList;
        Point position = new Point(0, 0);
        SortingBoxController sortingBoxController;
        Rectangle background; // Background rectangle
        TextBlock textBlockTop;
        TextBlock textBlockBottom;
        Size maxSize = new Size(600, 450);
        Size minSize = new Size(80, 60);
        private Point[] corners;

        internal SortingBox(SortingBoxController sortingBoxController)
        {
            this.sortingBoxController = sortingBoxController;
        }
        internal string SortingBoxID
        {
            get { return sortingBoxID; }
            set { sortingBoxID = value; }
        }

        internal string SortingBoxName
        {
            get { return name; }
        }
        internal double SortingBoxScale
        {
            get { return sortingBoxScale; }
        }

        internal double Rotation
        {
            get { return rotation; }
        }

        internal List<string> CardList
        {
            get { return cardList; }
        }

        internal Point Position
        {
            get { return position; }
        }

        internal Point[] Corners
        {
            get
            {
                return corners;
            }
        }

        /// <summary>
        /// Initialize sorting box
        /// </summary>
        /// <param name="sortingBoxID"></param>
        /// <param name="name"></param>
        /// <param name="user"></param>
        internal void Init(string sortingBoxID, string name, User user)
        {
            this.sortingBoxID = sortingBoxID;
            this.name = name;
            this.owner = user;
            cardList = new List<string>();
            LoadUI(user);
            //Register the touch events
            this.PointerPressed += PointerDown;
            this.PointerMoved += PointerMove;
            this.PointerExited += PointerUp;
            this.PointerReleased += PointerUp;
            this.PointerCaptureLost += PointerUp;
            this.PointerCanceled += PointerUp;
            this.ManipulationMode = ManipulationModes.All;
            this.ManipulationStarting += SortingBox_ManipulationStarting;
            this.ManipulationDelta += SortingBox_ManipulationDelta;
        }
        //Load UI component
        private void LoadUI(User user)
        {
            SortingBoxInfo info = SortingBoxInfo.GetSortingBoxInfo(user);
            this.Width = info.SortingBoxSize.Width;
            this.Height = info.SortingBoxSize.Height;
            position = info.SortingBoxPosition;
            sortingBoxScale = info.SortingBoxScale;
            rotation = info.SortingBoxRotation;
            UpdateTransform();
            //initialize the background rectangle
            background = new Rectangle();
            Calculator.InitializeUI(
               new Point(-0.5 * info.SortingBoxSize.Width, -0.5 * info.SortingBoxSize.Height), 0, 1,
               new Size(info.SortingBoxSize.Width, info.SortingBoxSize.Height),
               background);
            corners = new Point[] {
                    new Point(-background.Width/2, -background.Height/2),
                    new Point(background.Width/2, -background.Height/2),
                    new Point(background.Width/2, background.Height/2),
                    new Point(-background.Width/2, background.Height/2) };
            background.Fill = new SolidColorBrush(Colors.Transparent);
            background.Stroke = new SolidColorBrush(info.SortingBoxColor);
            background.StrokeThickness = 5;
            this.Children.Add(background);

            textBlockTop = new TextBlock();
            Calculator.InitializeUI(
                new Point(-0.5 * info.SortingBoxSize.Width, -0.5 * info.SortingBoxSize.Height), 0, 1,
                new Size(info.SortingBoxSize.Width, info.SortingBoxSize.Height),
                textBlockTop);

            textBlockTop.Foreground = new SolidColorBrush(Colors.White);
            textBlockTop.Text = " " + this.name;
            textBlockTop.FontFamily = new FontFamily("Comic Sans MS");
            textBlockTop.FontSize = 18;
            textBlockTop.FontWeight = FontWeights.Bold;
            textBlockTop.TextAlignment = TextAlignment.Left;
            textBlockTop.VerticalAlignment = VerticalAlignment.Center;

            textBlockBottom = new TextBlock();
            Calculator.InitializeUI(
                new Point(.5 * info.SortingBoxSize.Width, .5 * info.SortingBoxSize.Height), 180, 1,
                new Size(info.SortingBoxSize.Width, info.SortingBoxSize.Height),
                textBlockBottom);

            textBlockBottom.Foreground = new SolidColorBrush(Colors.White);
            textBlockBottom.Text = " " + this.name;
            textBlockBottom.FontFamily = new FontFamily("Comic Sans MS");
            textBlockBottom.FontSize = 18;
            textBlockBottom.FontWeight = FontWeights.Bold;
            textBlockBottom.TextAlignment = TextAlignment.Left;

            this.Children.Add(textBlockTop);
            this.Children.Add(textBlockBottom);

            Canvas.SetZIndex(background, 3);
            Canvas.SetZIndex(textBlockTop, 2);
            Canvas.SetZIndex(textBlockBottom, 1);

        }
        /// <summary>
        /// Deinitialize sorting box
        /// </summary>
        internal void Deinit()
        {
            background.PointerPressed -= PointerDown;
            background.PointerMoved -= PointerMove;
            background.PointerCanceled -= PointerUp;
            background.PointerReleased -= PointerUp;
            background.PointerExited -= PointerUp;
            background.PointerCaptureLost -= PointerUp;
        }
        /// <summary>
        /// Move the sorting box by vector.
        /// </summary>
        /// <param name="vector"></param>
        public void Move(Point vector)
        {
            this.position.X += vector.X;
            this.position.Y += vector.Y;
            UpdateTransform();
        }

        /// <summary>
        /// Move the box to the position
        /// </summary>
        /// <param name="position"></param>
        public void MoveTo(Point position)
        {
            this.position = position;
            UpdateTransform();
        }

        /// <summary>
        /// Increase the box rotation by angle
        /// </summary>
        /// <param name="angle"></param>
        public void Rotate(double angle)
        {
            this.rotation += angle;
            UpdateTransform();
        }

        /// <summary>
        /// Scale the box to scale
        /// </summary>
        /// <param name="scale"></param>
        public void Scale(double scale)
        {
            this.sortingBoxScale = scale;
            UpdateTransform();
        }

        /// <summary>
        /// Move the card to the new position with new rotation and scale value
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="scale"></param>
        public void ApplyNewTransform(Point position, double rotation, double scale)
        {
            this.position = position;
            this.rotation = rotation;
            this.sortingBoxScale = scale;
            UpdateTransform();
        }

        /// <summary>
        /// Update the rendertransform and show the new states
        /// </summary>
        private async void UpdateTransform()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ScaleTransform st = new ScaleTransform();
                st.ScaleX = sortingBoxScale;
                st.ScaleY = sortingBoxScale;
                RotateTransform rt = new RotateTransform();
                rt.Angle = Rotation;
                TranslateTransform tt = new TranslateTransform();
                tt.X = Position.X;
                tt.Y = Position.Y;
                TransformGroup transGroup = new TransformGroup();
                transGroup.Children.Add(st);
                transGroup.Children.Add(rt);
                transGroup.Children.Add(tt);
                this.RenderTransform = transGroup;
                if (corners != null)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        corners[i] = transGroup.TransformPoint(corners[i]);
                    }
                }
            });
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
            sortingBoxController.PointerUp(localPoint, globalPoint);
        }

        /// <summary>
        /// Call back method for pointer move
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PointerMove(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint localPoint = e.GetCurrentPoint(this);
            PointerPoint globalPoint = e.GetCurrentPoint(Coordination.Baselayer);
            sortingBoxController.PointerMove(localPoint, globalPoint);
        }

        /// <summary>
        /// Call back method for pointer down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PointerDown(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint localPoint = e.GetCurrentPoint(this);
            PointerPoint globalPoint = e.GetCurrentPoint(Coordination.Baselayer);
            sortingBoxController.PointerDown(localPoint, globalPoint, this, typeof(SortingBox));
        }

        /// <summary>
        /// Change the background color of the box
        /// </summary>
        /// <param name="color"></param>
        internal async void SetBackgroundColor(Color color)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                background.Fill = new SolidColorBrush(color);
            });
        }

        /// <summary>
        /// Add a card to the sorting box
        /// </summary>
        /// <param name="card"></param>
        internal void AddCard(Card card)
        {
            cardList.Add(card.CardID);
        }

        /// <summary>
        /// Remove a card from the sorting box
        /// </summary>
        /// <param name="card"></param>
        internal void RemoveCard(Card card)
        {
            if (cardList.Contains(card.CardID))
            {
                cardList.Remove(card.CardID);
            }
        }

        /// <summary>
        /// Remove all cards from the sorting box
        /// </summary>
        internal void Clear()
        {
            cardList.Clear();
        }

        /// <summary>
        /// Manipulate the sorting box. Move if the manipulation is valid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void SortingBox_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (IsValideManipulation(e.Delta.Translation, e.Delta.Rotation, e.Delta.Scale))
            {
                this.position.X += e.Delta.Translation.X;
                this.position.Y += e.Delta.Translation.Y;
                this.rotation += e.Delta.Rotation;
                MoveSortedCards(e.Delta.Translation);
                UpdateTransform();
                //this.sortingBoxScale *= e.Delta.Scale;
            }
        }

        private void MoveSortedCards(Point translation)
        {
            foreach (string cardID in cardList) {
                sortingBoxController.Controllers.CardController.MoveCardByVector(cardID, translation);
                //sortingBoxController.Controllers.GlowController.UpdateConnectedPosition(cardID, translation);
            }
        }

        /// <summary>
        /// Check if the manipulation is valid. 
        /// Cancel the manipulation if the sorting box is larger or smaller than the bound, or moved out of the screen.
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="rotat"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        private bool IsValideManipulation(Point trans, double rotat, double scale)
        {
            bool isValid = true;
            if (scale * this.sortingBoxScale * this.Width > maxSize.Width ||
                scale * this.sortingBoxScale * this.Height > maxSize.Height ||
                scale * this.sortingBoxScale * this.Width < minSize.Width ||
                scale * this.sortingBoxScale * this.Height < minSize.Height)
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
        /// Update the z index of the focused the sorting box. Put it on the top.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void SortingBox_ManipulationStarting(object sender, ManipulationStartingRoutedEventArgs e)
        {
            sortingBoxController.MoveSortingBoxToTop(this);
        }
    }
}