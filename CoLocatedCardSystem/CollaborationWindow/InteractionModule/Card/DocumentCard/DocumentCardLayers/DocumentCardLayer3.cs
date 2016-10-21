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
    class DocumentCardLayer3 : DocumentCardLayerBase
    {
        ContentTouchView contentView = new ContentTouchView();
        public DocumentCardLayer3(DocumentCardController cardController, DocumentCard card) : base(cardController, card)
        {

        }

        internal override async Task SetArticle(Document doc)
        {
            await base.SetArticle(doc);
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                contentView.Init(cardController, attachedCard, doc, ContentTouchView.LoadMode.KeyWord, contentView.Width);
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

                ScrollViewer contentSV = new ScrollViewer();
                contentSV.HorizontalScrollMode = ScrollMode.Disabled;
                contentSV.Width = attachedCard.Width;
                contentSV.Height = attachedCard.Height;
                contentView.Width = attachedCard.Width;
                contentSV.Content = contentView;
                contentSV.Padding = new Thickness(0);
                contentSV.Margin = new Thickness(0);
                this.Children.Add(contentSV);
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
                contentView.HighLight(token);
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
                contentView.DeHighLight(token);
            });
        }
    }
}
