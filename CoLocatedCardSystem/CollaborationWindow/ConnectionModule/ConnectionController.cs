using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoLocatedCardSystem.CollaborationWindow.Layers.Glow_Layer;
using CoLocatedCardSystem.CollaborationWindow.InteractionModule;
using CoLocatedCardSystem.CollaborationWindow.DocumentModule;
using CoLocatedCardSystem.ClusterModule;
using CoLocatedCardSystem.SecondaryWindow;
using System.Collections.Concurrent;

namespace CoLocatedCardSystem.CollaborationWindow.ConnectionModule
{
    class ConnectionController
    {
        private CentralControllers controllers;

        App app;
        public ConnectionController(CentralControllers centralControllers)
        {
            this.controllers = centralControllers;
            app = App.Current as App;
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
            foreach (KeyValuePair<string, GlowGroup> gg in controllers.GlowController.GetGroups())
            {
                var cardIDs = gg.Value.GetCardID();
                foreach (string id in cardIDs.Keys)
                {
                    CardStatus cs = await controllers.CardController.GetLiveCardStatus(id);
                    Document doc = controllers.CardController.DocumentCardController.GetDocumentCardById(id).Document;
                    Token[] tks = controllers.CardController.DocumentCardController.GetHighLightedContent(id);
                    if (doc != null && cs != null && tks != null)
                    {
                        double px = cs.position.X * SecondaryScreen.WIDTH * SecondaryScreen.SCALE_FACTOR / (Screen.WIDTH * Screen.SCALE_FACTOR);
                        double py = cs.position.Y * SecondaryScreen.HEIGHT * SecondaryScreen.SCALE_FACTOR / (Screen.HEIGHT * Screen.SCALE_FACTOR);
                        foreach (Token tk in tks)
                        {
                            AddWordToken(tk, doc.DocID, px, py);
                        }
                        string[] jpgs = doc.RawDocument.Jpg[0].Split(',');
                        AddImageToken(jpgs[0], doc.DocID, px, py);
                    }
                }
            }
        }

        internal void AddWordToken(Token tk, String group, double x, double y)
        {
            ClusterWord cw = new ClusterWord();
            cw.Text = tk.OriginalWord;
            cw.StemmedText = tk.StemmedWord;
            cw.X = x;
            cw.Y = y;
            cw.Type = 0;
            cw.Weight = 20;
            cw.Group = group;
            cw.Highlight = true;
            app.AddWordToScreen(cw);
        }

        internal void AddImageToken(String imgUrl, String group, double x, double y)
        {
            ClusterWord cw = new ClusterWord();
            cw.Text = imgUrl;
            cw.StemmedText = imgUrl;
            cw.X = x;
            cw.Y = y;
            cw.Type = 1;
            cw.Weight = 20;
            cw.Group = group;
            cw.Highlight = true;
            app.AddWordToScreen(cw);
        }
        internal void RemoveToken(Token tk, String group)
        {
            app.RemoveWord(tk.StemmedWord, group);
        }
    }
}
