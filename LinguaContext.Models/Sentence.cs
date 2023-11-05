using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace LinguaContext.Models;

public class Sentence
{
    [Key]
    public int SentenceId { get; set; }

    [Required, NotNull]
    [MaxLength(200)]
    [DisplayName("Sentence")]
    public string Phrase { get; set; }

    [Required, NotNull]
    [MaxLength(200)]
    [DisplayName("Formatted Sentence")]
    public string FormattedPhrase { get; set; }

    [MaxLength(200)]
    [DisplayName("Translation")]
    public string Translation { get; set; }

    [Required, NotNull]
    [MaxLength(50)]
    [DisplayName("Word")]
    public string Answer { get; set; }

    [MaxLength(50)]
    [DisplayName("Word Translation")]
    public string AnswerTranslation { get; set; }

    [DisplayName("Complexity Level")]
    public string? ComplexityLevel {  get; set; }

    public int? UserSentenceInfoId { get; set; }

    [ForeignKey("UserSentenceInfoId")]
    [ValidateNever]
    public UserSentenceInfo? UserSentenceInfo { get; set; }
}
