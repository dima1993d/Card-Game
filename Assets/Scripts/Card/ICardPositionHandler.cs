namespace Cards
{
    public interface ICardPositionHandler
    {
        public void AddCard(CardInfo newCard);
        public void RemoveCard(CardInfo cardToRemove);
        
    }
}