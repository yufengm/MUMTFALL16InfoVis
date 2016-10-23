using System;
using System.Linq;
using System.Threading.Tasks;
using VirtualKeyboard;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using CoLocatedCardSystem.CollaborationWindow.Layers.Card_Layer;
using CoLocatedCardSystem.CollaborationWindow.InteractionModule;

namespace CoLocatedCardSystem.CollaborationWindow.Layers.Menu_Layer
{
    class MenuBar : Canvas
    {
        MenuLayerController menuLayerController;
        OnScreenKeyBoard virtualKeyboard;
        TextBox textbox;
        TextInputButton createSortingBoxButton;
        User owner;
        DeleteButton deleteButton;
        TextInputButton searchButton;
        TextInputButton currentInputView = null;//Which input target the view is targeting
        SearchResultTray searchResultTray;
        public MenuBar(MenuLayerController controller)
        {
            this.menuLayerController = controller;
        }
        /// <summary>
        /// Initialize the menubar
        /// </summary>
        /// <param name="user"></param>
        internal void Init(User user)
        {
            MenuBarInfo info = MenuBarInfo.GetMenuBarInfo(user);
            this.owner = user;
            UIHelper.InitializeUI(info.Position, info.Rotate, info.Scale, info.Size, this);
            LoadUI(info);
        }
        /// <summary>
        /// Load the buttons and other ui elements on the menu
        /// </summary>
        /// <param name="info"></param>
        private void LoadUI(MenuBarInfo info)
        {
            LoadVirtualKeyboard(info);
            LoadDeleteButton(info);
            LoadSearchButton(info);
            LoadCreatingSortingBoxButton(info);
            LoadSearchResultTray(info);
            //Initialize the menubar
            ImageBrush brush = new ImageBrush();
            brush.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/menu_bg.png"));
            this.Background = brush;
            //this.Children.Add(createSortingBoxButton);
            this.Children.Add(deleteButton);
            this.Children.Add(searchButton);
            this.Children.Add(searchResultTray);
            this.Children.Add(virtualKeyboard);
            this.Children.Add(textbox);
        }
        /// <summary>
        /// Destroy the menubar.
        /// </summary>
        internal void Deinit()
        {
            UnregisterPointerEvent(createSortingBoxButton);
            createSortingBoxButton.Click -= KeyboardButton_Click;
            UnregisterPointerEvent(virtualKeyboard);
            virtualKeyboard.Disable();
            virtualKeyboard = null;
            UnregisterPointerEvent(deleteButton);
            searchButton.Click -= KeyboardButton_Click;
            UnregisterPointerEvent(searchButton);
            UnregisterPointerEvent(searchResultTray);
        }

        internal void RemoveUnusedHighlight()
        {
            searchResultTray.RemoveUnusedHighlight();
        }

        /// <summary>
        /// Add all the cards to the search result tray
        /// </summary>
        /// <param name="cards"></param>
        internal void ShowCardsInSearchResultTray(string content, DocumentCard[] cards)
        {
            searchResultTray.AddCards(content, cards);
        }

        private void LoadCreatingSortingBoxButton(MenuBarInfo info)
        {
            //Initialize the button to show the keyboard
            createSortingBoxButton = new TextInputButton();
            createSortingBoxButton.Init("Create a Box", "Close", virtualKeyboard, textbox);
            createSortingBoxButton.Click += KeyboardButton_Click;
            RegisterPointerEvent(createSortingBoxButton);
            createSortingBoxButton.IsTextScaleFactorEnabled = false;
            UIHelper.InitializeUI(info.SortingBoxButtonInfo.Position, 0, 1, info.SortingBoxButtonInfo.Size, createSortingBoxButton);
        }
        private void LoadDeleteButton(MenuBarInfo info)
        {
            deleteButton = new DeleteButton();
            deleteButton.Init(info);
            RegisterPointerEvent(deleteButton);
            UIHelper.InitializeUI(info.DeleteButtonInfo.Position, 0, 1, info.DeleteButtonInfo.Size, deleteButton);
        }
        private void LoadVirtualKeyboard(MenuBarInfo info)
        {
            //Initialize the text block
            textbox = new TextBox();
            textbox.AcceptsReturn = true;
            UIHelper.InitializeUI(info.InputTextBoxInfo.Position, 0, 1, info.InputTextBoxInfo.Size, textbox);
            textbox.Visibility = Visibility.Collapsed;
            textbox.TextChanged += Textbox_TextChanged;
            textbox.IsEnabled = false;
            //Initialize the keyboard to create the sorting box
            virtualKeyboard = new OnScreenKeyBoard();
            virtualKeyboard.InitialLayout = KeyboardLayouts.English;
            virtualKeyboard.Visibility = Visibility.Collapsed;
            RegisterPointerEvent(virtualKeyboard);
            UIHelper.InitializeUI(info.KeyboardInfo.Position, 0, 1, info.KeyboardInfo.Size, virtualKeyboard);
        }
        private void LoadSearchButton(MenuBarInfo info)
        {
            //Initialize search button
            searchButton = new TextInputButton();
            searchButton.Init("Search", "Close", virtualKeyboard, textbox);
            searchButton.Click += KeyboardButton_Click;
            RegisterPointerEvent(searchButton);
            searchButton.IsTextScaleFactorEnabled = false;
            UIHelper.InitializeUI(info.SearchButtonInfo.Position, 0, 1, info.SearchButtonInfo.Size, searchButton);
        }
        private void LoadSearchResultTray(MenuBarInfo info)
        {
            //Initialize search button
            searchResultTray = new SearchResultTray(menuLayerController);
            searchResultTray.Init(info);
            RegisterPointerEvent(searchResultTray);
            UIHelper.InitializeUI(info.SearchResultInfo.Position, 0, 1, info.SearchResultInfo.Size, searchResultTray);
        }
        private void RegisterPointerEvent(FrameworkElement element)
        {
            element.PointerPressed += PointerDown;
            element.PointerMoved += PointerMove;
            element.PointerExited += PointerUp;
            element.PointerReleased += PointerUp;
            element.PointerCaptureLost += PointerUp;
            element.PointerCanceled += PointerUp;
        }
        private void UnregisterPointerEvent(FrameworkElement element)
        {
            element.PointerPressed -= PointerDown;
            element.PointerMoved -= PointerMove;
            element.PointerExited -= PointerUp;
            element.PointerReleased -= PointerUp;
            element.PointerCaptureLost -= PointerUp;
            element.PointerCanceled -= PointerUp;
        }
        /// <summary>
        /// Detects when the an enter is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (textbox.Text.Length > 0 && textbox.Text.Last<char>().Equals('\n'))
                {
                    if (currentInputView == createSortingBoxButton)
                    {
                        CreateSortingBox(textbox.Text.TrimEnd());
                    }
                    else if (currentInputView == searchButton)
                    {
                        SearchContent(textbox.Text.TrimEnd());
                    }
                    currentInputView.Close();
                    currentInputView = null;
                }
            });
        }
        /// <summary>
        /// Configure the keyboard and the textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void KeyboardButton_Click(object sender, RoutedEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (currentInputView == null)
                {
                    if (sender == createSortingBoxButton)
                    {
                        currentInputView = createSortingBoxButton;
                        createSortingBoxButton.Open();
                    }
                    else if (sender == searchButton)
                    {
                        currentInputView = searchButton;
                        searchButton.Open();
                    }
                }
                else if (currentInputView != null && currentInputView.Equals(sender))
                {
                    currentInputView.Close();
                    currentInputView = null;
                }
                else
                {
                    currentInputView.Close();
                    if (sender == createSortingBoxButton)
                    {
                        currentInputView = createSortingBoxButton;
                        createSortingBoxButton.Open();
                    }
                    else if (sender == searchButton)
                    {
                        currentInputView = searchButton;
                        searchButton.Open();
                    }
                }
            });
        }

        /// <summary>
        /// Ask the interaction module to add a sorting box
        /// </summary>
        /// <param name="content"></param>
        private void CreateSortingBox(string content)
        {
            menuLayerController.CreateSortingBox(owner, content);
        }
        /// <summary>
        /// Search the document set and show the document card in the box.
        /// </summary>
        /// <param name="content"></param>
        private void SearchContent(string content)
        {
            menuLayerController.SearchDocumentCard(owner, content);
        }
        /// <summary>
        /// Check if the point is intersect with delete button
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        internal async Task<bool> IsIntersectWithDelete(Point position)
        {
            bool isIntersect = false;
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                //isIntersect = Coordination.IsIntersect(position, deleteButton, false);
            });
            return isIntersect;
        }

        /// <summary>
        /// Call back method for Pointer down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PointerDown(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint localPoint = e.GetCurrentPoint(this);
            PointerPoint globalPoint = e.GetCurrentPoint(Coordination.Baselayer);
            Type type = null;
            if (sender == createSortingBoxButton)
            {
                type = typeof(TextInputButton);
            }
            else if (sender == virtualKeyboard)
            {
                type = typeof(OnScreenKeyBoard);
            }
            else if (sender == deleteButton)
            {
                type = typeof(DeleteButton);
            }
            else if (sender == searchButton)
            {
                type = typeof(TextInputButton);
            }
            else if (sender == searchResultTray)
            {
                type = typeof(SearchResultTray);
            }
            menuLayerController.PointerDown(localPoint, globalPoint, sender, type);
        }
        /// <summary>
        /// Call back method for Pointer move
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PointerMove(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint localPoint = e.GetCurrentPoint(this);
            PointerPoint globalPoint = e.GetCurrentPoint(Coordination.Baselayer);
            menuLayerController.PointerMove(localPoint, globalPoint);
        }
        /// <summary>
        /// Call back method for pointer up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PointerUp(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint localPoint = e.GetCurrentPoint(this);
            PointerPoint globalPoint = e.GetCurrentPoint(Coordination.Baselayer);
            menuLayerController.PointerUp(localPoint, globalPoint);
        }
    }
}
