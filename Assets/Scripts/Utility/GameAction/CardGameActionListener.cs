using UnityEngine;
using Cards;
using UnityEngine.Events;
using Utility.GameAction;

namespace Utility.GameAction
{
    public class CardGameActionListener : MonoBehaviour, IGameActionListener<Card>
    {
        public CardGameAction GameAction;
        public MyCardEvent unityEvent;
        void OnEnable()
        {
            if (GameAction)
            {
                GameAction.RegisterListener(this);
            }
        }
        void OnDisable()
        {
            if (GameAction)
            {
                GameAction.UnRegisterListener(this);
            }
        }

        public void OnEventRaized(Card var)
        {
            unityEvent.Invoke(var);
        }
    }

    [System.Serializable]
    public class MyCardEvent : UnityEvent<Card>
    {
    }
}

