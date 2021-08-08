using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Utility.Variable;

namespace Cards
{
    public class DropArea : MonoBehaviour, IDropHandler
    {
        public CardInfoScriptableObject currentDraggedCard;
        public ICardPositionHandler dropAreaPositionHandler;

        private void Awake()
        {
            dropAreaPositionHandler = GetComponent<ICardPositionHandler>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (currentDraggedCard.cardInfo != null)
                {
                    dropAreaPositionHandler.AddCard(currentDraggedCard.cardInfo);
                    currentDraggedCard.Clear();
                
                }
            }
        }
    
    }
}

