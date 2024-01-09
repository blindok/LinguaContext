namespace LinguaContext.Models;

public class FavoriteSentence
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public int SentenceId { get; set; }

    public DateOnly Date { get; set; }
}
