using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoLocatedCardSystem.CollaborationWindow.Layers.Glow_Layer;
using CoLocatedCardSystem.CollaborationWindow.InteractionModule;

namespace CoLocatedCardSystem.CollaborationWindow.ConnectionModule
{
    class ConnectionController
    {
        private CentralControllers controllers;
        private List<GlowGroup> previousStatus;
        private List<GlowGroup> currentStatus;
        public ConnectionController(CentralControllers centralControllers)
        {
            this.controllers = centralControllers;
        }

        internal void Init()
        {
            previousStatus = new List<GlowGroup>();
            currentStatus = new List<GlowGroup>();
        }
        internal void Deinit() { }

        internal void SavePreviousStatus()
        {
            lock (previousStatus)
            {
                List<GlowGroup> glowgroups = controllers.GlowController.GetGroups();
                previousStatus.Clear();
                foreach (GlowGroup gg in glowgroups)
                {
                    GlowGroup newgg = new GlowGroup();
                    foreach (string id in gg.GetCardID())
                    {
                        newgg.AddCard(id);
                    }
                    previousStatus.Add(newgg);
                }
            }
        }

        internal void UpdateCurrentStatus()
        {
            lock (currentStatus)
            {
                List<GlowGroup> glowgroups = controllers.GlowController.GetGroups();
                currentStatus.Clear();
                foreach (GlowGroup gg in glowgroups)
                {
                    GlowGroup newgg = new GlowGroup();
                    foreach (string id in gg.GetCardID())
                    {
                        newgg.AddCard(id);
                    }
                    currentStatus.Add(newgg);
                }
            }            
        }
    }
}
