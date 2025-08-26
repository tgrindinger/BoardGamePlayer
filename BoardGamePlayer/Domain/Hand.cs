namespace BoardGamePlayer.Domain;

public class Hand
{
    public IEnumerable<Card> Cards { get; }

    public Hand(IEnumerable<Card> cards)
    {
        Cards = cards;
    }
}
