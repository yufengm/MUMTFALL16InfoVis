﻿using CoLocatedCardSystem.SecondaryWindow.AwareCloudModule;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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

        public CollaborationWindowSecondaryPage()
        {
            this.InitializeComponent();
            this.Loaded += CollaborationWindowSecondaryPage_Loaded;
        }

        private void CollaborationWindowSecondaryPage_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
        }

        private void Init()
        {
            var bounds = ApplicationView.GetForCurrentView().VisibleBounds;
            SecondaryScreen.WIDTH = (int)bounds.Right;
            SecondaryScreen.HEIGHT = (int)bounds.Bottom;
            System.Diagnostics.Debug.WriteLine("secondary: " + SecondaryScreen.WIDTH + " " + SecondaryScreen.HEIGHT);
            SecondaryScreen.SCALE_FACTOR = 1 / DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
            this.Width = SecondaryScreen.WIDTH / SecondaryScreen.SCALE_FACTOR;
            this.Height = SecondaryScreen.HEIGHT / SecondaryScreen.SCALE_FACTOR;
            ApplicationView.PreferredLaunchViewSize = new Size(SecondaryScreen.WIDTH, SecondaryScreen.HEIGHT);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.FullScreen;
            controller = new AwareCloudController();
            App app = App.Current as App;
            app.AwareCloudController = controller;
            this.WordCloud.Width = this.Width;
            this.WordCloud.Height = this.Height;
            System.Diagnostics.Debug.WriteLine(SecondaryScreen.SCALE_FACTOR + " " + this.WordCloud.Width + " " + this.WordCloud.Height);
            string src = "ms-appx-web:///Assets/p5/awarecloud.js/index.html";
            this.WordCloud.Navigate(new Uri(src));
            this.WordCloud.NavigationCompleted += WordCloud_NavigationCompleted;
        }

        private void WordCloud_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            App app = App.Current as App;
            controller.init(this.WordCloud, app.CentralController);
        }
    }
}
