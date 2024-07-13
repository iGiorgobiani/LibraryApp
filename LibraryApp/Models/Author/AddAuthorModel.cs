using LibraryApp.Models.Author.Attributes;
using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models.Author;

public class AddAuthorModel
{
	[Required(ErrorMessage="სავალდებულო ველი")]
	[StringLength(100, ErrorMessage ="To long to be true - maximum {1} characters")]
	public string? Firstname { get; set; }

	[Required(ErrorMessage = "სავალდებულო ველი")]
	[StringLength(100, ErrorMessage = "To long to be true - maximum {1} characters")]
	public string? Lastname { get; set; }

	[Required(ErrorMessage = "სავალდებულო ველი")]
	[DataType(DataType.Date, ErrorMessage ="ფორმატი არასწორია")]
	public DateTime Birthdate { get; set; }
    [AllowedMimeType("application/pdf", ErrorMessage = "ფაილის ფორმატი უნდა იყოს: pdf")]
    public IFormFile? Cv { get; set; }
    [AllowedMimeType("image/jpeg", "image/png", ErrorMessage = "ფაილის ფორმატი უნდა იყოს: jpg, jpeg, png")]
    public IFormFile? Image { get; set; }
}
