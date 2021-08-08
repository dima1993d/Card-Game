using System;
using System.Collections.Generic;
using UnityEngine;
using Utility.GameAction;
using Random = UnityEngine.Random;

namespace Cards
{
    public class MyCardsManager : MonoBehaviour, IHandManager
    {
        private IDeckManager myDeck;
        //[SerializeField] private List<Card> cardsInHand = new List<Card>();
        [SerializeField] int minStartHandSize = 4;
        [SerializeField] int maxStartHandSize = 6;
        public CardGameAction onCardDraw;
        [SerializeField] private int deckSize = 0;
        [SerializeField] private int numberOfCardsDrawn = 0;
        private void Awake()
        {
            myDeck = GetComponent<IDeckManager>();
            if (myDeck == null)
            {
                Debug.Log("no IDeckManager on this gameObject",gameObject);
            }
        }

        public void DrawCardFromDeck(int num)
        {
            if (deckSize > numberOfCardsDrawn)
            {
                Card topCard = myDeck.GetTopCard();
                if (topCard == null)
                {
                    return;
                }
                onCardDraw.InvokeAction(topCard);
                numberOfCardsDrawn++;
                deckSize++;
            }
        }
        public void DrawStartingHand(int size)
        {
            deckSize = size;
            
            if (deckSize > maxStartHandSize + 1)
            {
                numberOfCardsDrawn = Random.Range(minStartHandSize, maxStartHandSize + 1);
            }
            else
            {
                numberOfCardsDrawn = Random.Range(minStartHandSize, deckSize);
            }
            
            for (int i = 0; i < numberOfCardsDrawn; i++)
            {
                Card topCard = myDeck.GetTopCard();
                if (topCard == null)
                {
                    break;
                }
                //cardsInHand.Add(topCard);
                onCardDraw.InvokeAction(topCard);
            }
        }
    }
}


