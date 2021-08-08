using UnityEngine;
using Cards;

namespace Utility.GameAction
{
    [CreateAssetMenu(fileName = "New CardGameAction", menuName = "GameAction/CardGameAction", order = 50)]
    public class CardGameAction : GameAction<Card>
    {
        public void Trigger(Card num)
        {
            InvokeAction(num);
        }
    }
}