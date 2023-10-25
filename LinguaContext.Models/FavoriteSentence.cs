using Microsoft.EntityFrameworkCore;

namespace LinguaContext.Models;

[PrimaryKey(nameof(UserId), nameof(SentenceId))]
public class FavoriteSentence
{
    public int UserId { get; set; }
    public int SentenceId { get; set; }

    public DateTime TimeStamp { get; set; }
}
