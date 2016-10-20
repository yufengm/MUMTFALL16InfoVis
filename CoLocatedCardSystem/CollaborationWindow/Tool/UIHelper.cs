using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace CoLocatedCardSystem.CollaborationWindow
{
    class UIHelper
    {
        /// <summary>
        /// Update the render transform
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="scale"></param>
        /// <param name="element"></param>
        public static void InitializeUI(Point position, double rotation, double scale, Size size, FrameworkElement element)
        {
            element.Width = size.Width;
            element.Height = size.Height;
            ScaleTransform st = new ScaleTransform();
            st.ScaleX = scale;
            st.ScaleY = scale;
            RotateTransform rt = new RotateTransform();
            rt.Angle = rotation;
            TranslateTransform tt = new TranslateTransform();
            tt.X = position.X;
            tt.Y = position.Y;
            TransformGroup transGroup = new TransformGroup();
            transGroup.Children.Add(st);
            transGroup.Children.Add(rt);
            transGroup.Children.Add(tt);
            element.RenderTransform = transGroup;
        }
        /// <summary>
        /// Get the bounding size of the text.
        /// </summary>
        /// <returns></returns>
        public static Size GetBoundingSize(string text, double fontsize) {
            TextBlock tb = new TextBlock { Text = text, FontSize = fontsize};
            tb.Padding = new Thickness(0.5,0.5,0.5,0);
            tb.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            Size boundSize = new Size(tb.DesiredSize.Width*1.1,tb.DesiredSize.Height);
            return boundSize;
        }
    }
}
