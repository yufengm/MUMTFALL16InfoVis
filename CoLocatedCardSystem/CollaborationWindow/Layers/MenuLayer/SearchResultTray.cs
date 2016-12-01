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
using System.Collections.Generic;

namespace CoLocatedCardSystem.CollaborationWindow.Layers.Menu_Layer
{
    class SearchResultTray : Canvas
    {
        ScrollViewer scrollViewer;
        StackPanel stackPanel;
        Size blockSize = new Size(160 * Screen.SCALE_FACTOR, 130 * Screen.SCALE_FACTOR);
        DocumentCard[] currentSearchResult = null;
        List<Canvas> stackCanvas = new List<Canvas>();
        Button hideButton;
        TextBlock resultNum;
        int MAXCARDTOSHOW = 7;
        int cardToShow = 7;
        MenuLayerController menuLayerController;
        Storyboard showBoard = new Storyboard();
        Storyboard hideBoard = new Storyboard();
        MenuBarInfo info;
        public SearchResultTray(MenuLayerController controller)
        {
            this.menuLayerController = controller;
        }
        public void Init(MenuBarInfo info)
        {
            this.info = info;
            this.Background = new SolidColorBrush(Color.FromArgb(255,209,219,189));
            this.Width = this.info.SearchResultInfo.Size.Width;
            this.Height = this.info.SearchResultInfo.Size.Height;
            scrollViewer = new ScrollViewer();
            scrollViewer.Width = this.info.SearchResultInfo.Size.Width;
            scrollViewer.Height = this.info.SearchResultInfo.Size.Height;
            scrollViewer.HorizontalScrollMode = ScrollMode.Enabled;
            scrollViewer.VerticalScrollMode = ScrollMode.Disabled;
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            stackPanel = new StackPanel();
            stackPanel.Height = scrollViewer.Height;
            stackPanel.Orientation = Orientation.Horizontal;
            scrollViewer.Content = stackPanel;
            MAXCARDTOSHOW = (int)(this.info.SearchResultInfo.Size.Width / blockSize.Width) + 1;
            scrollViewer.ViewChanged += ScrollViewer_ViewChanged;

            hideButton = new Button();
            hideButton.Width = 40;
            hideButton.Height = 20;
            hideButton.Content = "Hide";
            hideButton.Padding = new Thickness(0);
            hideButton.Foreground = new SolidColorBrush(MyColor.Wheat);
            UIHelper.InitializeUI(new Point(this.Width - hideButton.Width, -hideButton.Height),
                0, 1, new Size(hideButton.Width, hideButton.Height), hideButton);
            hideButton.Click += HideButton_Click;


            resultNum = new TextBlock();
            resultNum.Width = 300;
            resultNum.Height = 20;
            resultNum.Text = "";
            resultNum.FontSize = 12;
            resultNum.IsHitTestVisible = false;
            resultNum.Padding = new Thickness(0);
            resultNum.Foreground = new SolidColorBrush(MyColor.Wheat);
            UIHelper.InitializeUI(new Point(0, -resultNum.Height),
                0, 1, new Size(resultNum.Width, resultNum.Height), resultNum);

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

            this.Children.Add(resultNum);
            this.Children.Add(hideButton);
            this.Children.Add(scrollViewer);
        }

        internal void RecycleCurrentCards()
        {
            List<string> docIDs = new List<string>();
            if (this.currentSearchResult != null&&currentSearchResult.Length>0) {
                foreach (DocumentCard card in currentSearchResult) {
                    docIDs.Add(card.Document.DocID);
                }
            }
            if (docIDs.Count > 0)
            {
                menuLayerController.Controllers.SemanticGroupController.SetSearchResult(docIDs.ToArray(), info.Owner, false);
            }
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
        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            ShowCard(scrollViewer.HorizontalOffset);
        }

        /// <summary>
        /// Load the cards to the search result tray
        /// </summary>
        /// <param name="cards"></param>
        internal void AddCards(string content, DocumentCard[] cards)
        {
            if (cards != null)
            {
                this.currentSearchResult = cards;
                stackPanel.Children.Clear();
                stackCanvas.Clear();
                cardToShow = MAXCARDTOSHOW > cards.Length ? cards.Length : MAXCARDTOSHOW;
                List<string> docIDs = new List<string>();
                foreach (DocumentCard card in cards)
                {
                    Canvas canvas = new Canvas();
                    canvas.Width = blockSize.Width;
                    canvas.Height = blockSize.Height;
                    stackPanel.Children.Add(canvas);
                    stackCanvas.Add(canvas);
                    docIDs.Add(card.Document.DocID);
                }
                if (docIDs.Count > 0) {
                    menuLayerController.Controllers.SemanticGroupController.SetSearchResult(docIDs.ToArray(), info.Owner, true);
                }
                menuLayerController.Controllers.ConnectionController.UpdateSemanticCloud();
                ShowCard(0);
                resultNum.Text = "Search: "+ content + " Result: " + cards.Length;
                
            }
        }
        /// <summary>
        /// Re-enable the card in the search result tray
        /// </summary>
        internal void EnableResultCard(string cardID) {
            foreach (Canvas block in stackCanvas) {
                if (block.Children.Count != 0) {
                    ResultCard resultCard = block.Children[0] as ResultCard;
                    if (resultCard != null && resultCard.CardID == cardID) {
                        resultCard.EnableCard();
                    }
                }
            }
        }
        /// <summary>
        /// Remove the high light of all highlight card
        /// </summary>
        internal void RemoveUnusedHighlight()
        {
            foreach (Canvas block in stackCanvas)
            {
                ResultCard rc = block.Children[0] as ResultCard;
                if (rc != null && rc.IsEnabled)
                {
                    menuLayerController.DehighLightAll(rc.CardID);
                }
            }
        }
        private async void ShowCard(double position)
        {
            int startCardID = (int)(position / blockSize.Width);
            if (startCardID + cardToShow >= stackCanvas.Count)
            {
                startCardID = stackCanvas.Count - cardToShow;
            }
            if (startCardID < 0) { 
                startCardID = 0;
            }
            if (stackCanvas.Count > 0)
            {
                for (int i = 0; i < startCardID; i++)
                {
                    if (stackCanvas[i].Children.Count != 0)
                        stackCanvas[i].Children.Clear();
                }
                for (int i = startCardID, endID = startCardID + cardToShow; i < endID; i++)
                {
                    if (stackCanvas[i].Children.Count == 0)
                    {
                        ResultCard resultCard = new ResultCard(menuLayerController.Controllers.CardController);
                        resultCard.MenuLayerController = menuLayerController;
                        resultCard.Init(currentSearchResult[i].CardID, currentSearchResult[i].Owner, currentSearchResult[i].Document);
                        await resultCard.LoadUI();
                        resultCard.MoveTo(new Point(stackCanvas[i].Width / 2, stackCanvas[i].Height / 2));
                        resultCard.Block = stackCanvas[i];

                        if (menuLayerController.Controllers.CardController.IsCardOnTable(currentSearchResult[i].CardID))
                        {
                            resultCard.DisableCard();
                        }
                        else {
                            resultCard.EnableCard();
                        }
                        stackCanvas[i].Children.Add(resultCard);
                    }
                }
                for (int i = startCardID + cardToShow+1; i < stackCanvas.Count; i++)
                {
                    if (stackCanvas[i].Children.Count != 0)
                        stackCanvas[i].Children.Clear();
                }
            }
        }
    }
}
