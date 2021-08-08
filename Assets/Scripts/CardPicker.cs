using System;
using Cards;
using UnityEngine;
using UnityEngine.EventSystems;
using Utility.GameAction;
using Utility.Variable;
[RequireComponent(typeof(CardInfo))]
public class CardPicker : MonoBehaviour, IPointerDownHandler
{
    public CardInfoScriptableObject currentDraggedCard;
    private CardInfo cardInfo;
    [SerializeField] private BoolVariable cardsAreClickable;
    [SerializeField] private CardInfoGameAction resetCardPositions;
    private void Awake()
    {
        cardInfo = GetComponent<CardInfo>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (!cardsAreClickable.Value)
            {
                return;
            }
            if (currentDraggedCard.cardInfo != null)
            {
                resetCardPositions.InvokeAction(currentDraggedCard.cardInfo);
                currentDraggedCard.Clear();
                return;
            }

            PickUpThisCard();
        }

    }

    void PickUpThisCard()
    {
        cardInfo.SetCanvasGroup(false); 
        currentDraggedCard.SetInfo(cardInfo);
    }
}
