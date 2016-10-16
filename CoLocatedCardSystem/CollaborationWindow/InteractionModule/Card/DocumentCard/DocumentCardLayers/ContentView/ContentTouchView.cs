using CoLocatedCardSystem.CollaborationWindow.DocumentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    class ContentTouchView : StackPanel
    {
        List<Tile> list = new List<Tile>();
        Document document;
        DocumentCardController controller;
        DocumentCard card;
        internal enum LoadMode {
            ALL,
            KeyWord,
            Highlight
        }
        /// <summary>
        /// Initialize the view.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="width"></param>
        internal async void Init(DocumentCardController controller, DocumentCard card, Document doc, LoadMode mode, double width)
        {
            this.document = doc;
            this.Width = width;
            this.controller = controller;
            this.card = card;
            await InitUI(doc,mode);
        }
        /// <summary>
        /// Hightlight the word
        /// </summary>
        /// <param name="token"></param>
        internal async void HighLight(Token token)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                foreach (Tile t in list) {
                    if (t.Token.Equals(token)) {
                        t.HighLight();
                    }
                }
            });
         }
        /// <summary>
        /// Remove the high light of the token
        /// </summary>
        /// <param name="token"></param>
        internal async void DeHighLight(Token token)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                foreach (Tile t in list)
                {
                    if (t.Token.Equals(token))
                    {
                        t.DeHighLight();
                    }
                }
            });
        }
        /// <summary>
        /// Initialize the UI of the content touch view. the words are aligned in a horizontal
        /// Stackpanel, and horizontal Stackpanels are filled in to a vertical Stackpanel. Key
        /// words are showed in Tile, other words are in TextBlock.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private async Task InitUI(Document doc, LoadMode mode)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                this.Children.Clear();
                this.Orientation = Orientation.Vertical;
                double currentHight = 0;
                double currentWidth = 0;
                this.BorderThickness = new Thickness(0);
                double textSize = 4;
                StackPanel horiPanel = new StackPanel();
                horiPanel.Orientation = Orientation.Horizontal;
                foreach (Token token in doc.ProcessedDocument.List)
                {
                    if (mode == LoadMode.KeyWord)
                    {
                        if (token.WordType == WordType.STOPWORD ||
                        token.WordType == WordType.REGULAR ||
                        token.WordType == WordType.PUNCTUATION ||
                        token.WordType == WordType.IRREGULAR ||
                        token.WordType == WordType.DEFAULT)
                        {
                            continue;
                        }
                    }
                    else if (mode == LoadMode.Highlight) {
                        if (!card.HighlightedTokens.Contains(token))
                        {
                            continue;
                        }
                    }
                    Size boxSize = UIHelper.GetBoundingSize(token.OriginalWord, textSize);
                    if (token.WordType == WordType.LINEBREAK) {
                        horiPanel = new StackPanel();
                        horiPanel.Width = this.Width;
                        horiPanel.Height = boxSize.Height;
                        horiPanel.Orientation = Orientation.Horizontal;
                        currentWidth = boxSize.Width;
                        currentHight += boxSize.Height;
                        this.Children.Add(horiPanel);
                        continue;
                    }
                    if (currentHight == 0) {
                        currentHight = boxSize.Height;
                        horiPanel = new StackPanel();
                        horiPanel.Width = this.Width;
                        horiPanel.Height = boxSize.Height;
                        horiPanel.Orientation = Orientation.Horizontal;
                        this.Children.Add(horiPanel);
                    }
                    currentWidth += boxSize.Width;
                    if (currentWidth > this.Width) {
                        horiPanel = new StackPanel();
                        horiPanel.Width = this.Width;
                        horiPanel.Height = boxSize.Height;
                        horiPanel.Orientation = Orientation.Horizontal;
                        currentWidth = boxSize.Width;
                        currentHight += boxSize.Height;
                        this.Children.Add(horiPanel);
                    }
                    Tile tile = new Tile();
                    tile.Init(controller,card, token, textSize, boxSize);
                    list.Add(tile);
                    horiPanel.Children.Add(tile);
                    if (card.HighlightedTokens.Contains(token))
                    {
                        tile.HighLight();
                    }
                }
                this.Height = currentHight;
            });
        }
    }
}
