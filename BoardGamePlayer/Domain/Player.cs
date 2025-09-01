namespace BoardGamePlayer.Domain;

public class Player
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid GameId { get; set; }
}
