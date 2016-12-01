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
        /// <summary>
        /// Save the status of the current connected cards
        /// </summary>
        internal async void UpdateCurrentStatus()
        {
            foreach (CardGroup gg in controllers.SemanticGroupController.GetGroups().Values)
            {
                if (gg.Count() > 1)
                {
                    var cardIDs = gg.GetCardID();
                    List<string> docIDs = new List<string>();
                    foreach (string id in cardIDs)
                    {
                        Document doc = controllers.CardController.DocumentCardController.GetDocumentCardById(id).Document;
                        if (!docIDs.Contains(doc.DocID))
                        {
                            docIDs.Add(doc.DocID);
                        }

                    }
                    if (docIDs.Count > 1)
                    {
                        await controllers.SemanticGroupController.MergeGroup(docIDs.ToArray());
                        UpdateSemanticCloud();
                    }
                }
            }
        }
        internal void UpdateSemanticCloud() {
            awareCloudController.UpdateSemanticCloud();
        }
    }
}
