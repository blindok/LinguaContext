using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using LinguaContext.Utility;

namespace LinguaContext.Models;

public class Folder
{
    [Key]
    public int FolderId { get; set; }

    [Required, NotNull]
    [MaxLength(35)]
    [DisplayName("Folder Name")]
    public string FolderName { get; set; }

    [MaxLength(200)]
    [DisplayName("Description")]
    public string? Description { get; set; }

    [Required, NotNull]
    [DisplayName("Sharing Status")]
    public string SharingStatusCode { get; set; } = SD.PrivateFolderStatus;
}
