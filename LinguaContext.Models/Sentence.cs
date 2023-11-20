using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.Json.Serialization;

namespace LinguaContext.Models;

public class Sentence
{
    [Key]
    public int SentenceId { get; set; }

    [Required, NotNull]
    [MaxLength(200)]
    [DisplayName("Sentence")]
    [JsonPropertyName("phrase")]
    public string Phrase { get; set; } = String.Empty;

    [Required, NotNull]
    [MaxLength(200)]
    [DisplayName("Formatted Sentence")]
    [JsonPropertyName("formattedPhrase")]
    public string FormattedPhrase { get; set; } = String.Empty;

    [MaxLength(200)]
    [DisplayName("Translation")]
    [JsonPropertyName("translation")]
    public string Translation { get; set; } = String.Empty;

    [Required, NotNull]
    [MaxLength(50)]
    [DisplayName("Word")]
    [JsonPropertyName("answer")]
    public string Answer { get; set; } = String.Empty;

    [MaxLength(50)]
    [DisplayName("Word Translation")]
    [JsonPropertyName("answerTranslation")]
    public string AnswerTranslation { get; set; } = String.Empty;

    [DisplayName("Complexity Level")]
    [JsonPropertyName("complexityLevel")]
    public string? ComplexityLevel {  get; set; }

    //public int? UserSentenceInfoId { get; set; }

    //[ForeignKey("UserSentenceInfoId")]
    //[ValidateNever]
    //public UserSentenceInfo? UserSentenceInfo { get; set; }
}
