using System;
using UnityEngine;

namespace Cards
{
    [Serializable]
    public class Minion : Card
    {
        public int attack = 1, health = 1;

        public Minion(Sprite main, Sprite background, int cost,int attack,int health)
        {
            this.cost = cost;
            this.health = health;
            this.attack = attack;
            this.main = main;
            this.background = background;
            
        }
    }
}

