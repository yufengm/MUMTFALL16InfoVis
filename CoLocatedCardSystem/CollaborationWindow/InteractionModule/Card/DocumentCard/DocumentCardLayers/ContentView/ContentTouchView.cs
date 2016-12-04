using CoLocatedCardSystem.CollaborationWindow.DocumentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    class ContentTouchView : StackPanel
    {
        List<Tile> list = new List<Tile>();
        Document document;
        DocumentCardController controller;
        DocumentCard card;
        double textSize = 3;

        public double TextSize
        {
            get
            {
                return textSize;
            }

            set
            {
                textSize = value;
            }
        }

        internal enum LoadMode {
            ALL,
            KeyWord
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
            //this.ManipulationMode = Windows.UI.Xaml.Input.ManipulationModes.None;
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
                StackPanel horiPanel = new StackPanel();
                horiPanel.Orientation = Orientation.Horizontal;
                int rIndex = 0;
                
                foreach (ProcessedDocument pd in doc.ProcessedDocument) {
                    if (mode == LoadMode.ALL)
                    {
                        horiPanel = new StackPanel();
                        horiPanel.Width = this.Width;
                        horiPanel.Height = 20;
                        horiPanel.Orientation = Orientation.Horizontal;
                        string[] jpgs = doc.DocumentAttributes.Jpg[rIndex].Split(',');
                        foreach (string jpgfile in jpgs) {
                            BitmapImage bitmapImage = new BitmapImage(new Uri(@"ms-appx:///Assets/review/" + jpgfile));
                            if (bitmapImage != null) {
                                Image img = new Image();
                                img.Source = bitmapImage;
                                Size addjustedSize = new Size();
                                double tempHeight = (double)bitmapImage.PixelHeight * (horiPanel.Width / 3) / bitmapImage.PixelWidth;
                                if (tempHeight > horiPanel.Height)
                                {
                                    addjustedSize.Width = (double)bitmapImage.PixelWidth * horiPanel.Height / bitmapImage.PixelHeight;
                                    addjustedSize.Height = horiPanel.Height;
                                }
                                else
                                {
                                    addjustedSize.Width = this.Width / 3;
                                    addjustedSize.Height = tempHeight;
                                }
                                UIHelper.InitializeUI(new Point(0, 0),
                                    0,
                                    1,
                                    addjustedSize,
                                    img);
                                 horiPanel.Children.Add(img);
                            }
                        }
                        rIndex++;
                        currentWidth = horiPanel.Width;
                        currentHight += horiPanel.Height;
                        this.Children.Add(horiPanel);
                    }
                    Size emptyBox = UIHelper.GetBoundingSize(" ", textSize);
                    if (mode == LoadMode.ALL || mode == LoadMode.KeyWord)
                    {
                        horiPanel = new StackPanel();
                        horiPanel.Width = this.Width;
                        horiPanel.Height = 0;
                        horiPanel.Orientation = Orientation.Horizontal;
                        currentWidth = horiPanel.Width;
                        currentHight += emptyBox.Height;
                        this.Children.Add(horiPanel);
                    }
                    foreach (Token token in pd.List)
                    {
                        if (mode == LoadMode.KeyWord)
                        {
                            if (token.WordType == WordType.STOPWORD ||
                            token.WordType == WordType.PUNCTUATION ||
                            token.WordType == WordType.IRREGULAR ||
                            token.WordType == WordType.DEFAULT)
                            {
                                continue;
                            }
                        }
                        Size boxSize = UIHelper.GetBoundingSize(token.OriginalWord, textSize);
                        if (token.WordType == WordType.LINEBREAK)
                        {
                            horiPanel = new StackPanel();
                            horiPanel.Width = this.Width;
                            horiPanel.Height = boxSize.Height;
                            horiPanel.Orientation = Orientation.Horizontal;
                            currentWidth = boxSize.Width;
                            currentHight += boxSize.Height;
                            this.Children.Add(horiPanel);
                            continue;
                        }
                        if (horiPanel.Height < boxSize.Height)
                        {
                            horiPanel.Height = boxSize.Height;
                        }
                        currentWidth += boxSize.Width;
                        if (currentWidth > this.Width)
                        {
                            horiPanel = new StackPanel();
                            horiPanel.Width = this.Width;
                            horiPanel.Height = boxSize.Height;
                            horiPanel.Orientation = Orientation.Horizontal;
                            currentHight += boxSize.Height;
                            currentWidth = boxSize.Width;
                            this.Children.Add(horiPanel);
                        }
                        Tile tile = new Tile();
                        tile.Init(controller, card, token, textSize, boxSize);
                        list.Add(tile);
                        horiPanel.Children.Add(tile);
                        if (card.HighlightedTokens.Contains(token))
                        {
                            tile.HighLight();
                        }
                    }
                    horiPanel = new StackPanel();
                    horiPanel.Width = this.Width;
                    horiPanel.Height = emptyBox.Height;
                    horiPanel.Orientation = Orientation.Horizontal;
                    currentWidth = horiPanel.Width;
                    currentHight += emptyBox.Height;
                    this.Children.Add(horiPanel);
                }
                this.Height = currentHight;
            });
        }
    }
}
