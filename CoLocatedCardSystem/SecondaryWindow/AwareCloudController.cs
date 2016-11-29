using System;
using CoLocatedCardSystem.CollaborationWindow;
using CoLocatedCardSystem.SecondaryWindow.Layers;
using CoLocatedCardSystem.SecondaryWindow.SemanticModule;
using System.Collections.Concurrent;
using CoLocatedCardSystem.SecondaryWindow.CloudModule;
using CoLocatedCardSystem.CollaborationWindow.InteractionModule;
using Windows.UI;
using CoLocatedCardSystem.CollaborationWindow.DocumentModule;
using CoLocatedCardSystem.CollaborationWindow.Tool;
using System.Threading.Tasks;
using CoLocatedCardSystem.SecondaryWindow.Tool;
using System.Collections.Generic;

namespace CoLocatedCardSystem.SecondaryWindow
{
    class AwareCloudController
    {
        //WebView webView;
        CentralControllers controllers;
        BaseLayerController baseLayerController;
        SemanticLayerController semanticLayerController;
        CloudLayerController cloudLayerController;
        AnimationController animationController;

        internal CloudLayerController CloudLayerController
        {
            get
            {
                return cloudLayerController;
            }

            set
            {
                cloudLayerController = value;
            }
        }

        internal BaseLayerController BaseLayerController
        {
            get
            {
                return baseLayerController;
            }

            set
            {
                baseLayerController = value;
            }
        }

        internal SemanticLayerController SemanticLayerController
        {
            get
            {
                return semanticLayerController;
            }

            set
            {
                semanticLayerController = value;
            }
        }

        internal AnimationController AnimationController
        {
            get
            {
                return animationController;
            }

            set
            {
                animationController = value;
            }
        }

        public CentralControllers Controllers
        {
            get
            {
                return controllers;
            }

            set
            {
                controllers = value;
            }
        }

        internal void Init(int width, int height)
        {
            // this.webView = v;
            App app = App.Current as App;
            this.controllers = app.CentralController;
            this.baseLayerController = new BaseLayerController(this);
            this.semanticLayerController = new SemanticLayerController(this);
            this.cloudLayerController = new CloudLayerController(this);
            this.animationController = new AnimationController(this);
            this.baseLayerController.Init(width, height);
            this.semanticLayerController.Init(width, height);
            this.cloudLayerController.Init(width, height);
            this.animationController.Init();
            this.animationController.StartThread();
        }

        internal void UpdateCloudNode(ConcurrentDictionary<string, CloudNode> cloudNodes)
        {
            this.cloudLayerController.UpdateCloudNode(cloudNodes);
            this.cloudLayerController.Invalidate();
        }

        internal void UpdateSemanticNode(ConcurrentDictionary<string, SemanticNode> semanticNodes)
        {
            this.semanticLayerController.UpdateSemanticNode(semanticNodes);
            this.semanticLayerController.Invalidate();
        }

        internal void UpdateSemanticCloud()
        {
            var sgroups = controllers.SemanticGroupController.GetSemanticGroup();
            animationController.SemanticCloud.UpdateSemanticNode(sgroups);
            animationController.AwareCloud.UpdateCloudNode(sgroups);
            animationController.ResetMoveStep();
        }
        internal void Deinit()
        {
            animationController.Deinit();
        }
    }
}
