using DataAttribute = CoLocatedCardSystem.CollaborationWindow.TableModule.DataAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.Foundation;
using Windows.UI.Xaml.Documents;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    class AttributeCardLayer2 : AttributeCardLayerBase
    {
        StackPanel panel = new StackPanel();

        public AttributeCardLayer2(AttributeCardController controller, Card card) : base(controller, card)
        {
        }

        internal override async Task SetAttribute(DataAttribute attr)
        {
            await base.SetAttribute(attr);
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                String str = attr.GetInfo();//need to define Get All method in Attribute
                
                String[] attributes = str.Split('\n');
                TextBlock title = new TextBlock();
                title.Text = attr.Name;
                title.HorizontalAlignment = HorizontalAlignment.Center;
                panel.Children.Add(title);
                foreach (String attributeName in attributes)
                {
                    if (!attributeName.Equals("Most Common Items:"))
                    {
                        Button button = new Button();
                        button.Content = attributeName;
                        if (str.Length > 5)
                        {
                            button.FontSize = 10;

                        }
                        if (str.Length > 10)
                        {
                            button.FontSize = 6;
                        }
                        button.FontStretch = FontStretch.Normal;
                        button.FontWeight = FontWeights.Bold;
                        button.Foreground = new SolidColorBrush(Colors.Black);
                        button.Margin = new Thickness(1);
                        button.Padding = new Thickness(-1);
                        panel.Children.Add(button);
                    }
                    else
                    {
                        TextBlock textBlock = new TextBlock();
                        Underline ul = new Underline();
                        Run r = new Run();
                        r.Text = attributeName;

                        ul.Inlines.Add(r);
                        textBlock.Inlines.Add(ul);
                        if (str.Length > 5)
                        {
                            textBlock.FontSize = 10;

                        }
                        if (str.Length > 10)
                        {
                            textBlock.FontSize = 6;
                        }
                        textBlock.FontStretch = FontStretch.Normal;
                        textBlock.FontWeight = FontWeights.Bold;
                        textBlock.Foreground = new SolidColorBrush(Colors.Black);
                        textBlock.Margin = new Thickness(1);
                        textBlock.Padding = new Thickness(0);
                        panel.Children.Add(textBlock);
                    }
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
