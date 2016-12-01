using CoLocatedCardSystem.CollaborationWindow.InteractionModule;
using CoLocatedCardSystem.SecondaryWindow.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace CoLocatedCardSystem.CollaborationWindow.Layers.Menu_Layer
{
    class SemanticGroupBoard : ComboBox
    {
        Dictionary<ComboBoxItem, string> semanticID = new Dictionary<ComboBoxItem, string>();
        MenuLayerController menuLayerController;
        User owner;
        ComboBoxItem seletedItem = null;

        public SemanticGroupBoard(MenuLayerController ctrls, User owner) {
            this.menuLayerController = ctrls;
            this.owner = owner;
        }
        internal void Init()
        {
            this.SelectionChanged += SemanticGroupBoard_SelectionChanged;
            this.Width = 300;
            this.MaxDropDownHeight = 300;
        }

        private void SemanticGroupBoard_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            seletedItem = this.SelectedItem as ComboBoxItem;
            if (seletedItem != null)
            {
                SemanticGroup sg = menuLayerController.Controllers.SemanticGroupController.GetSemanticGroupById(semanticID[seletedItem]);
                if (sg != null)
                {
                    menuLayerController.SearchDocumentCard(owner, sg);
                }
            }
        }
        internal void AddSemanticGroup(IEnumerable<SemanticGroup> sgroups) {
            ClearGroups();
            foreach (SemanticGroup group in sgroups)
            {
                AddSemanticGroup(group.Id, group.GetDescription(), ColorPicker.HsvToRgb(group.Hue, 1, 0.5));
            }
        }
        internal void AddSemanticGroup(string id, string text, Color color)
        {
            Canvas canvas = new Canvas();
            canvas.Width = 300 * Screen.SCALE_FACTOR;
            canvas.Height = 60 * Screen.SCALE_FACTOR;
            canvas.Background = new SolidColorBrush(color);


            TextBlock tb = new TextBlock();
            tb.Width = canvas.Width;
            tb.Height = canvas.Height;
            tb.Text = text;
            tb.FontSize = 10;
            tb.TextWrapping = Windows.UI.Xaml.TextWrapping.WrapWholeWords;
            tb.Foreground = new SolidColorBrush(MyColor.Wheat);
            canvas.Children.Add(tb);

            ComboBoxItem item = new ComboBoxItem();
            item.Padding = new Windows.UI.Xaml.Thickness(0, 2, 0, 2);
            item.Margin = new Windows.UI.Xaml.Thickness(0, 0, 0, 0);
            item.Content = canvas;
            this.Items.Add(item);
            semanticID.Add(item, id);
        }

        internal void ClearGroups()
        {
            foreach (ComboBoxItem item in semanticID.Keys)
            {
                this.Items.Remove(item);
            }
            semanticID.Clear();
        }
    }
}
