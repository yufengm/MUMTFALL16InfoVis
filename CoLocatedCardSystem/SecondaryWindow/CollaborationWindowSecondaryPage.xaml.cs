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
        public CollaborationWindowSecondaryPage()
        {
            this.InitializeComponent();
            Init();
        }

        private void Init()
        {
            
            SecondaryScreen.WIDTH = (int)ApplicationView.GetForCurrentView().VisibleBounds.Width;
            SecondaryScreen.HEIGHT = (int)ApplicationView.GetForCurrentView().VisibleBounds.Height;
            SecondaryScreen.SCALE_FACTOR = 1 / DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
            this.Width = SecondaryScreen.WIDTH;
            this.Height = SecondaryScreen.HEIGHT;
            //Container.Width = this.Width;
            //Container.Height = this.Height;
            ApplicationView.PreferredLaunchViewSize = new Size(this.Width, this.Height);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.FullScreen;
        }
    }
}
