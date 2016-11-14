using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
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

namespace CoLocatedCardSystem.CollaborationWindow
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CollaborationWindowMainPage : Page
    {
        CentralControllers controllers;
        CollaborationWindowLifeEventControl lifeEventControl;
        Canvas container;
        public CollaborationWindowMainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            lifeEventControl = (CollaborationWindowLifeEventControl)e.Parameter;
            FilePath.CSVFile=lifeEventControl.TableFileDir;
            FilePath.NewsArticle = lifeEventControl.DocFileDir;
            this.Loaded += CollaborationWindowMainPage_Loaded;
        }

        private void CollaborationWindowMainPage_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
        }

        public async void Init()
        {
            Screen.WIDTH = (int)ApplicationView.GetForCurrentView().VisibleBounds.Width;
            Screen.HEIGHT = (int)ApplicationView.GetForCurrentView().VisibleBounds.Height;
            Screen.SCALE_FACTOR = 1 / DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
            this.Width = Screen.WIDTH;
            this.Height = Screen.HEIGHT;
            container = new Canvas();
            container.Width = Screen.WIDTH;
            container.Height = Screen.HEIGHT;
            this.Content = container;
            controllers = new CentralControllers();
            controllers.Init(Screen.WIDTH, Screen.HEIGHT);
            await Task.Delay(TimeSpan.FromSeconds(3));
            container.Children.Add(controllers.BaseLayerController.GetBaseLayer());
            container.Children.Add(controllers.GlowLayerController.GetGlowLayer());
            container.Children.Add(controllers.CardLayerController.GetCardLayer());
            container.Children.Add(controllers.SortingBoxLayerController.GetSortingBoxLayer());
            container.Children.Add(controllers.MenuLayerController.GetMenuLayer());
        }

        public void Deinit()
        {
            controllers.Deinit();
            container.Children.Remove(controllers.BaseLayerController.GetBaseLayer());
            container.Children.Remove(controllers.CardLayerController.GetCardLayer());
            container.Children.Remove(controllers.SortingBoxLayerController.GetSortingBoxLayer());
            container.Children.Remove(controllers.MenuLayerController.GetMenuLayer());
        }
    }
}
