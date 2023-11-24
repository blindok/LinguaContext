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

    public int ForReviewBaseTasksNumber { get; set; }
    public int ForReviewUserTasksNumber { get; set; }

    public int NewBaseTasksNumber       { get; set; }
    public int NewUserTasksNumber       { get; set; }

    public int ReviewedBaseTasksNumber  { get; set; }
    public int ReviewedUserTasksNumber  { get; set; }

    public int CreatedTasksNumber       { get; set; }

    [ForeignKey("UserId")]
    [ValidateNever]
    public User User { get; set; }
}
