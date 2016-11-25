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
    class DocumentCardLayer1 : DocumentCardLayerBase
    {
        TextBlock titleTextBlock = new TextBlock();

        public DocumentCardLayer1(DocumentCardController cardController, DocumentCard card) : base(cardController, card)
        {
        }
        /// <summary>
        /// Initialize the card layers
        /// </summary>
        /// <param name="doc"></param>
        internal override async Task SetArticle(Document doc)
        {
            await base.SetArticle(doc);
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,() =>
            {
                titleTextBlock.Text = doc.GetName();
                double fsize = 42 * Math.Pow(doc.GetName().Length, -0.43);
                if (fsize > 16)
                {
                    fsize = 16;
                }
                titleTextBlock.FontSize = fsize;
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
                Calculator.InitializeUI(
                    new Point(-0.5 * attachedCard.Width, -0.5 * attachedCard.Height),
                    0,
                    1,
                    new Size(this.Width, this.Height),
                    this);
                titleTextBlock.Width = attachedCard.Width;
                titleTextBlock.Height = attachedCard.Height;
                titleTextBlock.Foreground = new SolidColorBrush(MyColor.Wheat);
                titleTextBlock.LineHeight = 1;
                titleTextBlock.TextWrapping = TextWrapping.Wrap;
                titleTextBlock.TextAlignment = TextAlignment.Center;
                titleTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
                titleTextBlock.VerticalAlignment = VerticalAlignment.Center;
                titleTextBlock.FontStretch = FontStretch.Normal;
                titleTextBlock.FontWeight = FontWeights.Bold;
                titleTextBlock.IsHitTestVisible = false;
                this.Children.Add(titleTextBlock);
            });
        }
    }
}
