using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace ExcelTabellenAuswerung.Helpers.Validators;

public class NotEmptyValidationRule : ValidationRule
{
    
    public bool Enable { get; set; }

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (Enable == true && (value == null || string.IsNullOrWhiteSpace(value.ToString())))
        {
            return new ValidationResult(false, "Field cannot be empty.");
        }
        return ValidationResult.ValidResult;
    }
}