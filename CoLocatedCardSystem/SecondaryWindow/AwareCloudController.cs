using System;
using CoLocatedCardSystem.CollaborationWindow;
using CoLocatedCardSystem.SecondaryWindow.Layers;
using CoLocatedCardSystem.SecondaryWindow.SemanticModule;
using System.Collections.Concurrent;
using CoLocatedCardSystem.SecondaryWindow.CloudModule;
using CoLocatedCardSystem.CollaborationWindow.InteractionModule;
using Windows.UI;
using CoLocatedCardSystem.CollaborationWindow.DocumentModule;

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

        internal void AddInitialSemanticsGroup()
        {
            var sgroups = controllers.SemanticGroupController.GetSemanticGroup();
            Random colorRand = new Random();
            foreach (SemanticGroup sg in sgroups)
            {
                animationController.SemanticCloud.AddSemanticNode(sg.Id, sg.GetDescription());
                int h = colorRand.Next(200);
                animationController.SemanticCloud.SetSemanticNodeColor(sg.Id, h, 1, 1);
            }
            foreach (SemanticGroup sg1 in sgroups)
            {
                foreach (SemanticGroup sg2 in sgroups)
                {
                    if (sg1 != sg2 && sg1.ShareWord(sg2))
                    {
                        animationController.SemanticCloud.ConnectSemanticNode(sg1.Id, sg2.Id);
                    }
                }
            }
            foreach (SemanticGroup sg in sgroups)
            {
                foreach (Semantic sem in sg.GetSemantics())
                {
                    animationController.AwareCloud.CreateCloudNode(sem.DocID, CloudNode.NODETYPE.DOC, sg.Id);
                }
                foreach (Token tk in sg.GetToken())
                {
                    animationController.AwareCloud.CreateCloudNode(sg.Id + tk.StemmedWord, CloudNode.NODETYPE.WORD, sg.Id);
                    animationController.AwareCloud.SetCloudNodeText(sg.Id + tk.StemmedWord, tk.OriginalWord, tk.StemmedWord);
                    animationController.AwareCloud.SetCloudNodeWeight(sg.Id + tk.StemmedWord, 20);
                }
            }
            animationController.ResetMoveStep();
        }
        internal void Deinit()
        {
            animationController.Deinit();
        }
    }
}
