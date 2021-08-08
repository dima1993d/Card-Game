using UnityEngine;
using Utility.GameAction;

namespace Cards
{
    public class ReturnAreaCardPositionHandler : MonoBehaviour, ICardPositionHandler
    {
        [SerializeField] private CardInfoGameAction resetCards;
        public void AddCard(CardInfo newCard)
        {
            resetCards.InvokeAction(newCard); 
        }

        public void RemoveCard(CardInfo cardToRemove)
        {
            
        }
    }
}


