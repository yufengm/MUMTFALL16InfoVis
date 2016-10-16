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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace VirtualKeyboard
{
    public partial class OnScreenKeyBoard : UserControl
    {
        #region Properties
        public object Host { get; set; }

        public KeyboardLayouts InitialLayout
        {
            get { return (this.DataContext as KeyboardViewModel).Layout; }
            set { (this.DataContext as KeyboardViewModel).Layout = value; }
        }
        public static readonly DependencyProperty InitialLayoutProperty = DependencyProperty.Register("InitialLayout", typeof(KeyboardLayouts), typeof(OnScreenKeyBoard), new PropertyMetadata(KeyboardLayouts.English));
        #endregion

        public OnScreenKeyBoard()
        {
            DataContext = new KeyboardViewModel(this);
            InitializeComponent();
        }

        public void RegisterTarget(TextBox control)
        {
            RegisterBox(control);
        }
        public void RegisterTarget(PasswordBox control)
        {
            RegisterBox(control);
        }
        public void Enable(Control control)
        {
            (DataContext as KeyboardViewModel).TargetBox = control;
        }
        public void Disable()
        {
            (DataContext as KeyboardViewModel).TargetBox = null;
        }
        private void RegisterBox(Control control)
        {
            control.GotFocus += delegate { (DataContext as KeyboardViewModel).TargetBox = control; };
            control.LostFocus += delegate { (DataContext as KeyboardViewModel).TargetBox = null; };
        }
    }
}
