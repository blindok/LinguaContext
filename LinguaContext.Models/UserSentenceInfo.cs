using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;

namespace LinguaContext.Models;

public class UserSentenceInfo
{
    [Key]
    public int UserSentenceInfoId { get; set; }

    [Required, NotNull]
    [DisplayName("User Id")]
    public int UserId { get; set; }

    [Required, NotNull]
    [DisplayName("Id of Source File")]
    public int SourceContextId { get; set; }

    [DisplayName("Creation Time")]
    public DateTime CreatedAt { get; set; }

    [DisplayName("Last Time Edited")]
    public DateTime LastEditedAt { get; set; }

    public int PositionInText { get; set; }

    [DisplayName("Last Seen")]
    public string? Comment { get; set; }

    [ValidateNever]
    [ForeignKey("UserId")]
    public User User {  get; set; }

    [ValidateNever]
    [ForeignKey("SourceContextId")]
    public Context Context { get; set; }
}
