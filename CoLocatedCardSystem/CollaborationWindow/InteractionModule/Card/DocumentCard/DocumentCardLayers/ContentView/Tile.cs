using CoLocatedCardSystem.CollaborationWindow.DocumentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    class Tile : Canvas
    {
        Token token;
        double textSize;
        Size boxSize;
        bool isHighlighted = false;
        bool isKeyWord = false;
        DocumentCardController documentCardController;
        DocumentCard docCard;
        internal Token Token
        {
            get
            {
                return token;
            }
        }
        internal void Init(DocumentCardController controller,DocumentCard docCard, Token token, double textSize, Size boxSize)
        {
            this.token = token;
            this.textSize = textSize;
            this.boxSize = boxSize;
            this.docCard = docCard;
            if (token.WordType !=WordType.REGULAR)
            {
                this.isKeyWord = false;
            }
            else
            {
                this.isKeyWord = true;
            }
            this.documentCardController = controller;
            InitUI();
        }

        private void InitUI()
        {
            if (isKeyWord)
            {
                this.Background = new SolidColorBrush(MyColor.DarkGrassGreen);
            }
            else
            {
                this.Background = new SolidColorBrush(Colors.Transparent);
            }
            TextBlock tb = new TextBlock();
            tb.Text = token.OriginalWord;
            tb.FontSize = textSize;
            tb.Padding = new Thickness(1,0,1,0);
            tb.TextAlignment = TextAlignment.Center;
            tb.VerticalAlignment = VerticalAlignment.Bottom;
            tb.Width = boxSize.Width;
            tb.Height = boxSize.Height;
            tb.Foreground = new SolidColorBrush(MyColor.Wheat);
            this.Width = boxSize.Width;
            this.Height = boxSize.Height;
            this.Children.Add(tb);            
            if (isKeyWord)
            {
                this.Tapped += Tile_Tapped;
            }
        }

        private void Tile_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (isHighlighted)
            {
                docCard.RemoveHighLightWord(token);
            }
            else
            {
                docCard.AddHighLightWord(token);
            }
        }

        internal void Deinit()
        {
            if (isKeyWord)
            {
                this.Tapped -= Tile_Tapped;
            }
        }
        internal void HighLight()
        {
            this.Background = new SolidColorBrush(MyColor.Yellow);
            isHighlighted = true;
        }
        internal void DeHighLight()
        {
            this.Background = new SolidColorBrush(MyColor.DarkGrassGreen);
            isHighlighted = false;
        }
        private void Tile_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            DeHighLight();
        }

        private void Tile_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            HighLight();
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
            documentCardController.PointerDown_Tile(localPoint, globalPoint, this, typeof(Tile));
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
            documentCardController.CardController.PointerMove(localPoint, globalPoint);
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
            documentCardController.CardController.PointerUp(localPoint, globalPoint);
        }
    }
}
