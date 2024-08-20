using System.ComponentModel.DataAnnotations;

namespace UsersAPI.Attributes
{
    public class NotNullOrEmptyAttribute : ValidationAttribute
    {
        public NotNullOrEmptyAttribute() { }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string stringValue)
            {
                if (string.IsNullOrWhiteSpace(stringValue))
                {
                    // Return a ValidationResult with an error message.
                    return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} cannot be null or empty.");
                }
            }

            // If the value is valid or the value is not a string, return success.
            return ValidationResult.Success;
        }
    }
}
