using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualKeyboard;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace CoLocatedCardSystem.CollaborationWindow.Layers.Menu_Layer
{
    /// <summary>
    /// Base class to activate a text input widget that needs virtual keyboard
    /// </summary>
    class TextInputButton : Button
    {
        string activeText = "";
        string deactiveText = "";
        Image activeImage;
        Image deactiveImage;
        OnScreenKeyBoard virtualKeyboard;
        TextBox textBox;
        public void Init(string actText, string deText, OnScreenKeyBoard keyboard, TextBox tBox)
        {
            this.activeText = actText;
            this.deactiveText = deText;
            this.virtualKeyboard = keyboard;
            this.textBox = tBox;
            this.Content = activeText;
        }
        public void Init(Uri activeImgUri, Uri deImagUri, OnScreenKeyBoard keyboard, TextBox tBox)
        {
            this.activeImage = new Image();
            activeImage.Source = new BitmapImage(activeImgUri);
            activeImage.VerticalAlignment = VerticalAlignment.Center;
            this.deactiveImage = new Image();
            deactiveImage.Source = new BitmapImage(deImagUri);
            deactiveImage.VerticalAlignment = VerticalAlignment.Center;
            this.virtualKeyboard = keyboard;
            this.textBox = tBox;
            this.Content = activeImage;
        }
        private void HideKeyboard()
        {
            virtualKeyboard.Visibility = Visibility.Collapsed;
            textBox.Visibility = Visibility.Collapsed;
            virtualKeyboard.Disable();
            textBox.Text = "";
        }
        private void ShowKeyboard()
        {
            textBox.Visibility = Visibility.Visible;
            virtualKeyboard.Visibility = Visibility.Visible;
            virtualKeyboard.Enable(textBox);
        }
        public virtual void Open()
        {
            ShowKeyboard();
            if (deactiveImage != null)
            {
                this.Content = deactiveImage;
            }
            else
            {
                this.Content = deactiveText;
            }
        }
        public virtual void Close()
        {
            HideKeyboard();
            if (activeImage != null)
            {
                this.Content = activeImage;
            }
            else
            {
                this.Content = activeText;
            }
        }
    }
}
