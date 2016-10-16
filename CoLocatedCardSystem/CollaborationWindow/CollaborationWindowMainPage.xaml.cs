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
            Container.Width = this.Width;
            Container.Height = this.Height;
            controllers = new CentralControllers();
            controllers.Init(Screen.WIDTH, Screen.HEIGHT);
            Container.Children.Add(controllers.BaseLayerController.GetBaseLayer());
            Container.Children.Add(controllers.GlowLayerController.GetGlowLayer());
            Container.Children.Add(controllers.CardLayerController.GetCardLayer());
            Container.Children.Add(controllers.PlotLayerController.GetPlotLayer());
            Container.Children.Add(controllers.SortingBoxLayerController.GetSortingBoxLayer());
            Container.Children.Add(controllers.MenuLayerController.GetMenuLayer());
        }
        public void Deinit()
        {
            controllers.Deinit();
            Container.Children.Remove(controllers.BaseLayerController.GetBaseLayer());
            Container.Children.Remove(controllers.CardLayerController.GetCardLayer());
            Container.Children.Remove(controllers.SortingBoxLayerController.GetSortingBoxLayer());
            Container.Children.Remove(controllers.MenuLayerController.GetMenuLayer());
        }
    }
}
