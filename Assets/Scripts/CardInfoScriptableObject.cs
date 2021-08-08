using System.Collections;
using System.Collections.Generic;
using Cards;
using Unity.VisualScripting;
using UnityEngine;

namespace Utility.Variable
{
    [CreateAssetMenu(fileName = "CardInfo", menuName = "ScriptableObjects/CardInfoScriptableObject", order = 1)]
    public class CardInfoScriptableObject : ScriptableObject
    {
        public CardInfo cardInfo;

        public void SetInfo(CardInfo cardInfo)
        {
            this.cardInfo = cardInfo;
            OnPickedUp();
        }
        public void Clear()
        {
            OnCardReleased();
            cardInfo = null;
        }
        private void OnPickedUp()
        {
            if (cardInfo != null && cardInfo.pickedUpHandler != null)
            {
                cardInfo.pickedUpHandler.OnCardPickedUp();
            }
        }
        private void OnCardReleased()
        {
            if (cardInfo != null && cardInfo.pickedUpHandler != null)
            {
                cardInfo.pickedUpHandler.OnCardReleased();
            }
        }
        
    }

}
