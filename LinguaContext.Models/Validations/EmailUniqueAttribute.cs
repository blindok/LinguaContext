using System.ComponentModel.DataAnnotations;
using LinguaContext.DataAccess.Data;

namespace LinguaContext.Models.Validations;

public class EmailUniqueAttribute : ValidationAttribute
{
    private readonly string _IdPropertyName;

    public EmailUniqueAttribute(string IdPropertyName)
    {
        _IdPropertyName = IdPropertyName;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        string name = value.ToString();
        var property = validationContext.ObjectType.GetProperty(_IdPropertyName);
        if (property != null) 
        {
            var idValue = (int)property.GetValue(validationContext.ObjectInstance, null)!;

            var context = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext))!;
            var entity = context.Users.FirstOrDefault(u => u.Email == value.ToString() && u.Id != idValue);

            //var context = (DbContext)validationContext.GetService(typeof(DbContext))!;
            //var entity = context.Set<User>().FirstOrDefault(u => u.UserName == value.ToString() && u.Id != idValue);

            if (entity != null)
            {
                return new ValidationResult(GetErrorMessage(value.ToString()));
            }
        }
        return ValidationResult.Success;
        //return base.IsValid(value, validationContext);
    }

    public string GetErrorMessage(string name)
    {
        return $"Name {name} is already in use.";
    }
}
