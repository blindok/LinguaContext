using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;

namespace LinguaContext.Models;

public class PersonalStatistics
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }

    [DisplayName("Date")]
    public DateOnly Date {  get; set; }

    public int ComplitedTaskNumber { get; set; }
    public int CreatedTaskNumber { get; set; }
    public int NewTaskNumber { get; set; }

    [ForeignKey("UserId")]
    [ValidateNever]
    public User User { get; set; }
}
