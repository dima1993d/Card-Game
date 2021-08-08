using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Cards
{
    public class CardPickedUpHandler : MonoBehaviour, IPickedUpHandler
    {
        [SerializeField] private UnityEvent onPickedUp, onRelease;
        public void OnCardPickedUp()
        {
            onPickedUp.Invoke();
        }
        public void OnCardReleased()
        {
            onRelease.Invoke();
        }

    }
}

