using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace LinguaContext.Models.ViewModels;

public class SentenceVM
{
    [Required, NotNull]
    [MaxLength(350)]
    public string Sentence { get; set; }

    [MaxLength(350)]
    public string Translation { get; set; }

    [Required, NotNull]
    [MaxLength(50)]
    public string Word { get; set; }

    [MaxLength(50)]
    public string WordTranslation { get; set; }

    [MaxLength(200)]
    public string? Comment { get; set; }
}
