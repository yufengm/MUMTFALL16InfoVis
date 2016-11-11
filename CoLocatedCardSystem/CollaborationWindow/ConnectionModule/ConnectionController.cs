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
            List<GlowGroup> glowgroups = controllers.GlowController.GetGroups();
            foreach (GlowGroup gg in glowgroups)
            {
                var cardIDs = gg.GetCardID();
                foreach (string id in cardIDs)
                {
                    CardStatus cs = await controllers.CardController.GetLiveCardStatus(id);
                    Document d = controllers.CardController.DocumentCardController.GetDocumentCardById(id).Document;
                    Token[] tks = controllers.CardController.DocumentCardController.GetHighLightedContent(id);
                    if (d != null && cs != null && tks != null)
                    {
                        foreach (Token tk in tks)
                        {
                            AddToken(tk, d.DocID, cs.position.X * SecondaryScreen.WIDTH * SecondaryScreen.SCALE_FACTOR / (Screen.WIDTH * Screen.SCALE_FACTOR),
                                cs.position.Y * SecondaryScreen.HEIGHT * SecondaryScreen.SCALE_FACTOR / (Screen.HEIGHT * Screen.SCALE_FACTOR));
                        }
                    }
                }
            }
        }

        internal void AddToken(Token tk, String group, double x, double y)
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

        internal void RemoveToken(Token tk, String group)
        {
            app.RemoveWord(tk.StemmedWord, group);
        }
    }
}
