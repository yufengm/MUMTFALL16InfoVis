using CoLocatedCardSystem.SecondaryWindow.CloudModule;
using CoLocatedCardSystem.SecondaryWindow.SemanticModule;
using System;
using Windows.System.Threading;

namespace CoLocatedCardSystem.SecondaryWindow
{
    class AnimationController
    {
        AwareCloudController awareCloudController;
        SemanticCloud semanticCloud;
        AwareCloud awareCloud;
        Random rand = new Random();
        ThreadPoolTimer periodicTimer;
        internal static double INITALSTEP = 10;
        TimeSpan period = TimeSpan.FromMilliseconds(10);
        double timerCount = 0;
        double timerExeBound = 50;
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

        internal AwareCloudController AwareCloudController
        {
            get
            {
                return awareCloudController;
            }

            set
            {
                awareCloudController = value;
            }
        }

        public AnimationController(AwareCloudController awareCloudController)
        {
            this.awareCloudController = awareCloudController;
        }

        public void Init()
        {
            semanticCloud = new SemanticCloud(this);
            awareCloud = new AwareCloud(this);
        }
        public void StartThread()
        {
            periodicTimer = ThreadPoolTimer.CreatePeriodicTimer((source) =>
            {
                try
                {
                    if (timerCount >= timerExeBound)
                    {
                        if (semanticCloud.MoveStep > awareCloud.MoveStep)
                        {
                            awareCloud.MoveStep = semanticCloud.MoveStep;
                        }
                        semanticCloud.Update();
                        awareCloud.Update();
                        if (awareCloud.MoveStep < 5)
                        {
                            awareCloudController.UpdateSemanticNode(semanticCloud.GetSemanticNodes());
                            awareCloudController.UpdateCloudNode(awareCloud.GetCloudNodes());
                        }
                        timerExeBound = -4.5 * awareCloud.MoveStep + 50;
                        timerExeBound = timerExeBound < 10 ? 10 : timerExeBound;
                        timerExeBound = timerExeBound > 50 ? 50 : timerExeBound;
                        timerCount = 0;
                    }
                    else
                    {
                        timerCount += 10;
                    }

                }
                catch (Exception ex) {

                }
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
