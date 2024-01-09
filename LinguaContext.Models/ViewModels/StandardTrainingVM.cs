using System.ComponentModel.DataAnnotations;

namespace LinguaContext.Models.ViewModels;

public class StandardTrainingVM
{
    public PersonalSettings     Settings        { get; set; }
    public PersonalStatistics   Statistics      { get; set; }
    public Sentence             Sentence        { get; set; }
    public byte                 ButtonValue     { get; set; }
    public bool                 WrongAnswer     { get; set; }
    public bool                 IsReviewed      { get; set; }
    public bool                 IsLiked         { get; set; } 
    public bool                 IsUnwanted      { get; set; }
}