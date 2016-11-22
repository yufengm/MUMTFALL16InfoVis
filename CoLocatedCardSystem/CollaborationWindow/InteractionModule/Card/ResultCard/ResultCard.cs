using CoLocatedCardSystem.CollaborationWindow.DocumentModule;
using CoLocatedCardSystem.CollaborationWindow.Layers.Menu_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    class ResultCard : Card
    {
        Document document;
        TextBlock titleTextBlock = new TextBlock();
        /// <summary>
        /// A fake card added to the tray just to show the search result. The move of the card is restricted to the block.
        /// </summary>
        Canvas block = null;
        MenuLayerController menuLayerController = null;
        bool isEnabled = true;
        public Canvas Block
        {
            get
            {
                return block;
            }

            set
            {
                block = value;
            }
        }

        internal MenuLayerController MenuLayerController
        {
            get
            {
                return menuLayerController;
            }

            set
            {
                menuLayerController = value;
            }
        }

        public bool IsEnabled
        {
            get
            {
                return isEnabled;
            }

            set
            {
                isEnabled = value;
            }
        }

        internal ResultCard(CardController cardController) : base(cardController)
        {
            
        }

        /// <summary>
        /// Initialize a semantic card.
        /// </summary>
        /// <param name="cardID"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="position"></param>
        /// <param name="scale"></param>
        /// <param name="rotation"></param>
        internal void Init(string cardID, User user, Document doc)
        {
            base.Init(cardID, user);
            this.document = doc;
        }

        internal override async Task LoadUI()
        {
            await base.LoadUI();
            await Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                titleTextBlock.Width = this.Width;
                titleTextBlock.Height = this.Height;
                titleTextBlock.Foreground = new SolidColorBrush(MyColor.Wheat);
                titleTextBlock.LineHeight = 1;
                titleTextBlock.TextWrapping = TextWrapping.Wrap;
                titleTextBlock.TextAlignment = TextAlignment.Center;
                titleTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
                titleTextBlock.VerticalAlignment = VerticalAlignment.Center;
                titleTextBlock.FontStretch = FontStretch.Normal;
                titleTextBlock.FontWeight = FontWeights.Bold;
                UIHelper.InitializeUI(
                    new Point(-0.5 * this.Width, -0.5 * this.Height),
                    0,
                    1,
                    new Size(this.Width, this.Height),
                    titleTextBlock);
                this.Children.Add(titleTextBlock);
                titleTextBlock.Text = this.document.GetName();
                double fsize= 42 * Math.Pow(this.document.GetName().Length, -0.43);
                if (fsize > 16) {
                    fsize = 16;
                }
                titleTextBlock.FontSize = fsize;
            });
        }

        protected override void PointerUp(object sender, PointerRoutedEventArgs e)
        {
            MoveTo(new Point(80 * Screen.SCALE_FACTOR, 65 * Screen.SCALE_FACTOR));
            PointerPoint localPoint = e.GetCurrentPoint(this);
            PointerPoint globalPoint = e.GetCurrentPoint(Coordination.Baselayer);
            cardController.PointerUp(localPoint, globalPoint);
        }
        protected override void PointerDown(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint localPoint = e.GetCurrentPoint(this);
            PointerPoint globalPoint = e.GetCurrentPoint(Coordination.Baselayer);
            cardController.PointerDown(localPoint, globalPoint, this, typeof(ResultCard));
        }
        protected override void Card_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (isEnabled)
            {
                double translate = 0;
                if (owner == User.ALEX)
                {
                    translate = -e.Delta.Translation.X;
                }
                else if (owner == User.BEN)
                {
                    translate = e.Delta.Translation.Y;
                }
                else if (owner == User.CHRIS)
                {
                    translate = e.Delta.Translation.X;
                }
                else if (owner == User.DANNY)
                {
                    translate = -e.Delta.Translation.Y;
                }
                if (this.position.Y + translate >= 0
                    && this.position.Y + translate < 80 * Screen.SCALE_FACTOR)
                {
                    this.position.Y += translate;
                    UpdateTransform();
                }
                else if (this.position.Y + translate < 0)
                {
                    MoveTo(new Point(80 * Screen.SCALE_FACTOR, 65 * Screen.SCALE_FACTOR));
                    menuLayerController.AddPulledCard(this);
                    DisableCard();
                }
            }
        }
        internal void EnableCard()
        {
            isEnabled = true;
            titleTextBlock.Foreground = new SolidColorBrush(MyColor.Wheat);
            this.IsHitTestVisible = true;
        }
        internal void DisableCard()
        {
            isEnabled = false;
            titleTextBlock.Foreground = new SolidColorBrush(
                Color.FromArgb(100, MyColor.DarkGrassGreen.R, MyColor.DarkGrassGreen.G, MyColor.DarkGrassGreen.B));
            this.IsHitTestVisible = false;
        }
    }
}
