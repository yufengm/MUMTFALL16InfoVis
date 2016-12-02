using CoLocatedCardSystem.CollaborationWindow.DocumentModule;
using CoLocatedCardSystem.CollaborationWindow.InteractionModule;
using CoLocatedCardSystem.SecondaryWindow;
using CoLocatedCardSystem.SecondaryWindow.CloudModule;
using System.Collections.Generic;

namespace CoLocatedCardSystem.CollaborationWindow.ConnectionModule
{
    class ConnectionController
    {
        private CentralControllers controllers;
        AwareCloudController awareCloudController;
        App app;

        internal AwareCloudController AwareCloudController
        {
            get
            {
                return awareCloudController;
            }

            set
            {
                awareCloudController = value;
            }
        }

        public ConnectionController(CentralControllers centralControllers)
        {
            this.controllers = centralControllers;
        }

        internal void Init(AwareCloudController awaCtrls)
        {
            awareCloudController = awaCtrls;
        }
        internal void Deinit() { }

        internal void UpdateSemanticCloud() {
            awareCloudController.UpdateSemanticCloud();
        }
    }
}
