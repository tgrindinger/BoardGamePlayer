namespace BoardGamePlayer.Domain;

public class Game
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required Guid UserId { get; set; }
    public required GameStatus GameStatus { get; set; }
}
