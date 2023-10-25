using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace LinguaContext.Models;

public class User
{
    [Key]
    public int UserId { get; set; }

    [Required, NotNull]
    [MaxLength(35)]
    [DisplayName("User Name")]
    public string UserName { get; set; }

    [Required, NotNull]
    [DisplayName("Email")]
    public string Email { get; set; }

    [Required, NotNull]
    [MaxLength(35)]
    [DisplayName("First Name")]
    public string FirstName { get; set; }

    [MaxLength(35)]
    [DisplayName("Last Name")]
    public string? LastName { get; set; }

    [Required, NotNull]
    [DisplayName("Birth Day")]
    public DateOnly BirthDay { get; set; }

    [DisplayName("Interval Modifier")]
    public double PersonalIntervalModifier { get; set; } = 1;
}
