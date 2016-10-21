using CoLocatedCardSystem.CollaborationWindow.Layers.Menu_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    class ResultCard:DocumentCard
    {
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
