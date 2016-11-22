using CoLocatedCardSystem.CollaborationWindow.DocumentModule;
using CoLocatedCardSystem.CollaborationWindow.InteractionModule;
using CoLocatedCardSystem.SecondaryWindow;
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
            app = App.Current as App;
            awareCloudController = app.AwareCloudController;
        }

        internal void Init()
        {
        }
        internal void Deinit() { }

        internal void AddSemanticCluster(SemanticGroup group)
        {
            if (awareCloudController != null)
            {
                // awareCloudController.AddSemanticNode(group.Id, group.GetDescription());
            }
        }
        /// <summary>
        /// Save the status of the current connected cards
        /// </summary>
        internal async void UpdateCurrentStatus()
        {
            foreach (CardGroup gg in controllers.SemanticGroupController.GetGroups().Values)
            {
                var cardIDs = gg.GetCardID();
                List<Document> docs = new List<Document>();
                double px = 0;
                double py = 0;
                List<User> ownerList = new List<User>();
                foreach (string id in cardIDs)
                {
                    Document doc = controllers.CardController.DocumentCardController.GetDocumentCardById(id).Document;
                    docs.Add(doc);
                    CardStatus cs = await controllers.CardController.GetLiveCardStatus(id);
                    px += cs.position.X;
                    py += cs.position.Y;
                    if (!ownerList.Contains(cs.owner))
                    {
                        ownerList.Add(cs.owner);
                    }
                }
                px /= gg.Count();
                py /= gg.Count();
                System.Diagnostics.Debug.WriteLine(gg.Id);
                if (docs.Count > 0)
                {
                    Token[] tks = controllers.MlController.GetTopicToken(docs.ToArray());
                    if (tks != null)
                    {
                        foreach (Token tk in tks)
                        {
                            //AddWordToken(tk, ownerList[0], MyColor.Color1, gg.Id, px, py);
                        }

                    }
                }
            }
        }

        internal void HightLightSearchResult(string[] ids) {
            
        }
    }
}
