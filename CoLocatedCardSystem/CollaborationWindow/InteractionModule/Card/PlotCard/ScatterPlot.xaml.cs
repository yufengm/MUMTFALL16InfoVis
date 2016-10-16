using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    public sealed partial class ScatterPlot : UserControl
    {

        IEnumerable<Point> data;
        public ScatterPlot()
        {
            this.InitializeComponent();
        }
        internal void Init(double width, double height)
        {
            this.Width = width;
            this.Height = height;
            ScatterChart.Width = width;
            ScatterChart.Height = height;
            (ScatterChart.Series[0] as ScatterSeries).ItemsSource = data;
        }
        internal void SetData(IEnumerable<Point> data) {
            this.data = data;
        }
    }
}
