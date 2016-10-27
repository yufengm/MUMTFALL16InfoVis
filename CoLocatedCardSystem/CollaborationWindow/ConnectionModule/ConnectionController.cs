using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoLocatedCardSystem.CollaborationWindow.Layers.Glow_Layer;
using CoLocatedCardSystem.CollaborationWindow.InteractionModule;
using CoLocatedCardSystem.CollaborationWindow.DocumentModule;

namespace CoLocatedCardSystem.CollaborationWindow.ConnectionModule
{
    class ConnectionController
    {
        private CentralControllers controllers;
        public ConnectionController(CentralControllers centralControllers)
        {
            this.controllers = centralControllers;
        }

        internal void Init()
        {
        }
        internal void Deinit() { }
        /// <summary>
        /// Save the status of the current connected cards
        /// </summary>
        internal async void UpdateCurrentStatus()
        {
            List<GlowGroup> glowgroups = controllers.GlowController.GetGroups();
            List<Document[]> docList = new List<Document[]>();
            List<CardStatus[]> stateList = new List<CardStatus[]>();
            foreach (GlowGroup gg in glowgroups)
            {
                var cardIDs = gg.GetCardID();
                List<Document> docs = new List<Document>();
                List<CardStatus> status = new List<CardStatus>();
                foreach (string id in cardIDs)
                {
                    CardStatus cs = await controllers.CardController.GetLiveCardStatus(id);
                    Document d = controllers.CardController.DocumentCardController.GetDocumentCardById(id).Document;
                    if (d != null && cs != null)
                    {
                        docs.Add(d);
                        status.Add(cs);
                    }
                }
                if (docs.Count > 0 && docs.Count == status.Count) {
                    docList.Add(docs.ToArray());
                    stateList.Add(status.ToArray());
                }
            }
            App app = App.Current as App;
            app.SetCardClusters(docList.ToArray(), stateList.ToArray());
        }
    }
}
