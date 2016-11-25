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

        internal Task<Topic> GetSubTopicToken(string[] docList)
        {
            return controllers.MlController.GetTopicToken(docList);
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
            foreach (SemanticGroup sg in sgroups)
            {
                string newID = sg.Id;
                int h = ColorPicker.GetColorHue();
                animationController.SemanticCloud.AddSemanticNode(newID, sg.GetDescription());
                animationController.SemanticCloud.SetSemanticNodeColor(newID, h, 1, 1);
                animationController.SemanticCloud.SetSemanticNodeOptimal(newID, 20);
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
                string newID = sg.Id;
                SemanticNode node = animationController.SemanticCloud.FindNode(sg.Id);
                int h = node.H;
                string preID = sg.Id;

                newID = sg.Id + User.ALEX.ToString() + User.BEN.ToString();
                animationController.SemanticCloud.AddSemanticNode(newID, sg.GetDescription());
                animationController.SemanticCloud.SetSemanticNodeColor(newID, h, 1, 1);
                animationController.SemanticCloud.SetSemanticNodeOptimal(newID, 20);
                animationController.SemanticCloud.ConnectSemanticNode(newID, preID);

                newID = sg.Id + User.ALEX.ToString() + User.CHRIS.ToString();
                animationController.SemanticCloud.AddSemanticNode(newID, sg.GetDescription());
                animationController.SemanticCloud.SetSemanticNodeColor(newID, h, 1, 1);
                animationController.SemanticCloud.SetSemanticNodeOptimal(newID, 20);
                animationController.SemanticCloud.ConnectSemanticNode(newID, preID);

                newID = sg.Id + User.BEN.ToString() + User.CHRIS.ToString();
                animationController.SemanticCloud.AddSemanticNode(newID, sg.GetDescription());
                animationController.SemanticCloud.SetSemanticNodeColor(newID, h, 1, 1);
                animationController.SemanticCloud.SetSemanticNodeOptimal(newID, 20);
                animationController.SemanticCloud.ConnectSemanticNode(newID, preID);

                newID = sg.Id + User.ALEX.ToString();
                animationController.SemanticCloud.AddSemanticNode(newID, sg.GetDescription());
                animationController.SemanticCloud.SetSemanticNodeColor(newID, h, 1, 1);
                animationController.SemanticCloud.SetSemanticNodeOptimal(newID, 20);
                animationController.SemanticCloud.ConnectSemanticNode(newID, sg.Id + User.ALEX.ToString() + User.BEN.ToString());
                animationController.SemanticCloud.ConnectSemanticNode(newID, sg.Id + User.ALEX.ToString() + User.CHRIS.ToString());

                newID = sg.Id + User.BEN.ToString();
                animationController.SemanticCloud.AddSemanticNode(newID, sg.GetDescription());
                animationController.SemanticCloud.SetSemanticNodeColor(newID, h, 1, 1);
                animationController.SemanticCloud.SetSemanticNodeOptimal(newID, 20);
                animationController.SemanticCloud.ConnectSemanticNode(newID, sg.Id + User.ALEX.ToString() + User.BEN.ToString());
                animationController.SemanticCloud.ConnectSemanticNode(newID, sg.Id + User.BEN.ToString() + User.CHRIS.ToString());

                newID = sg.Id + User.CHRIS.ToString();
                animationController.SemanticCloud.AddSemanticNode(newID, sg.GetDescription());
                animationController.SemanticCloud.SetSemanticNodeColor(newID, h, 1, 1);
                animationController.SemanticCloud.SetSemanticNodeOptimal(newID, 20);
                animationController.SemanticCloud.ConnectSemanticNode(newID, sg.Id + User.ALEX.ToString() + User.CHRIS.ToString());
                animationController.SemanticCloud.ConnectSemanticNode(newID, sg.Id + User.BEN.ToString() + User.CHRIS.ToString());
            }
            foreach (SemanticGroup sg in sgroups)
            {
                foreach (string docID in sg.GetDocs())
                {
                    animationController.AwareCloud.CreateCloudNode(docID, CloudNode.NODETYPE.DOC, sg.Id, User.NONE);
                    animationController.AwareCloud.SetCloudNodeDoc(docID, docID);

                }
                foreach (Token tk in sg.GetToken())
                {
                    animationController.AwareCloud.CreateCloudNode(sg.Id + tk.StemmedWord, CloudNode.NODETYPE.WORD, sg.Id, User.NONE);
                    animationController.AwareCloud.SetCloudNodeText(sg.Id + tk.StemmedWord, tk.OriginalWord, tk.StemmedWord);
                    animationController.AwareCloud.SetCloudNodeWeight(sg.Id + tk.StemmedWord, 10);
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
