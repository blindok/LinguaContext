using LinguaContext.Models.Validations;
using LinguaContext.Utility;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace LinguaContext.Models;

[Index(nameof(UserName), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
public class User
{
    [Key]
    public int Id { get; set; }

    [Required, NotNull]
    [MaxLength(35)]
    [DisplayName("Никнейм")]
    [UserNameUnique(nameof(Id))]
    //[System.Web.Mvc.Remote("IsUserNameExists", "Authentication", ErrorMessage = "Имя пользователя уже используется")]
    public string UserName { get; set; }

    [Required, NotNull]
    [DisplayName("Почта")]
    [EmailUnique(nameof(Id))]
    //[Remote("IsEmailExists", "Authentication", ErrorMessage = "Почта уже используется")]
    public string Email { get; set; }

    [Required, NotNull]
    [MaxLength(70)]
    [DisplayName("Имя")]
    public string Name { get; set; }

    [Required, NotNull]
    [DisplayName("День Рождения")]
    public DateOnly? BirthDay { get; set; }

    [DisplayName("Аватарка")]
    public string? AvatarUrl { get; set; }

    [Required, NotNull]
    public string Role { get; set; } = SD.Role_User;

    [Required]
    public string PasswordHash { get; set; }

    [ValidateNever]
    public PersonalSettings? PersonalSettings { get; set; }

    [ValidateNever]
    public PersonalFactors? Factors { get; set; } // Reference navigation to dependent
}
