using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LinguaContext.Models;

public class SentenceInFolder
{
    [Key]
    public int Id { get; set; }

    public int FolderId { get; set; }
    public int SentenceId { get; set; }

    public DateTime AddedAt {  get; set; } 
}