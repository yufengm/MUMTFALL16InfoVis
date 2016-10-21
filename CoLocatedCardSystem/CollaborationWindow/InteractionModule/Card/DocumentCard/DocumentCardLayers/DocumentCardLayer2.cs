using CoLocatedCardSystem.CollaborationWindow.DocumentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    class DocumentCardLayer2 : DocumentCardLayerBase
    {
        ContentTouchView contentView = new ContentTouchView();
        Document doc;
        public DocumentCardLayer2(DocumentCardController cardController, DocumentCard card) : base(cardController, card)
        {
        }


        internal override async Task SetArticle(Document doc)
        {
            await base.SetArticle(doc);
            this.doc = doc;
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                contentView.Init(cardController, attachedCard, doc, ContentTouchView.LoadMode.Highlight, contentView.Width);
            });
        }

        /// <summary>
        /// Initialize the layer
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        internal override async void Init()
        {
            this.Width = attachedCard.Width;
            this.Height = attachedCard.Height;

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                //Move the textblock - 1 / 2 width and -1 / 2 height to the center.
                UIHelper.InitializeUI(
                    new Point(-0.5 * attachedCard.Width, -0.5 * attachedCard.Height),
                    0,
                    1,
                    new Size(this.Width, this.Height),
                    this);
                Grid grid = new Grid();
                // Create row definitions.
                RowDefinition rowDefinition1 = new RowDefinition();
                RowDefinition rowDefinition2 = new RowDefinition();
                rowDefinition1.Height = new GridLength(1,GridUnitType.Star);
                rowDefinition2.Height = new GridLength(6, GridUnitType.Star);

                grid.RowDefinitions.Add(rowDefinition1);
                grid.RowDefinitions.Add(rowDefinition2);

                TextBlock label = new TextBlock();
                label.Foreground = new SolidColorBrush(Colors.Black);
                label.Padding = new Thickness(0);
                label.LineHeight = 1;
                label.TextWrapping = TextWrapping.Wrap;
                label.FontSize = 4;
                label.FontStretch = FontStretch.Normal;
                label.Text = "High light:";
                grid.Children.Add(label);
                Grid.SetRow(label, 0);

                ScrollViewer contentSV = new ScrollViewer();
                contentSV.HorizontalScrollMode = ScrollMode.Disabled;
                contentSV.Width = attachedCard.Width;
                contentSV.Height = attachedCard.Height;
                contentView.Width = attachedCard.Width;
                contentSV.Content = contentView;
                contentSV.Padding = new Thickness(0);
                contentSV.Margin = new Thickness(0);
                grid.Children.Add(contentSV);
                Grid.SetRow(contentSV, 1);
                contentSV.VerticalAlignment = VerticalAlignment.Top;
                this.Children.Add(grid);
            });
        }
        internal override void DisableTouch()
        {
            base.DisableTouch();
            contentView.IsHitTestVisible = false;

        }
        internal override void EnableTouch()
        {
            base.EnableTouch();
            contentView.IsHitTestVisible = true;
        }
        /// <summary>
        /// Highlight tokens
        /// </summary>
        /// <param name="token"></param>
        internal override async void HighlightToken(Token token)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                contentView.Init(cardController, attachedCard, doc, ContentTouchView.LoadMode.Highlight, contentView.Width);
            });
        }
        /// <summary>
        /// Dehighlight tokens
        /// </summary>
        /// <param name="token"></param>
        internal override async void DehighlightToken(Token token)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                contentView.Init(cardController, attachedCard, doc, ContentTouchView.LoadMode.Highlight, contentView.Width);
            });
        }
    }
}
