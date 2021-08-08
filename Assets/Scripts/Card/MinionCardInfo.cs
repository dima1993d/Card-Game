using System;
using TMPro;
using UnityEngine;
using Utility.GameAction;

namespace Cards
{
    [RequireComponent(typeof(CanvasGroup))]
    public class MinionCardInfo : CardInfo
    {
        public Minion Info
        {
            get { return minionInfo; }
            set => UpdateInfo(value);
        }
        [SerializeField] private TextMeshProUGUI attackText, healthText;
        [SerializeField] private Minion minionInfo = new Minion(null,null,1,1,1);
        [SerializeField] private CardInfoGameAction destroyCard;
        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            if (canvasGroup == null)
            {
                canvasGroup = GetComponent<CanvasGroup>();
            }
            if (pickedUpHandler == null)
            {
                pickedUpHandler = GetComponent<IPickedUpHandler>();
            }
        }

        public void UpdateInfo(Minion value)
        {
            minionInfo = value;
            main.sprite = value.main;
            nameText.text = value.name;
            descriptionText.text = value.description;
            costText.text = value.cost.ToString();
            attackText.text = value.attack.ToString();
            healthText.text = value.health.ToString();
            if (value.health < 1)
            {
                destroyCard.InvokeAction(this);
                Destroy(gameObject); 
            }
        }

        public override void SetCanvasGroup(bool state)
        {
            canvasGroup.blocksRaycasts = state;
            canvasGroup.interactable = state;
        }
    }
}