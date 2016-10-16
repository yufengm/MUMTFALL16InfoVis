﻿using CoLocatedCardSystem.CollaborationWindow.DocumentModule;
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
    class DocumentCardLayer4 : DocumentCardLayerBase
    {
        ContentTouchView contentView = new ContentTouchView();

        public DocumentCardLayer4(DocumentCardController cardController, DocumentCard card) : base(cardController, card)
        {
        }
        /// <summary>
        /// Add the article to the doc.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        internal override async Task SetArticle(Document doc)
        {
            await base.SetArticle(doc);
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                contentView.Init(cardController, attachedCard, doc,ContentTouchView.LoadMode.ALL,contentView.Width);
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
        /// <summary>
        /// Hightlight the content
        /// </summary>
        /// <param name="token"></param>
        internal void HightLight(Token token)
        {
            contentView.HighLight(token);
        }

        internal void DeHightLight(Token token)
        {
            contentView.DeHighLight(token);
        }
    }
}
