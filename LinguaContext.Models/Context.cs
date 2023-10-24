using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace LinguaContext.Models;

public class Context
{
    [Key]
    public int ContextId { get; set; }

    [Required, NotNull]
    [DisplayName("User Id")]
    public int UserId { get; set; }

    [Required, NotNull]
    [DisplayName("File Path")]
    public string FilePath { get; set; }

    [MaxLength(200)]
    [DisplayName("Description")]
    public string? Description { get; set; }

    [DisplayName("Creation Time")]
    public DateTime CreatedAt { get; set; }

    [DisplayName("Last Visited")]
    public DateTime LastSeenAt { get; set; }

    [ForeignKey("UserId")]
    [ValidateNever]
    public User User { get; set; }
}
