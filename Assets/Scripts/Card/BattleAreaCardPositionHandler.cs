using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Utility.Bezier;
using Utility.GameAction;
using Utility.Variable;

//[ExecuteInEditMode]
namespace Cards
{
    public class BattleAreaCardPositionHandler : MonoBehaviour, ICardPositionHandler
    {
        public List<CardInfo> cardsOnTheField;
        [SerializeField] private float timeToMoveCard = 0.5F;
        [SerializeField] private float cardWidth = 100;

        [SerializeField] private Vector3 offset = new Vector3(0, -10, 10);
        public CardInfoGameAction removeCardFromAnyHolder;
        //[SerializeField] private CardInfoScriptableObject selectedCard;
        public void AddCard(CardInfo newCard)
        {
            removeCardFromAnyHolder.InvokeAction(newCard);
            cardsOnTheField.Add(newCard);
            SetCanvasGroups(false);
            UpdateCardsPositions();
        }

        public void RemoveCard(CardInfo cardToRemove)
        {
            if (cardsOnTheField.Contains(cardToRemove))
            {
                cardsOnTheField.Remove(cardToRemove);
                UpdateCardsPositions();
            }
        }
        

        public void UpdateCardsPositions()
        {
            int numberOfCardsInHand = cardsOnTheField.Count;
            Vector3 handlerPosition = transform.position;
            for (int i = 0; i < numberOfCardsInHand; i++)
            {
                Vector3 newPos = handlerPosition + new Vector3(i * cardWidth,0,0) - new Vector3(numberOfCardsInHand * cardWidth,0,0)/2 ;
                newPos.z = handlerPosition.z;
                cardsOnTheField[i].rectTransform.DOMove(newPos + offset, timeToMoveCard);
                cardsOnTheField[i].rectTransform.SetSiblingIndex(i);
            }

            DelayedEnableCardsInteractable(timeToMoveCard);
        }

        public void DelayedEnableCardsInteractable(float f)
        {
            StartCoroutine(DelayedEnableInteractable(f));
        }

        private IEnumerator DelayedEnableInteractable(float f)
        {
            yield return new WaitForSeconds(f);
            SetCanvasGroups(true);
        }
        private void SetCanvasGroups(bool state)
        {
            for (int i = 0; i < cardsOnTheField.Count; i++)
            {
                cardsOnTheField[i].SetCanvasGroup(state);
            }
        }
    }
}


