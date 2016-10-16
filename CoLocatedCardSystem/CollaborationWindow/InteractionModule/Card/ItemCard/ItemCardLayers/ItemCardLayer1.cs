using CoLocatedCardSystem.CollaborationWindow.TableModule;
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
    class ItemCardLayer1 : ItemCardLayerBase
    {
        TextBlock contentTextBlock = new TextBlock();
        public ItemCardLayer1(Card card) : base(card)
        {
        }
        internal override async Task SetItem(DataRow item)
        {
            await base.SetItem(item);
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                contentTextBlock.Text = item.GetIndex();
                if (item.GetIndex().Length > 50)
                {
                    contentTextBlock.FontSize = 15;

                }
                if (item.GetIndex().Length > 100)
                {
                    contentTextBlock.FontSize = 12;
                }
            });
        }
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
                contentTextBlock.Width = attachedCard.Width;
                contentTextBlock.Height = attachedCard.Height;
                contentTextBlock.Foreground = new SolidColorBrush(Colors.Black);
                contentTextBlock.LineHeight = 1;
                contentTextBlock.TextWrapping = TextWrapping.Wrap;
                contentTextBlock.FontSize = 20;
                contentTextBlock.TextAlignment = TextAlignment.Center;
                contentTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
                contentTextBlock.VerticalAlignment = VerticalAlignment.Center;
                contentTextBlock.FontStretch = FontStretch.Normal;
                contentTextBlock.FontWeight = FontWeights.Bold;
                this.Children.Add(contentTextBlock);
            });
        }
    }
}
