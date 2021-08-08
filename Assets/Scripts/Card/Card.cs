using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cards
{
    [Serializable]
    public class Card
    {
        [FormerlySerializedAs("Main image")] public Sprite main;
        [FormerlySerializedAs("Background image")] public Sprite background;
        public int cost = 1;
        public bool visible = true;
        public string name;
        public string description;
        
        public Card(Sprite main, Sprite background, int cost)
        {
            this.cost = cost;
            this.main = main;
            this.background = background;
            
        }

        protected Card()
        {
            cost = 1;
            visible = true;
        }
    }
}



