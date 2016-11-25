using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace CoLocatedCardSystem.CollaborationWindow.Layers.Menu_Layer
{
    class DeleteButton : Canvas
    {
        Button deleteButton;
        TextBlock notificationBlock;

        public DeleteButton()
        {
            
        }
        public void Init(MenuBarInfo info) {
            //Initialize the Deletebutton
            deleteButton = new Button();
            deleteButton.Content = "Delete";
            deleteButton.IsTextScaleFactorEnabled = false;

            //Initialize the notificationBlock + Grid
            notificationBlock = new TextBlock();
            notificationBlock.Text = "The Sorting Box will be deleted when it's dragged into this.";
            notificationBlock.TextWrapping = TextWrapping.Wrap;
            Point position = new Point(250, -60);
            Calculator.InitializeUI(position, 0, 1, info.DeleteButtonInfo.Size, notificationBlock);
            notificationBlock.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Sets the delete notification to appear if the user hovers over the delete button
        /// </summary>
        public void setGridVisibility()
        {
            if (notificationBlock.Visibility.Equals(Visibility.Visible))
            {
                notificationBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                notificationBlock.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Callback method when the delete an item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
