using CoLocatedCardSystem.CollaborationWindow.InteractionModule;
using CoLocatedCardSystem.SecondaryWindow;

namespace CoLocatedCardSystem.CollaborationWindow.ConnectionModule
{
    class ConnectionController
    {
        private CentralControllers controllers;
        AwareCloudController awareCloudController;
        App app;
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
            //foreach (SemanticGroup gg in controllers.SemanticGroupController.GetGroups().Values)
            //{
            //    var cardIDs = gg.GetCardID();
            //    List<Document> docs = new List<Document>();
            //    double px = 0;
            //    double py = 0;
            //    List<User> ownerList = new List<User>();
            //    foreach (string id in cardIDs)
            //    {
            //        Document doc = controllers.CardController.DocumentCardController.GetDocumentCardById(id).Document;
            //        docs.Add(doc);
            //        CardStatus cs = await controllers.CardController.GetLiveCardStatus(id);
            //        px += cs.position.X;
            //        py += cs.position.Y;
            //        if (!ownerList.Contains(cs.owner))
            //        {
            //            ownerList.Add(cs.owner);
            //        }
            //    }
            //    px /= gg.Count();
            //    py /= gg.Count();
            //    System.Diagnostics.Debug.WriteLine(gg.Id);
            //    if (docs.Count > 0)
            //    {
            //        Token[] tks = controllers.MlController.GetTopicToken(docs.ToArray());
            //        foreach (Token tk in tks)
            //        {
            //            //AddWordToken(tk, ownerList[0], MyColor.Color1, gg.Id, px, py);
            //        }
            //    }
            //}
        }

        //internal void AddWordToken(Token tk, User owner, Color color, String group, double x, double y)
        //{
        //    Node cw = new Node();
        //    cw.Owner = owner.ToString();
        //    cw.Color = color.R + "," + color.G + "," + color.B;
        //    cw.Text = tk.OriginalWord;
        //    cw.StemmedText = tk.StemmedWord;
        //    cw.X = x;
        //    cw.Y = y;
        //    cw.Type = 0;
        //    cw.Weight = 20;
        //    cw.Group = group;
        //    cw.Highlight = true;
        //    app.AddWordToScreen(cw);
        //}

        //internal void AddImageToken(String imgUrl, User owner, Color color, String group, double x, double y)
        //{
        //    Node cw = new Node();
        //    cw.Owner = owner.ToString();
        //    cw.Color = color.R + "," + color.G + "," + color.B;
        //    cw.Text = imgUrl;
        //    cw.StemmedText = imgUrl;
        //    cw.X = x;
        //    cw.Y = y;
        //    cw.Type = 1;
        //    cw.Weight = 20;
        //    cw.Group = group;
        //    cw.Highlight = true;
        //    app.AddWordToScreen(cw);
        //}
        //internal void RemoveToken(Token tk, String cardID)
        //{
        //    app.RemoveWord(tk.StemmedWord, cardID);
        //}
    }
}
