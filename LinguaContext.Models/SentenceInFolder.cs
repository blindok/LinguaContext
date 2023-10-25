using Microsoft.EntityFrameworkCore;

namespace LinguaContext.Models;

[PrimaryKey(nameof(FolderId), nameof(SentenceId))]
public class SentenceInFolder
{
    public int FolderId { get; set; }
    public int SentenceId { get; set; }

    public DateTime AddedAt {  get; set; } 
}