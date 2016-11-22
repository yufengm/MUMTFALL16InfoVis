using CoLocatedCardSystem.ClusterModule;
using CoLocatedCardSystem.CollaborationWindow;
using CoLocatedCardSystem.CollaborationWindow.ConnectionModule;
using CoLocatedCardSystem.CollaborationWindow.DocumentModule;
using CoLocatedCardSystem.CollaborationWindow.InteractionModule;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System.Threading;
using Windows.UI.Xaml.Controls;

namespace CoLocatedCardSystem.SecondaryWindow.AwareCloudModule
{
    class AwareCloudController
    {
        WebView webView;
        CentralControllers controllers;
        internal void init(WebView v, CentralControllers ctrls)
        {
            this.webView = v;
            this.controllers = ctrls;
        }
        internal void AddInitialSemanticsGroup()
        {
            var sgroups = controllers.SemanticGroupController.GetSemanticGroup();
            Random colorRand = new Random();
            foreach (SemanticGroup sg in sgroups)
            {
                AddSemanticNode(sg.Id, sg.GetDescription());
                SetSemanticNodeColor(sg.Id, new int[] { colorRand.Next(205)+50, colorRand.Next(205) + 50, colorRand.Next(100) });
            }
            foreach (SemanticGroup sg1 in sgroups)
            {
                foreach (SemanticGroup sg2 in sgroups)
                {
                    if (sg1 != sg2 && sg1.ShareWord(sg2))
                    {
                        ConnectSemanticGroup(sg1.Id, sg2.Id);
                    }
                }
            }
            foreach (SemanticGroup sg in sgroups)
            {
                foreach (Semantic sem in sg.GetSemantics())
                {
                    CreateWordNode(sem.DocID, "point", sg.Id);
                }
                foreach (Token tk in sg.GetToken())
                {
                    CreateWordNode(sg.Id + tk.StemmedWord, "text", sg.Id);
                    SetWordNodeText(sg.Id + tk.StemmedWord, tk.OriginalWord, tk.StemmedWord);
                    SetWordNodeWeight(sg.Id + tk.StemmedWord,"" + 20);
                }
            }
            ResetMoveStep();
        }

        private async void SetWordNodeWeight(string id, string weight)
        {
            await webView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, async () =>
            {
                await webView.InvokeScriptAsync("setWordNodeWeight",
                          new string[] { id, weight });
            });
        }

        private async void ResetMoveStep()
        {
            await webView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, async () =>
            {
                await webView.InvokeScriptAsync("resetMoveStep",
                          new string[] { });
            });
        }

        private async void SetWordNodeText(string id, string originalWord, string stemmedWord)
        {
            await webView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, async () =>
            {
                await webView.InvokeScriptAsync("setWordNodeText",
                          new string[] { id, originalWord, stemmedWord });
            });
        }

        private async void SetSemanticNodeColor(string id, int[] color)
        {
            await webView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, async () =>
            {
                await webView.InvokeScriptAsync("setSemanticNodeColor",
                          new string[] { id, "" + color[0], "" + color[1], "" + color[2] });
            });
        }

        private async void ConnectSemanticGroup(string id1, string id2)
        {
            await webView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, async () =>
            {
                await webView.InvokeScriptAsync("connectSemanticNode",
                          new string[] { id1, id2 });
            });
        }

        private async void CreateWordNode(string docID, string type, string id)
        {
            await webView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, async () =>
            {
                await webView.InvokeScriptAsync("createWordNode",
                          new string[] { docID, type, id });
            });
        }


        internal async void AddSemanticNode(string id, string semantic)
        {
            await webView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, async () =>
            {
                await webView.InvokeScriptAsync("addSemanticNode",
                          new string[] { id, semantic });
            });
        }
        internal void Deinit()
        {
        }
    }
}
