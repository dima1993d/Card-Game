using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Cards
{
    public abstract class CardInfo : MonoBehaviour
    {
        [SerializeField] protected TextMeshProUGUI nameText, descriptionText, costText;
        [SerializeField] protected Image main;
        public RectTransform rectTransform;
        [SerializeField] protected CanvasGroup canvasGroup;
        public IPickedUpHandler pickedUpHandler;
        public abstract void SetCanvasGroup(bool state);
        
    }
}
