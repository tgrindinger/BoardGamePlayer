namespace BoardGamePlayer.Domain;

public class Deck
{
    public IEnumerable<Card> Cards { get; }

    private const int StartingHandSize = 7;

    public Hand DealStartingHand()
    {
        var deck = DeckBuilder.Build();
        return new Hand(deck.Take(StartingHandSize));
    }
}
