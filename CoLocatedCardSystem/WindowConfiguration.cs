using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace CoLocatedCardSystem
{
    public partial class MainPage : Page
    {
        public const string FEATURE_NAME = "Collaborative Visualization";

        List<Scenario> scenarios = new List<Scenario>
        {
            new Scenario() { Title="Collaboration Window", ClassType=typeof(CollaborationWindow.CollaborationWindowMainPage)},
            new Scenario() { Title="Secondary Window", ClassType=typeof(SecondaryWindow.CollaborationWindowSecondaryPage)}
        };

        public CollaborationWindow.CollaborationWindowLifeEventControl CollaborationWindowLifeControl;
        public SecondaryWindow.SecondaryWindowLifeEventControl SecondaryWindowLifeControl;
    }

    public class Scenario
    {
        public string Title { get; set; }
        public Type ClassType { get; set; }
    }
}
