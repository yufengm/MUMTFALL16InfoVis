using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualKeyboard;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace CoLocatedCardSystem.CollaborationWindow.Layers.Menu_Layer
{
    /// <summary>
    /// Base class to activate a text input widget that needs virtual keyboard
    /// </summary>
    class TextInputButton : Button
    {
        string activeText;
        string deactiveText;
        OnScreenKeyBoard virtualKeyboard;
        TextBox textBox;
        public void Init(string actText, string deText, OnScreenKeyBoard keyboard, TextBox tBox) {
            this.activeText = actText;
            this.deactiveText = deText;
            this.virtualKeyboard = keyboard;
            this.textBox = tBox;
            this.Content = activeText;
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
        public virtual void Open() {
            ShowKeyboard();
            this.Content = deactiveText;
        }
        public virtual void Close()
        {
            HideKeyboard();
            this.Content = activeText;
        }
    }
}
