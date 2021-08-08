using Cards;
using UnityEngine;
using Utility.GameAction;

public class MinionCardSpawner : MonoBehaviour, ICardSpawner
{
    public GameObject minionPrefab;
    public Transform spawnPosition;
    public Transform spawnParent;
    public CardInfoGameAction onCardSpawned;
    public System.Type TypeOfSpawner()
    {
        return typeof(Minion);
    }

    public void SpawnCard(Card card)
    {
        GameObject cardInstance = Instantiate(minionPrefab,spawnPosition.position,spawnPosition.rotation, spawnParent);
        MinionCardInfo info = cardInstance.GetComponent<MinionCardInfo>();
        info.Info = (Minion)card;
        onCardSpawned.InvokeAction(info);
    }
}
