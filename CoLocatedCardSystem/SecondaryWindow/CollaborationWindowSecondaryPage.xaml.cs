using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CoLocatedCardSystem.SecondaryWindow
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CollaborationWindowSecondaryPage : Page
    {
        public static CollaborationWindowSecondaryPage Current;
        AwareCloudController controller;
        Canvas container;
        public CollaborationWindowSecondaryPage()
        {
            this.InitializeComponent();
            this.Loaded += CollaborationWindowSecondaryPage_Loaded;
        }

        private void CollaborationWindowSecondaryPage_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
        }

        private async void Init()
        {
            var bounds = ApplicationView.GetForCurrentView().VisibleBounds;
            //SecondaryScreen.WIDTH = (int)bounds.Right;
            //SecondaryScreen.HEIGHT = (int)bounds.Bottom;
            SecondaryScreen.WIDTH = 1280;
            SecondaryScreen.HEIGHT = 720;
            container = new Canvas();
            container.Width = SecondaryScreen.WIDTH;
            container.Height = SecondaryScreen.HEIGHT;
            System.Diagnostics.Debug.WriteLine("secondary: " + SecondaryScreen.WIDTH + " " + SecondaryScreen.HEIGHT);
            SecondaryScreen.SCALE_FACTOR = 1.0f / (float) DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
            this.Width = SecondaryScreen.WIDTH;
            this.Height = SecondaryScreen.HEIGHT;
            ApplicationView.PreferredLaunchViewSize = new Size(SecondaryScreen.WIDTH, SecondaryScreen.HEIGHT);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.FullScreen;
            controller = new AwareCloudController();
            App app = App.Current as App;
            app.AwareCloudController = controller;
            controller.Init((int)this.Width, (int)this.Height);
            await Task.Delay(TimeSpan.FromSeconds(3));
            container.Children.Add(controller.BaseLayerController.BaseLayer);
            container.Children.Add(controller.SemanticLayerController.Semanticlayer);
            container.Children.Add(controller.CloudLayerController.CloudLayer);
            this.Content = container;
            //this.WordCloud.Width = this.Width;
            //this.WordCloud.Height = this.Height;
            //string src = "ms-appx-web:///Assets/p5/awarecloud.js/index.html";
            //this.WordCloud.Navigate(new Uri(src));
            //this.WordCloud.NavigationCompleted += WordCloud_NavigationCompleted;
        }

        private void WordCloud_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            App app = App.Current as App;
            //controller.init(this.WordCloud, app.CentralController);
        }
    }
}
