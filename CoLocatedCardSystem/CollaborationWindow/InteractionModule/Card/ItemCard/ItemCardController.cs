using CoLocatedCardSystem.CollaborationWindow.TableModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    public class ItemCardController:CardController
    {
        ItemCardList list;

        public ItemCardController(CentralControllers ctrls) : base(ctrls)
        {
        }

        /// <summary>
        /// Initialize the cardController with a list of documents
        /// </summary>
        /// <param name="items"></param>
        internal async Task Init(DataRow[] items)
        {
            list = new ItemCardList();
            foreach (User user in UserInfo.GetLiveUsers())
            {
                for (int i = 0; i < 10; i++)
                {
                    await list.AddCard(items[i], user, this);
                }

                Card[] cardsToBePlaced = GetCard(user);//debug
                CardLayoutGenerator.ApplyLayout(cardsToBePlaced, user);
            }
        }
        /// <summary>
        /// Destroy the card list
        /// </summary>
        internal void Deinit()
        {
            list.Clear();
        }
        /// <summary>
        /// Add a card to the user
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="user"></param>
        internal void AddCard(DataRow item, User user)
        {
            //TO DO: add a document to the user. Information is in user info
            UserInfo userInfo = UserInfo.GetUserInfo(user);

        }
        /// <summary>
        /// Get card by id
        /// </summary>
        /// <param name="cardID"></param>
        /// <returns></returns>
        internal Card GetCard(string cardID)
        {
            return list.GetCard(cardID);
        }
        /// <summary>
        /// Get all the cards.
        /// </summary>
        /// <returns></returns>
        internal Card[] GetCard()
        {
            return list.GetCard();
        }
        /// <summary>
        /// Get all the cards belong to a user. The returned results are references.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        internal Card[] GetCard(User user)
        {
            return list.GetCard(user);
        }        
    }
}
