using CoLocatedCardSystem.CollaborationWindow.DocumentModule;
using CoLocatedCardSystem.CollaborationWindow.GestureModule;
using CoLocatedCardSystem.CollaborationWindow.TouchModule;
using CoLocatedCardSystem.CollaborationWindow.TableModule;
using CoLocatedCardSystem.CollaborationWindow.Layers.Glow_Layer;
using CoLocatedCardSystem.CollaborationWindow.Layers.Card_Layer;
using CoLocatedCardSystem.CollaborationWindow.Layers.SortingBox_Layer;
using CoLocatedCardSystem.CollaborationWindow.Layers.Menu_Layer;
using CoLocatedCardSystem.CollaborationWindow.Layers.Linking_Layer;
using CoLocatedCardSystem.CollaborationWindow.Layers.Base_Layer;
using CoLocatedCardSystem.CollaborationWindow.InteractionModule;
using CoLocatedCardSystem.CollaborationWindow.ConnectionModule;
using CoLocatedCardSystem.CollaborationWindow.MachineLearningModule;

namespace CoLocatedCardSystem.CollaborationWindow
{
    /// <summary>
    /// A central controller for all other controllers
    /// </summary>
    public class CentralControllers
    {
        DocumentController documentController;
        MLController mlController;
        TableController tableController;
        CardController cardController;
        SortingBoxController sortingBoxController;
        TouchController touchController;
        GestureController gestureController;
        SemanticGroupController semanticGroupController;
        BaseLayerController baseLayerController;
        CardLayerController cardLayerController;
        LinkingLayerController linkingLayerController;
        MenuLayerController menuLayerController;
        SortingBoxLayerController sortingBoxLayerController;
        GlowLayerController glowLayerController;
        ConnectionController connectionController;
        internal DocumentController DocumentController
        {
            get
            {
                return documentController;
            }
        }
        internal TableController TableController
        {
            get
            {
                return tableController;
            }
        }
        internal CardController CardController
        {
            get
            {
                return cardController;
            }
        }
        internal SortingBoxController SortingBoxController
        {
            get
            {
                return sortingBoxController;
            }
        }
        internal TouchController TouchController
        {
            get
            {
                return touchController;
            }
        }
        internal GestureController GestureController
        {
            get
            {
                return gestureController;
            }
        }

        internal BaseLayerController BaseLayerController
        {
            get
            {
                return baseLayerController;
            }
        }
        internal CardLayerController CardLayerController
        {
            get
            {
                return cardLayerController;
            }
        }
        internal LinkingLayerController LinkingLayerController
        {
            get
            {
                return linkingLayerController;
            }
        }
        internal MenuLayerController MenuLayerController
        {
            get
            {
                return menuLayerController;
            }
        }
        internal SortingBoxLayerController SortingBoxLayerController
        {
            get
            {
                return sortingBoxLayerController;
            }
        }

        internal SemanticGroupController SemanticGroupController
        {
            get
            {
                return semanticGroupController;
            }
        }

        internal GlowLayerController GlowLayerController
        {
            get
            {
                return glowLayerController;
            }
        }

        internal LinkingLayerController LinkingLayerController1
        {
            get
            {
                return linkingLayerController;
            }
        }

        internal ConnectionController ConnectionController
        {
            get
            {
                return connectionController;
            }
        }

        internal MLController MlController
        {
            get
            {
                return mlController;
            }

            set
            {
                mlController = value;
            }
        }

        /// <summary>
        /// Initialize all documents
        /// </summary>
        internal async void Init(int width, int height)
        {
            //create controllers
            documentController = new DocumentController(this);
            mlController = new MLController(this);
            tableController = new TableController(this);
            cardController = new CardController(this);
            sortingBoxController = new SortingBoxController(this);
            touchController = new TouchController(this);
            gestureController = new GestureController(this);
            semanticGroupController = new SemanticGroupController(this);
            baseLayerController = new BaseLayerController(this);
            cardLayerController = new CardLayerController(this);
            sortingBoxLayerController = new SortingBoxLayerController(this);
            menuLayerController = new MenuLayerController(this);
            glowLayerController = new GlowLayerController(this);
            connectionController = new ConnectionController(this);
            await StopwordMarker.Load();
            //Initialize controllers
            touchController.Init();
            gestureController.Init();
            baseLayerController.Init(width, height);
            Coordination.Baselayer = baseLayerController.GetBaseLayer();//Set the base layer to the coordination helper
            cardLayerController.Init(width, height);
            sortingBoxLayerController.Init(width, height);
            menuLayerController.Init(width, height);
            glowLayerController.Init(width, height);
            //Load the documents, cards and add them to the card layer
            await documentController.Init(FilePath.NewsArticle);//Load the document
            mlController.Init();
            semanticGroupController.Init();
            cardController.Init();
            cardController.InitDocCard(documentController.GetDocument());
            //Load the sorting box and add them to the sorting box layer
            sortingBoxController.Init();
            SortingBoxLayerController.LoadBoxes(sortingBoxController.GetAllSortingBoxes());
            App app = App.Current as App;
            connectionController.Init(app.AwareCloudController);
        }

        /// <summary>
        /// Destroy the interaction listener
        /// </summary>
        internal void Deinit()
        {
            gestureController.Deinit();
            gestureController = null;
            touchController.Deinit();
            touchController = null;
            sortingBoxController.Deinit();
            sortingBoxController = null;
            cardController.Deinit();
            cardController = null;
            documentController.Deinit();
            documentController = null;
            tableController.Deinit();
            tableController = null;
            baseLayerController.Deinit();
            baseLayerController = null;
            cardLayerController.Deinit();
            cardLayerController = null;
            sortingBoxLayerController.Deinit();
            sortingBoxLayerController = null;
            menuLayerController.Deinit();
            menuLayerController = null;
            connectionController.Deinit();
            connectionController = null;
        }
    }
}
