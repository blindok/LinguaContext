using System.ComponentModel.DataAnnotations;

namespace LinguaContext.Models.ViewModels;

public class StandardTrainingVM
{
    public PersonalSettings Settings        { get; set; }
    public Sentence         Sentence        { get; set; }
    public byte             WordsNumber     { get; set; }
    public byte             ButtonValue     { get; set; }
    public bool             WrongAnswer     { get; set; }
}