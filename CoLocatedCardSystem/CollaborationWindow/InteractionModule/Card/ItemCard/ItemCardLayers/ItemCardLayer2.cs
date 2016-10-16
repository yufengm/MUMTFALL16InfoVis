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
    class ItemCardLayer2 : ItemCardLayerBase
    {
        StackPanel panel = new StackPanel();

        public ItemCardLayer2(Card card) : base(card)
        {

        }
        internal override async Task SetItem(DataRow item)
        {
            await base.SetItem(item);
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                String str = item.GetAll();
                String[] items = str.Split('\n');

                foreach (String itemName in items)
                {
                    Button button = new Button();
                    button.Content = itemName;
                    if (str.Length > 50)
                    {
                        button.FontSize = 6;
                    }
                    if (str.Length > 100)
                    {
                        button.FontSize = 4;
                    }
                    button.FontStretch = FontStretch.Normal;
                    button.FontWeight = FontWeights.Bold;
                    button.Foreground = new SolidColorBrush(Colors.Black);
                    button.Margin = new Thickness(1);
                    button.Padding = new Thickness(-1);
                    panel.Children.Add(button);
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

                panel.Width = attachedCard.Width;
                //panel.Padding = new Thickness(-1);
                
                //contentTextBlock.LineHeight = 1;
                //contentTextBlock.TextWrapping = TextWrapping.Wrap;
                //contentTextBlock.TextAlignment = TextAlignment.Left;
                ScrollViewer sv = new ScrollViewer();
                sv.Width = attachedCard.Width;
                sv.Height = attachedCard.Height;
                sv.HorizontalAlignment = HorizontalAlignment.Center;
                sv.VerticalAlignment = VerticalAlignment.Center;
                sv.Content = panel;
                //sv.Background = new SolidColorBrush(Colors.Green);
                this.Children.Add(sv);
            });
        }
    }
}
