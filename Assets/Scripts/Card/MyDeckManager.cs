using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Utility.GameAction;

namespace Cards
{
    public class MyDeckManager : MonoBehaviour , IDeckManager
    {
        [SerializeField] private Deck myDeck = new Deck();
        [SerializeField] private int deckSize = 30;
        [SerializeField] private Sprite defaultBackgrownd;
        [SerializeField] private List<Texture2D> mainImages = new List<Texture2D>();  
        [SerializeField] private Loader loader;
        [SerializeField] private IntGameAction finishedLoadingImages;
        [SerializeField] private BoolGameAction finishedLoading;
        private async void Awake()
        {
            mainImages = await LoadImages();
            GenerateRandomDeckOfMinions(mainImages, ref myDeck);
            finishedLoadingImages.InvokeAction(deckSize);
            finishedLoading.InvokeAction(true);
        }
        private async Task<List<Texture2D>> LoadImages()
        {
            List<Texture2D> images = new List<Texture2D>();
            for (int i = 0; i < deckSize; i++)
            {
                images.Add(await loader.LoadTexture(367, 512));
            }

            return images;
        }
        private void GenerateRandomDeckOfMinions(List<Texture2D> cardCoverImages, ref Deck deck)
        {
            deck.cardsInDeck.Clear();
            for (int i = 0; i < deckSize; i++)
            {
                if (cardCoverImages[i] == null)
                {
                    return;
                }
                Sprite mySprite = Sprite.Create(cardCoverImages[i], 
                    new Rect(0.0f, 0.0f, cardCoverImages[i].width, cardCoverImages[i].height), 
                    new Vector2(0.5f, 0.5f), 
                    100.0f);
                deck.cardsInDeck.Add(new Minion(mySprite,defaultBackgrownd, 
                    Random.Range(1,5),
                    Random.Range(1,5),
                    Random.Range(1,5)));
            }
            
        }

        public Card GetTopCard()
        {
            if (myDeck.cardsInDeck.Count>0)
            {
                Card minion = myDeck.cardsInDeck[0];
                myDeck.cardsInDeck.RemoveAt(0);
                return minion;
            }
            return null;
        }
    }
}


