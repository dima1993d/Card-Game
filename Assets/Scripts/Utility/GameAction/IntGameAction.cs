using UnityEngine;

namespace Utility.GameAction
{
    [CreateAssetMenu(fileName = "New IntGameAction", menuName = "GameAction/IntGameAction", order = 50)]
    public class IntGameAction : GameAction<int>
    {

        public void Trigger(int num)
        {
            InvokeAction(num);
        }
    }
}
