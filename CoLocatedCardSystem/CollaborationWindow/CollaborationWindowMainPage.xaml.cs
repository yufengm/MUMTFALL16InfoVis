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
            Init();
        }
        public void Init()
        {
            Screen.WIDTH = (int)ApplicationView.GetForCurrentView().VisibleBounds.Width;
            Screen.HEIGHT = (int)ApplicationView.GetForCurrentView().VisibleBounds.Height;
            Screen.SCALE_FACTOR= 1/DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
            this.Width = Screen.WIDTH;
            this.Height = Screen.HEIGHT;
            container = new Canvas();
            container.Width = Screen.WIDTH;
            container.Height = Screen.HEIGHT;
            controllers = new CentralControllers();
            controllers.Init(Screen.WIDTH, Screen.HEIGHT);
            container.Children.Add(controllers.BaseLayerController.GetBaseLayer());
            container.Children.Add(controllers.GlowLayerController.GetGlowLayer());
            container.Children.Add(controllers.CardLayerController.GetCardLayer());
            container.Children.Add(controllers.SortingBoxLayerController.GetSortingBoxLayer());
            container.Children.Add(controllers.MenuLayerController.GetMenuLayer());
            this.Content = container;
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
