using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LinguaContext.Utility;

namespace LinguaContext.Models;

public class PersonalFactors
{
    [Key]
    public int PersonalFactorsId { get; set; }

    [DisplayName("User Id")]
    public int UserId { get; set; } // Required foreign key property

    [DisplayName("Interval Modifier / M")]
    public double IntervalModifier { get; set; } = DefaultSettings.IntervalModifier;

    [DisplayName("Fail Interval Modifier / M1")]
    public double FailIntervalModifier { get; set; } = DefaultSettings.FailIntervalModifier;

    [DisplayName("Hard Interval Modifier / M2")]
    public double HardIntervalModifier { get; set; } = DefaultSettings.HardIntervalModifier;

    [DisplayName("Easy Interval Modifier / M4")]
    public double EasyIntervalModifier { get; set; } = DefaultSettings.EasyIntervalModifier;

    [ForeignKey("UserId")]
    [ValidateNever]
    public User User { get; set; } // Required reference navigation to principal
}
