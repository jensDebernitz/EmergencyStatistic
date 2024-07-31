using ExcelTabellenAuswerung.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace ExcelTabellenAuswerung.Helpers.Validators;

public class ClinicalValidationRule : ValidationRule
{
    public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
    {
        EditEmergencyData data = (value as BindingGroup).Items[0] as EditEmergencyData;


        if (data.EnableLengthValidation)
        {
            int intValue = data.ClinicalEvaluation.Length;

            if (data.MaxLength != -1)
            {
                if (intValue > data.MaxLength)
                {
                    return new ValidationResult(false, "Der Eingegebene Wert hat eine falsche maximale Länge!");
                }
            }

            if (data.MinLength != -1)
            {
                if (intValue < data.MinLength)
                {
                    return new ValidationResult(false, "Der Eingegebene Wert hat eine falsche minimale Länge!");
                }
            }

        }

        if (data.EnableNoEmptyValidation)
        {
            if (string.IsNullOrWhiteSpace(data.ClinicalEvaluation))
            {
                return new ValidationResult(false, "Der Eingegebene Wert darf nicht leer sein!");
            }
        }

        return ValidationResult.ValidResult;

    }
}

