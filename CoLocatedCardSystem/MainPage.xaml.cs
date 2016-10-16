using System;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CoLocatedCardSystem
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static MainPage Current;

        public MainPage()
        {
            this.InitializeComponent();
            Init();
        }
        /// <summary>
        /// Initialize the MainPage
        /// </summary>
        internal async void Init()
        {
            Current = this;
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.FullScreen;
        }
        /// <summary>
        /// Callback method of the start button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void startButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.CollaborationWindowLifeControl == null && this.SecondaryWindowLifeControl == null)
            {
                CoreApplicationView newView = CoreApplication.CreateNewView();
                int collaborationPageID = 0;
                CollaborationWindow.CollaborationWindowLifeEventControl cwLifeEventControl = null;
                String talbeDir= CSVFileDirTextBlock.Text.Trim();
                String docDir = JSONFileDirTextBlock.Text.Trim();
                await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    this.CollaborationWindowLifeControl = CollaborationWindow.CollaborationWindowLifeEventControl.CreateForCurrentView();
                    cwLifeEventControl = CollaborationWindow.CollaborationWindowLifeEventControl.CreateForCurrentView();
                    cwLifeEventControl.DocFileDir = docDir;
                    cwLifeEventControl.TableFileDir = talbeDir;
                    var frame = new Frame();
                    frame.Navigate(typeof(CollaborationWindow.CollaborationWindowMainPage), cwLifeEventControl);
                    Window.Current.Content = frame;
                    // You have to activate the window in order to show it later.
                    Window.Current.Activate();
                    collaborationPageID = ApplicationView.GetForCurrentView().Id;
                });
                newView = CoreApplication.CreateNewView();
                int secondaryPageID = 0;
                SecondaryWindow.SecondaryWindowLifeEventControl swLifeEventControl = null;
                await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    this.SecondaryWindowLifeControl = SecondaryWindow.SecondaryWindowLifeEventControl.CreateForCurrentView();
                    swLifeEventControl = SecondaryWindow.SecondaryWindowLifeEventControl.CreateForCurrentView();
                    var frame = new Frame();
                    frame.Navigate(typeof(SecondaryWindow.CollaborationWindowSecondaryPage), swLifeEventControl);
                    Window.Current.Content = frame;
                    // You have to activate the window in order to show it later.
                    Window.Current.Activate();
                    secondaryPageID = ApplicationView.GetForCurrentView().Id;
                });

                bool viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(secondaryPageID);
                viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(collaborationPageID);
                try
                {
                    // Show the view on a second display (if available) or on the primary display
                    await ProjectionManager.StartProjectingAsync(SecondaryWindowLifeControl.Id, ApplicationView.GetForCurrentView().Id);

                    swLifeEventControl.StartViewInUse();
                    cwLifeEventControl.StartViewInUse();
                }
                catch (InvalidOperationException)
                {
                }
            }
        }
    }
}
