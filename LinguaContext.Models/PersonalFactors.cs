using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinguaContext.Models;

public class PersonalFactors
{
    [Key]
    public int PersonalFactorsId { get; set; }

    public int UserId { get; set; }

    [DisplayName("Interval Modifier / M")]
    public double IntervalModifier { get; set; } = 1.0;

    [DisplayName("Fail Interval Modifier / M1")]
    public double FailIntervalModifier { get; set; } = 0.0;

    [DisplayName("Hard Interval Modifier / M2")]
    public double HardIntervalModifier { get; set; } = 1.2;

    [DisplayName("Easy Interval Modifier / M3")]
    public double EasyIntervalModifier { get; set; } = 1.3;

    [ForeignKey("UserId")]
    [ValidateNever]
    public User User { get; set; }
}
