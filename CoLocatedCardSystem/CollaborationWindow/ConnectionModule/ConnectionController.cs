using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoLocatedCardSystem.CollaborationWindow.Layers.Glow_Layer;

namespace CoLocatedCardSystem.CollaborationWindow.ConnectionModule
{
    class ConnectionController
    {
        private CentralControllers centralControllers;

        public ConnectionController(CentralControllers centralControllers)
        {
            this.centralControllers = centralControllers;
        }

        internal void Init() { }
        internal void Deinit() { }
        /// <summary>
        /// Update the visualization on the secondary display
        /// </summary>
        /// <param name="newgg1">The old glow groups. If null, adding newgg2</param>
        /// <param name="newgg2">The new glow groups, If null, remove newgg1</param>
        internal void UpdateViz(GlowGroup[] newgg1, GlowGroup[] newgg2)
        {
            if (newgg1 == null) {
                AddCloud(newgg2);
            }
            if (newgg2 == null)
            {
                RemoveCloud(newgg1);
            }
            else {
                UpdateCloud(newgg1,newgg2);
            }
        }
        private void AddCloud(GlowGroup[] newgg2)
        {
            System.Diagnostics.Debug.WriteLine("add cloud");
        }

        private void UpdateCloud(GlowGroup[] newgg1, GlowGroup[] newgg2)
        {
            System.Diagnostics.Debug.WriteLine("update cloud");
        }

        private void RemoveCloud(GlowGroup[] newgg1)
        {
            System.Diagnostics.Debug.WriteLine("remove cloud");
        }


    }
}
