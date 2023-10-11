using System.ComponentModel.DataAnnotations;

namespace net_il_mio_fotoalbum.Validation_Attributes
{
    public class MoreThanFiveWords : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string)
            {
                string inputValue = (string)value;

                if (inputValue == null || inputValue.Split(' ').Length < 5)
                {
                    return new ValidationResult("Il campo deve contenere almeno 5 parole!");
                }

                return ValidationResult.Success;

            }

            return new ValidationResult("Il campo inserito non è di tipo stringa");


        }
    }
}
