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
using CoLocatedCardSystem.CollaborationWindow.Layers.Plot_Layer;

namespace CoLocatedCardSystem.CollaborationWindow
{
    /// <summary>
    /// A central controller for all other controllers
    /// </summary>
    public class CentralControllers
    {
        DocumentController documentController;
        TableController tableController;
        CardController cardController;
        SortingBoxController sortingBoxController;
        TouchController touchController;
        GestureController gestureController;
        GestureListenerController listenerController;
        GlowController glowController;
        BaseLayerController baseLayerController;
        CardLayerController cardLayerController;
        LinkingLayerController linkingLayerController;
        PlotLayerController plotLayerController;
        MenuLayerController menuLayerController;
        SortingBoxLayerController sortingBoxLayerController;
        GlowLayerController glowLayerController;
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
        public CardController CardController
        {
            get
            {
                return cardController;
            }
        }
        public SortingBoxController SortingBoxController
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
        internal GestureListenerController ListenerController
        {
            get
            {
                return listenerController;
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

        internal GlowController GlowController
        {
            get
            {
                return glowController;
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

        internal PlotLayerController PlotLayerController
        {
            get
            {
                return plotLayerController;
            }
        }


        /// <summary>
        /// Initialize all documents
        /// </summary>
        public async void Init(int width, int height)
        {
            //create controllers
            documentController = new DocumentController(this);
            tableController = new TableController(this);
            cardController = new CardController(this);
            sortingBoxController = new SortingBoxController(this);
            touchController = new TouchController(this);
            gestureController = new GestureController(this);
            listenerController = new GestureListenerController(this);
            glowController = new GlowController(this);
            baseLayerController = new BaseLayerController(this);
            cardLayerController = new CardLayerController(this);
            sortingBoxLayerController = new SortingBoxLayerController(this);
            plotLayerController = new PlotLayerController(this);
            menuLayerController = new MenuLayerController(this);
            glowLayerController = new GlowLayerController(this);
            //Initialize controllers
            touchController.Init();
            gestureController.Init();
            listenerController.Init();
            glowController.Init();
            baseLayerController.Init(width, height);
            Coordination.Baselayer = baseLayerController.GetBaseLayer();//Set the base layer to the coordination helper
            cardLayerController.Init(width, height);
            sortingBoxLayerController.Init(width, height);
            plotLayerController.Init(width, height);
            menuLayerController.Init(width, height);
            glowLayerController.Init(width, height);
            //Load the documents, cards and add them to the card layer
            await documentController.Init(FilePath.NewsArticle);//Load the document
            await tableController.Init(FilePath.CSVFile);
            cardController.Init();
            cardController.InitDocCard(documentController.GetDocument());
            //await cardController.InitItemCard(tableController.GetItem());
            //await cardController.InitAttributeCard(tableController.GetAttribute());
            //Card[] cards = cardController.AttributeCardController.GetCard();
            //foreach (Card c in cards) {
            //    cardController.MoveCardToTable(c,typeof(AttributeCard));
            //}
            //cardLayerController.LoadCards(cards);
            //Load the sorting box and add them to the sorting box layer
            sortingBoxController.Init();
            SortingBoxLayerController.LoadBoxes(sortingBoxController.GetAllSortingBoxes());
            //Start the gesture detection thread
            gestureController.StartGestureDetection();

        }

        /// <summary>
        /// Destroy the interaction listener
        /// </summary>
        internal void Deinit()
        {
            gestureController.Deinit();
            gestureController = null;
            listenerController.Deinit();
            listenerController = null;
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
            plotLayerController.Deinit();
            plotLayerController = null;
            sortingBoxLayerController = null;
            menuLayerController.Deinit();
            menuLayerController = null;
        }
    }
}
