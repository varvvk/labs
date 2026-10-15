using System;
using System.ComponentModel.DataAnnotations;

public class PastDateAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value is DateTime date)
        {
            return date <= DateTime.Now;
        }
        return false;
    }

    public override string FormatErrorMessage(string name)
    {
        return "Дата не может быть в будущем.";
    }
}
