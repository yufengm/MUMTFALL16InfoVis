using CoLocatedCardSystem.ClusterModule;
using CoLocatedCardSystem.SecondaryWindow.CloudModule;
using CoLocatedCardSystem.SecondaryWindow.SemanticModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System.Threading;

namespace CoLocatedCardSystem.SecondaryWindow.Layers
{
    class AnimationController
    {
        AwareCloudController awareCloudController;
        SemanticCloud semanticCloud;
        AwareCloud awareCloud;
        Random rand = new Random();
        ThreadPoolTimer periodicTimer;
        internal SemanticCloud SemanticCloud
        {
            get
            {
                return semanticCloud;
            }

            set
            {
                semanticCloud = value;
            }
        }

        internal AwareCloud AwareCloud
        {
            get
            {
                return awareCloud;
            }

            set
            {
                awareCloud = value;
            }
        }

        public AnimationController(AwareCloudController awareCloudController)
        {
            this.awareCloudController = awareCloudController;
        }

        public void Init()
        {
            semanticCloud = new SemanticCloud();
            awareCloud = new AwareCloud();
            awareCloud.Init(semanticCloud);
            //for (int i = 0; i < 5; i++)
            //{
            //    semanticCloud.AddSemanticNode("node" + i, "test");
            //}
            //semanticCloud.ConnectSemanticNode("node0", "node1");
            //semanticCloud.ConnectSemanticNode("node0", "node2");
            //semanticCloud.ConnectSemanticNode("node0", "node3");
            //semanticCloud.ConnectSemanticNode("node3", "node4");
            //for (int i = 0; i < 100; i++)
            //{
            //    string newid = "point" + i;
            //    string sid = "node" + rand.Next(5);
            //    awareCloud.CreateCloudNode(newid, NODETYPE.DOC, sid);
            //}
            //for (int i = 0; i < 200; i++)
            //{
            //    string newid = "text" + i;
            //    string sid = "node" + rand.Next(semanticCloud.GetSemanticNodes().Count());
            //    awareCloud.CreateCloudNode(newid, NODETYPE.WORD, sid);
            //    awareCloud.SetCloudNodeText(newid, newid, newid);
            //}
        }
        public void StartThread()
        {
            TimeSpan period = TimeSpan.FromMilliseconds(33);

            periodicTimer = ThreadPoolTimer.CreatePeriodicTimer((source) =>
            {
                semanticCloud.Update();
                awareCloud.Update();
                awareCloudController.UpdateSemanticNode(semanticCloud.GetSemanticNodes());
                awareCloudController.UpdateCloudNode(awareCloud.GetCloudNodes());
            }, period);
        }

        internal void Deinit() {
            if (periodicTimer != null) {
                periodicTimer.Cancel();
            }
        }
        internal void ResetMoveStep()
        {
            semanticCloud.MoveStep = 10;
            awareCloud.MoveStep = 10;
        }
    }
}
