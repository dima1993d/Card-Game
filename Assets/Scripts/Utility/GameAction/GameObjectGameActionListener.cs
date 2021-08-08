using UnityEngine;
using UnityEngine.Events;

namespace Utility.GameAction
{
    public class GameObjectGameActionListener : MonoBehaviour, IGameActionListener<GameObject>
    {
        public GameObjectGameAction GameAction;
        public MyGameObjectEvent unityEvent;
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

        public void OnEventRaized(GameObject var)
        {
            unityEvent.Invoke(var);
        }
    }
    [System.Serializable]
    public class MyGameObjectEvent : UnityEvent<GameObject>
    {
    }
}
