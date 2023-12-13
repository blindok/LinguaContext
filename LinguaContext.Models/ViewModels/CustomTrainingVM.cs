namespace LinguaContext.Models.ViewModels;

public class CustomTrainingVM
{
    public PersonalSettings Settings { get; set; }
    public PersonalStatistics Statistics { get; set; }
    public Sentence Sentence { get; set; }
    public byte ButtonValue { get; set; }
    public bool WrongAnswer { get; set; }
    public bool IsReviewed { get; set; }
    public string? Comment { get; set; }
}
