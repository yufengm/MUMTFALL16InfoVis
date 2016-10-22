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
    class ResultCard:DocumentCard
    {
        TextBlock titleTextBlock = new TextBlock();
        /// <summary>
        /// A fake card added to the tray just to show the search result. The move of the card is restricted to the block.
        /// </summary>
        Canvas block = null;
        MenuLayerController menuLayerController = null;
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

        internal ResultCard(CardController cardController) : base(cardController)
        {
            
        }
        internal override async Task LoadUI()
        {
            await base.LoadUI();
            await Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                titleTextBlock.Width = this.Width;
                titleTextBlock.Height = this.Height;
                titleTextBlock.Foreground = new SolidColorBrush(Colors.Black);
                titleTextBlock.LineHeight = 1;
                titleTextBlock.TextWrapping = TextWrapping.Wrap;
                titleTextBlock.FontSize = 13;
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
                titleTextBlock.Text = this.Document.GetName();
                if (this.Document.GetName().Length > 25)
                {
                    titleTextBlock.FontSize = 11;

                }
                if (this.Document.GetName().Length > 50)
                {
                    titleTextBlock.FontSize = 9;
                }
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
                block.Children.Remove(this);
                menuLayerController.AddPulledCard(this);
            }
        }
    }
}
