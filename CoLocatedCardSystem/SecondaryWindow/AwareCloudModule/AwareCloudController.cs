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
        internal async void UpdateView()
        {
            App app = App.Current as App;
            this.docs = app.Docs;
            foreach (ClusterWord word in docs.GetAllWords())
            {
                await webView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, async () =>
                {
                    await webView.InvokeScriptAsync("addNode",
                        new string[] {
                            "" + word.Type,
                            word.Text,
                            "" + word.Weight,
                            "" + word.X,
                            "" + word.Y,
                            "" + word.Highlight,
                            word.Group });
                });
            }
        }
    }
}
