using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public class CardSpawnersController : MonoBehaviour
    {
        private Dictionary<Type, ICardSpawner> spawnerTypes = new Dictionary<Type, ICardSpawner>();
        void Awake()
        {
            ICardSpawner[] temp = GetComponents<ICardSpawner>();
            for (int i = 0; i < temp.Length; i++)
            {
                spawnerTypes.Add(temp[i].TypeOfSpawner(),temp[i]);
            }
        }

        public void SpawnAnyCard(Card card)
        {
            spawnerTypes[card.GetType()]?.SpawnCard(card);
        }
    }
}

