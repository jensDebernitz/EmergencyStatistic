using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ExcelTabellenAuswerung.Helpers.Validators;

public class ValidationRuleCollection : ValidationRule
{
    public IEnumerable<ValidationRule> Source { get; set; }

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        foreach (var rule in Source)
        {
            var result = rule.Validate(value, cultureInfo);
            if (!result.IsValid)
            {
                return result;
            }
        }
        return ValidationResult.ValidResult;
    }
}