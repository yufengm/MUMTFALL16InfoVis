using DataAttribute = CoLocatedCardSystem.CollaborationWindow.TableModule.DataAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoLocatedCardSystem.CollaborationWindow.TableModule;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    public class AttributeCardController:CardController
    {
        AttributeCardList list;

        public AttributeCardController(CentralControllers ctrls) : base(ctrls)
        {

        }

        /// <summary>
        /// Initialize the cardController with a list of documents
        /// </summary>
        /// <param name="items"></param>
        internal async Task Init(DataAttribute[] attributes)
        {
            list = new AttributeCardList();
            foreach (User user in UserInfo.GetLiveUsers())
            {
                foreach (DataAttribute attr in attributes)
                {
                    await list.AddCard(attr, user, this);
                }

                Card[] cardsToBePlaced = GetCard(user);
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
        /// Get the attribute from card
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal DataAttribute GetAttribute(string id)
        {
            AttributeCard card = list.GetCard(id);
            return card.Attribute;
        }
        /// <summary>
        /// Add a card to the user
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="user"></param>
        internal void AddCard(DataAttribute attribute, User user)
        {
            //TO DO: add a document to the user. Information is in user info
            UserInfo userInfo = UserInfo.GetUserInfo(user);

        }
        /// <summary>
        /// Get card by id
        /// </summary>
        /// <param name="cardID"></param>
        /// <returns></returns>
        internal AttributeCard GetCard(string cardID)
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
