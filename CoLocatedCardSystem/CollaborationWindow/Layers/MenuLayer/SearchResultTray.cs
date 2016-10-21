using System;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Input;
using Windows.Foundation;
using Windows.UI.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;
using CoLocatedCardSystem.CollaborationWindow.Layers.Card_Layer;
using CoLocatedCardSystem.CollaborationWindow.InteractionModule;

namespace CoLocatedCardSystem.CollaborationWindow.Layers.Menu_Layer
{
    class SearchResultTray : Canvas
    {
        ScrollViewer scrollViewer;
        StackPanel stackPanel;
        Size blockSize = new Size(160 * Screen.SCALE_FACTOR, 130 * Screen.SCALE_FACTOR);
        DocumentCard[] currentSearchResult = null;
        Button hideButton;
        int currentStart = 0;
        int currentEnd = 0;
        int numToAdd = 10;
        int numToShow = 10;
        MenuLayerController menuLayerController;
        Storyboard showBoard = new Storyboard();
        Storyboard hideBoard = new Storyboard();
        public SearchResultTray(MenuLayerController controller)
        {
            this.menuLayerController = controller;
        }
        public void Init(MenuBarInfo info)
        {
            this.Background = new SolidColorBrush(Colors.LightBlue);//for debug
            this.Width = info.SearchResultInfo.Size.Width;
            this.Height = info.SearchResultInfo.Size.Height;
            scrollViewer = new ScrollViewer();
            scrollViewer.Width = info.SearchResultInfo.Size.Width;
            scrollViewer.Height = info.SearchResultInfo.Size.Height;
            scrollViewer.HorizontalScrollMode = ScrollMode.Enabled;
            scrollViewer.VerticalScrollMode = ScrollMode.Disabled;
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            stackPanel = new StackPanel();
            stackPanel.Height = scrollViewer.Height;
            stackPanel.Orientation = Orientation.Horizontal;
            scrollViewer.Content = stackPanel;
            scrollViewer.ViewChanged += ScrollViewer_ViewChanged;
            hideButton = new Button();
            hideButton.Width = 40;
            hideButton.Height = 20;
            hideButton.Content = "Hide";
            hideButton.Padding = new Thickness(0);
            UIHelper.InitializeUI(new Point(this.Width - hideButton.Width, -hideButton.Height),
                0, 1, new Size(hideButton.Width, hideButton.Height), hideButton);
            hideButton.Click += HideButton_Click;

            showBoard.Duration = TimeSpan.FromSeconds(1);
            DoubleAnimation anim1 = new DoubleAnimation();
            anim1.SpeedRatio = 2;
            anim1.From = this.Height + info.Size.Height;
            anim1.To = 0;
            showBoard.Children.Add(anim1);
            Storyboard.SetTarget(anim1, this);
            Storyboard.SetTargetProperty(anim1, "Canvas.Top");

            hideBoard.Duration = TimeSpan.FromSeconds(1);
            DoubleAnimation anim2 = new DoubleAnimation();
            anim2.SpeedRatio = 2;
            anim2.From = 0;
            anim2.To = this.Height + info.Size.Height;
            hideBoard.Children.Add(anim2);
            Storyboard.SetTarget(anim2, this);
            Storyboard.SetTargetProperty(anim2, "Canvas.Top");

            this.Children.Add(hideButton);
            this.Children.Add(scrollViewer);
        }

        private void HideButton_Click(object sender, RoutedEventArgs e)
        {

            if (hideButton.Content.Equals("Hide"))
            {
                hideBoard.Begin();
                hideButton.Content = "Show";
            }
            else
            {
                showBoard.Begin();
                hideButton.Content = "Hide";
            }
        }

        private async void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var horizontalOffset = scrollViewer.HorizontalOffset;
            var maxHorizontalOffset = scrollViewer.ScrollableWidth;
            if (maxHorizontalOffset == 0 ||
                horizontalOffset == maxHorizontalOffset)
            {
                if (currentEnd < currentSearchResult.Length)
                {
                    stackPanel.Children.Clear();
                    currentEnd = (numToAdd + currentEnd) < currentSearchResult.Length
                        ? (numToAdd + currentEnd) : currentSearchResult.Length;
                    currentStart = currentEnd - numToShow;
                    await ShowCard(currentStart, currentEnd);
                    scrollViewer.ChangeView(maxHorizontalOffset * 0.5, null, null);
                }
            }
            else if (horizontalOffset == 0)
            {
                if (currentStart > 0)
                {
                    currentStart = (currentStart - numToAdd) > 0 ? (currentStart - numToAdd) : 0;
                    currentEnd = currentStart + numToShow;
                    await ShowCard(currentStart, currentEnd);
                    scrollViewer.ChangeView(maxHorizontalOffset * 0.5, null, null);
                }
            }
        }

        /// <summary>
        /// Load the cards to the search result tray
        /// </summary>
        /// <param name="cards"></param>
        public async void AddCards(DocumentCard[] cards)
        {
            if (cards != null)
            {
                this.currentSearchResult = cards;
                currentStart = 0;
                currentEnd = numToShow < cards.Length ? numToShow : cards.Length;
                await ShowCard(currentStart, currentEnd);
            }
        }
        /// <summary>
        /// Shuo the card from start to end (exd.)
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private async Task ShowCard(int start, int end)
        {
            if (start >= 0 && start < currentSearchResult.Length && end > 0 && end <= currentSearchResult.Length && end > start)
            {
                stackPanel.Children.Clear();
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    for (int i = start; i < end; i++)
                    {
                        Canvas canvas = new Canvas();
                        canvas.Width = blockSize.Width;
                        canvas.Height = blockSize.Height;
                        if (!menuLayerController.IsCardOnTable(currentSearchResult[i]))
                        {
                            ResultCard resultCard = new ResultCard(currentSearchResult[i].CardController);
                            resultCard.MenuLayerController = menuLayerController;
                            resultCard.Init(currentSearchResult[i].CardID, currentSearchResult[i].Owner, currentSearchResult[i].Document);
                            await resultCard.LoadUI();
                            resultCard.MoveTo(new Point(canvas.Width / 2, canvas.Height / 2));
                            resultCard.Block = canvas;
                            canvas.Children.Add(resultCard);
                        }
                        stackPanel.Children.Add(canvas);
                    }
                    stackPanel.Width = blockSize.Width * (end - start);
                });
            }
        }

    }
}
