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
                if (gg.Count() > 1) {
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

                    //Token[] tks = controllers.MlController.GetTopicToken(docs.ToArray());
                }
            }
        }
        internal void UpdateSemanticCloud() {
            awareCloudController.UpdateSemanticCloud();
        }
    }
}
