using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Model.Helper.Attributes;

public class AllowedMimeTypeAttribute : ValidationAttribute
{
    private readonly List<string> _allowedMimeTypes;

    public AllowedMimeTypeAttribute(params string[] allowedMimeTypes)
    {
        _allowedMimeTypes = new List<string>(allowedMimeTypes);
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is IFormFile file)
        {
            if (_allowedMimeTypes.Contains(file.ContentType))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(GetErrorMessage());
            }
        }

        return ValidationResult.Success;
    }

    private string GetErrorMessage()
    {
        return ErrorMessage ?? "The MIME type of the uploaded file is not allowed.";
    }
}
