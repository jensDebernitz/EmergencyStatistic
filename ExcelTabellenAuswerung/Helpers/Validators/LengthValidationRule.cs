using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ExcelTabellenAuswerung.Helpers.Validators;

/// <inheritdoc />

public class LengthValidationRule :  ValidationRule
{
    public int MinLength { get; set; }

    public bool Enable { get; set; } = false;

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        BindingGroup group = value as BindingGroup;

        if (group != null)
        {
           IList temp = group.Items;
        }

        if (Enable == true && (value == null || value.ToString().Length < MinLength))
        {
            return new ValidationResult(false, $"Field must be at least {MinLength} characters long.");
        }
        return ValidationResult.ValidResult;
    }
}