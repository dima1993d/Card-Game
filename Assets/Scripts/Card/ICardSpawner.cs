using Cards;

public interface ICardSpawner
{
    public System.Type TypeOfSpawner();
    public void SpawnCard(Card card);
}