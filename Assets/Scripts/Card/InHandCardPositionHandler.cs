using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using DG.Tweening;
using UnityEngine;
using Utility.Bezier;
using Utility.GameAction;
using Utility.Variable;

//[ExecuteInEditMode]
namespace Cards
{
    public class InHandCardPositionHandler : MonoBehaviour, ICardPositionHandler
    {
        public List<CardInfo> cardsInHand;
        public List<CardInfo> queue;
        //public List<Vector3>  newPoss;
        public float arcRadius;
        public float arcAngle;
        [SerializeField] private float timeToMoveCardToHand = 0.5F;
        [SerializeField] private float timeToDrawCard = 1;
        public Vector3 arcOffset;
        public BezierSpline drawFromDeckPath;
        //private int numberOfCardsInHand = 0;
        private List<Vector3> points;
        [SerializeField] private BoolVariable cardsAreClickable; //Set Cards to not Clickable when drawing cards 
        [SerializeField] private int degreesPerCard = 3;
        void Start()
        {
            points = new List<Vector3>(drawFromDeckPath.points);
            points.RemoveAt(0);//tween does not need first point, it uses objects position TODO: move to separate method and make it work for all list lengths
            Vector3 pointToMove = points[2];
            points.RemoveAt(2);
            points.Insert(0,pointToMove);//reposition point 
        
            DOTween.Init();
            //DrawCardFromDeck(timeToDrawCard);
        }

        public void RemoveCard(CardInfo cardToPickUp)
        {
            if (cardsInHand.Contains(cardToPickUp))
            {
                cardsInHand.Remove(cardToPickUp);
                UpdateCardsPositions();
            }
        }
        public void AddCard(CardInfo newCard)
        {
            cardsInHand.Add(newCard);
            queue.Add(newCard);
            cardsAreClickable.Value = false;
            if (queue.Count > 1)
            {
                return;
            }
            DrawCardFromDeck(timeToDrawCard);
        }
        
        public void RandomizeCardValues(int variable)
        {
            StartCoroutine(DelayedRandomizeCardValues(0.25F));

        }
        private IEnumerator DelayedRandomizeCardValues(float delay)
        {
            for (int i = 0; i < cardsInHand.Count; i++)
            {
                if (cardsInHand[i].GetType() == typeof(MinionCardInfo))
                {
                    MinionCardInfo minionCardInfo = (MinionCardInfo) cardsInHand[i];
                    int valueIndex = Random.Range(0, 3); //select random value
                    switch (valueIndex)
                    {
                        case 0:
                        {
                            minionCardInfo.Info.cost = Random.Range(-2, 9);
                            break;
                        }
                        case 1:
                        {
                            minionCardInfo.Info.attack = Random.Range(-2, 9);
                            break;
                        }
                        case 2:
                        {
                            minionCardInfo.Info.health = Random.Range(-2, 9);
                            break;
                        }
                    } 
                    minionCardInfo.UpdateInfo(minionCardInfo.Info);
                    yield return new WaitForSeconds(delay);
                }
                
            }

            if (cardsInHand.Count>0)
            {
                RandomizeCardValues(0);
            }
            
        }
        public void DrawCardFromDeck(float delay)
        {
            queue[0].rectTransform.DOPath(points.ToArray(), delay, PathType.CubicBezier, PathMode.Full3D, 10, Color.green);
            queue[0].rectTransform.DORotate(-transform.forward, delay, RotateMode.Fast);
            StartCoroutine(DelayedUpdatePositions(delay));
        }

        private IEnumerator DelayedUpdatePositions(float delay)
        {
            yield return new WaitForSeconds(delay);
            queue.RemoveAt(0);
            //Debug.Log(Time.timeSinceLevelLoad.ToString());
            UpdateCardsPositions();
        }
        public void UpdateCardsPositions()
        {
            int numberOfCardsInHand = cardsInHand.Count - queue.Count;
            Vector3 handlerPosition = transform.position;
            for (int i = 0; i < numberOfCardsInHand; i++)
            {
                Vector3 newPos;
            
                if (numberOfCardsInHand == 1)
                {
                    newPos = GetPositionOnCircle(handlerPosition + arcOffset, arcRadius, 90);
                    newPos.z = handlerPosition.z;
                    cardsInHand[i].rectTransform.DOMove(newPos, timeToMoveCardToHand);
                    break;
                }
                arcAngle = numberOfCardsInHand * degreesPerCard;
                float relativeAngle = (arcAngle * 2 / (numberOfCardsInHand - 1) * i) - 90 - arcAngle;
                newPos = GetPositionOnCircle(handlerPosition + arcOffset, arcRadius, -relativeAngle);
                newPos.z = handlerPosition.z;
                cardsInHand[i].rectTransform.DOMove(newPos, timeToMoveCardToHand);
                cardsInHand[i].rectTransform.SetSiblingIndex(cardsInHand[i].rectTransform.parent.childCount - numberOfCardsInHand + i);
            }
            if (queue.Count > 0)
            {
                DrawCardFromDeck(timeToDrawCard);
            }
            else
            {
                DelayedEnableCardsInteractable(timeToMoveCardToHand);
            }
        }
        
        public void DelayedEnableCardsInteractable(float f)
        {
            StartCoroutine(DelayedEnableInteractable(f));
        }

        private IEnumerator DelayedEnableInteractable(float f)
        {
            yield return new WaitForSeconds(f);
            SetCanvasGroups(true);
        }
        private void SetCanvasGroups(bool state)
        {
            for (int i = 0; i < cardsInHand.Count; i++)
            {
                cardsInHand[i].SetCanvasGroup(state);
            }

            cardsAreClickable.Value = true;
        }


        public Vector2 GetPositionOnCircle(Vector2 center, float radius, float angle) {

            Vector2 p = new Vector2((float) (center.x + radius * Mathf.Cos(Mathf.Deg2Rad * angle)), (float) (center.y + radius* Mathf.Sin(Mathf.Deg2Rad * angle)));

            return p;
        }
        
#if UNITY_EDITOR
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            int numberOfCardsInHand = cardsInHand.Count - queue.Count;
            Vector3 positionOfHolder = transform.position;
            for (int i = 0; i < numberOfCardsInHand; i++)
            {
                Vector3 newPos;
                if (numberOfCardsInHand == 1)
                {
                    newPos = GetPositionOnCircle(transform.position + arcOffset, arcRadius, 90);
                    newPos.z = positionOfHolder.z;
                    Gizmos.DrawSphere(newPos, 2) ;
                    break;
                }
                arcAngle = numberOfCardsInHand * degreesPerCard;
                float relativeAngle = (arcAngle * 2 / (numberOfCardsInHand - 1) * i) - 90 - arcAngle;
                newPos = GetPositionOnCircle(positionOfHolder + arcOffset, arcRadius, -relativeAngle);
                newPos.z = positionOfHolder.z;
                //newPos = transform.position - new Vector3(newPos.x,newPos.y,0) ;
                Gizmos.DrawSphere(newPos, 2) ;

            }
        }
    }
#endif
}


