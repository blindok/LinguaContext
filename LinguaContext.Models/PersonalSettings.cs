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

    public int NewDailyCardsNumber { get; set; } = DefaultSettings.NewDailyCardsNumber;

    [ForeignKey("UserId")]
    [ValidateNever]
    public User User { get; set; }
}
