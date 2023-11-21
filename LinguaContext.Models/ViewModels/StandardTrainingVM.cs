using System.ComponentModel.DataAnnotations;

namespace LinguaContext.Models.ViewModels;

public class StandardTrainingVM
{
    public Sentence   Sentence    { get; set; }
    public UserTask   Task        { get; set; }
    public int        WordsNumber { get; set; }
    public string[]?  Answer      { get; set; }
    public int        ButtonValue { get; set; }
}