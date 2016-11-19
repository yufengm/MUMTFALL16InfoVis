using CoLocatedCardSystem.ClusterModule;
using CoLocatedCardSystem.CollaborationWindow;
using CoLocatedCardSystem.CollaborationWindow.ConnectionModule;
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
        ThreadPoolTimer periodicTimer;
        TimeSpan period = TimeSpan.FromMilliseconds(1000);
        WebView webView;
        ClusterDocs docs;
        internal void init(WebView v)
        {
            this.webView = v;
            if (periodicTimer == null)
            {
                periodicTimer = ThreadPoolTimer.CreatePeriodicTimer((source) =>
                {
                    UpdateView();
                }, period);
            }
        }

        internal void Deinit()
        {
            if (periodicTimer != null)
            {
                periodicTimer.Cancel();
                periodicTimer = null;
            }
        }
        /// <summary>
        /// Update the word cloud, iterate all docs in the cluster module, if the text exists, update its position and size
        /// </summary>
        internal async void UpdateView()
        {
            App app = App.Current as App;
            this.docs = app.Docs;
            foreach (ClusterWord word in docs.GetAllWords())
            {
                await webView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, async () =>
                {
                    await webView.InvokeScriptAsync("updateNode",
                        new string[] {
                            "" + word.Type,
                            word.Owner,
                            word.Color,
                            word.Text,
                            word.StemmedText,
                            word.Group,
                            "" + word.Weight,
                            "" + word.X,
                            "" + word.Y,
                            "" + word.Highlight, });
                });
            }
        }
        /// <summary>
        /// Remove a word from the cloud
        /// </summary>
        internal async void removeWord(string text, string group)
        {
            App app = App.Current as App;
            await webView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, async () =>
            {
                await webView.InvokeScriptAsync("removeNode",
                    new string[] { text, group });
            });
        }
    }
}
