using System.ComponentModel.DataAnnotations;

namespace FarmFresh.Infrastructure.Validation;

public class PrivacyAcceptedAttribute : ValidationAttribute
{
    public override bool IsValid(object value) =>
         value is bool && (bool)value;
    
    public override string FormatErrorMessage(string name) =>
         $"You must accept the Privacy Policy and Terms of Service to proceed.";
}
