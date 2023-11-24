using LinguaContext.Utility;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinguaContext.Models;

[PrimaryKey(nameof(UserId), nameof(SentenceId))]
public class UserTask
{
    public int UserId { get; set; }
    public int SentenceId { get; set; }

    [DisplayName("First Review")]
    public DateTime FirstReview { get; set; }
    [DisplayName("Last Review")]
    public DateTime LastReview { get; set; }
    [DisplayName("Next Review")]
    public DateTime NextReview { get; set; }

    [DisplayName("Repetition Number")]
    public int RepetitionNumber { get; set; } = 0;

    [DisplayName("Interval")]
    public int CurrentInterval { get; set; } = 0;

    [DisplayName("Ease Factor")]
    public double EaseFactor { get; set; } = DefaultSettings.EaseFactor;

    public bool IsPersonalTask { get; set; } = false;
    public int  WrongAnswersNumber { get; set; } = 0;

    [ValidateNever]
    [ForeignKey("UserId")]
    public User User { get; set; }

    [ValidateNever]
    [ForeignKey("SentenceId")]
    public Sentence Sentence { get; set;}
}
