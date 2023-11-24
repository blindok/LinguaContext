using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using LinguaContext.Utility;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinguaContext.Models;

public class PersonalSettings
{
    [Key]
    public int PersonalSettingsId { get; set; }

    public int UserId { get; set; }

    [Range(0, 50)]
    public int NewDailyCardsNumber { get; set; } = DefaultSettings.NewDailyCardsNumber;

    public bool HighlightAnswer    { get; set; } = DefaultSettings.HighlightAnswer;
    public bool DisplayTranslation { get; set; } = DefaultSettings.DisplayTranslation;

    [ForeignKey("UserId")]
    [ValidateNever]
    public User User { get; set; }
}
