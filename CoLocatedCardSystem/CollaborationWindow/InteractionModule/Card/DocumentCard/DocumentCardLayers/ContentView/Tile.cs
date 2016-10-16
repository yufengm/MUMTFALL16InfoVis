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
        DocumentCardController cardController;
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
            if (token.WordType == WordType.IRREGULAR ||
                token.WordType == WordType.LINEBREAK ||
                token.WordType == WordType.PUNCTUATION ||
                token.WordType == WordType.REGULAR ||
                token.WordType == WordType.STOPWORD ||
                token.WordType == WordType.DEFAULT)
            {
                this.isKeyWord = false;
            }
            else
            {
                this.isKeyWord = true;
            }
            this.cardController = controller;
            InitUI();
        }

        private void InitUI()
        {
            if (isKeyWord)
            {
                this.Background = new SolidColorBrush(Colors.LightGray);
            }
            else
            {
                this.Background = new SolidColorBrush(Colors.Transparent);
            }
            TextBlock tb = new TextBlock();
            tb.Text = token.OriginalWord;
            tb.FontSize = textSize;
            tb.Padding = new Thickness(0);
            tb.TextAlignment = TextAlignment.Justify;
            tb.Width = boxSize.Width;
            tb.Height = boxSize.Height;

            TextBlock note = new TextBlock();
            note.FontSize = 1.5;
            note.Padding = new Thickness(0);
            
            switch (token.WordType)
            {
                case WordType.DATE:
                    tb.Foreground = new SolidColorBrush(Colors.Blue);
                    note.Text = "DAT";
                    break;
                case WordType.PERSON:
                    tb.Foreground = new SolidColorBrush(Colors.Red);
                    note.Text = "PER";
                    break;
                case WordType.MONEY:
                    tb.Foreground = new SolidColorBrush(Colors.DarkSalmon);
                    note.Text = "MON";
                    break;
                case WordType.ORGANIZATION:
                    tb.Foreground = new SolidColorBrush(Colors.Green);
                    note.Text = "ORG";
                    break;
                case WordType.LOCACTION:
                    tb.Foreground = new SolidColorBrush(Colors.DarkGoldenrod);
                    note.Text = "LOC";
                    break;
                case WordType.TIME:
                    tb.Foreground = new SolidColorBrush(Colors.LightBlue);
                    note.Text = "TIM";
                    break;
            }
            this.Width = boxSize.Width;
            this.Height = boxSize.Height;
            this.Children.Add(tb);            
            if (isKeyWord)
            {
                this.Tapped += Tile_Tapped;
                UIHelper.InitializeUI(new Point(0, -0.5), 0, 1, new Size(20,10), note);
                note.Foreground = tb.Foreground;
                this.Children.Add(note);
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
            this.Background = new SolidColorBrush(Colors.Yellow);
            isHighlighted = true;
        }
        internal void DeHighLight()
        {
            this.Background = new SolidColorBrush(Colors.LightGray);
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
            cardController.PointerDown_Tile(localPoint, globalPoint, this, typeof(Tile));
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
    }
}
