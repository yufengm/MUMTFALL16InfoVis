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
        ConcurrentBag<ClusterDoc[]> list=new ConcurrentBag<ClusterDoc[]>();
        internal void init(WebView v)
        {
            this.webView = v;
            if (periodicTimer == null)
            {
                periodicTimer = ThreadPoolTimer.CreatePeriodicTimer((source) =>
                {
                    FetchNewArray();
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
        internal async void UpdateView() {
            foreach (ClusterDoc[] docs in list)
            {
                foreach (ClusterDoc doc in docs)
                {
                    await webView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, async () =>
                    {
                        await webView.InvokeScriptAsync("addNode", new string[] {doc.GetName()});
                    });
                }
            }

        }
        private void FetchNewArray()
        {
            App app = App.Current as App;
            list = app.GetCurrent();
        }
    }
}
