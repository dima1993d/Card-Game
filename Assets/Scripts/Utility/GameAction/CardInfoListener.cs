using Cards;
using UnityEngine;
using UnityEngine.Events;

namespace Utility.GameAction
{
    public class CardInfoListener : MonoBehaviour, IGameActionListener<CardInfo>
    {
        public CardInfoGameAction GameAction;
        public MyRectTransformEvent unityEvent;
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

        public void OnEventRaized(CardInfo var)
        {
            unityEvent.Invoke(var);
        }
    }
    [System.Serializable]
    public class MyRectTransformEvent : UnityEvent<CardInfo>
    {
    }
}
