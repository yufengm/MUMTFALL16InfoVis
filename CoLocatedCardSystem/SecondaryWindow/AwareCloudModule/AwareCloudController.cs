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
        TimeSpan period = TimeSpan.FromMilliseconds(3000);
        WebView webView;
        internal void init(WebView v) {
            this.webView = v;
            if (periodicTimer == null)
            {
                periodicTimer = ThreadPoolTimer.CreatePeriodicTimer((source) =>
                {
                    FetchNewArray();
                }, period);
            }
        }

        internal void Deinit() {
            if (periodicTimer != null)
            {
                periodicTimer.Cancel();
                periodicTimer = null;
            }
        }

        private async void FetchNewArray()
        {
            App app = App.Current as App;
            ConcurrentBag<ClusterDoc[]> bag = app.GetCurrent();
            foreach (ClusterDoc[] docs in bag)
            {
                foreach (ClusterDoc doc in docs)
                {
                    await webView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, async () =>
                     {
                         await webView.InvokeScriptAsync("addNode", new string[] {  Guid.NewGuid().ToString(),doc.RawDocument.Name.Split(' ')[0] });
                     });
                }
            }
        }
    }
}
