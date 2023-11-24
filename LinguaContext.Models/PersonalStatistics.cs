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

    public int ForReviewBaseTasksNumber { get; set; } = 0;
    public int ForReviewUserTasksNumber { get; set; } = 0;

    public int NewBaseTasksNumber       { get; set; } = 0;
    public int NewUserTasksNumber       { get; set; } = 0;

    public int ReviewedBaseTasksNumber  { get; set; } = 0;
    public int ReviewedUserTasksNumber  { get; set; } = 0;

    public int CreatedTasksNumber       { get; set; } = 0;

    [ForeignKey("UserId")]
    [ValidateNever]
    public User User { get; set; }
}
