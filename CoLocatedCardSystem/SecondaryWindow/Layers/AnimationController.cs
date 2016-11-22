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
        internal static double INITALSTEP = 2;
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
        }
        public void StartThread()
        {
            TimeSpan period = TimeSpan.FromMilliseconds(50);

            periodicTimer = ThreadPoolTimer.CreatePeriodicTimer((source) =>
            {
                if (semanticCloud.MoveStep > awareCloud.MoveStep)
                {
                    awareCloud.MoveStep = semanticCloud.MoveStep;
                }
                semanticCloud.Update();
                awareCloud.Update();
                awareCloudController.UpdateSemanticNode(semanticCloud.GetSemanticNodes());
                awareCloudController.UpdateCloudNode(awareCloud.GetCloudNodes());
            }, period);
        }

        internal void Deinit()
        {
            if (periodicTimer != null)
            {
                periodicTimer.Cancel();
            }
        }
        internal void ResetMoveStep()
        {
            semanticCloud.MoveStep = INITALSTEP;
            awareCloud.MoveStep = INITALSTEP;
        }
    }
}
