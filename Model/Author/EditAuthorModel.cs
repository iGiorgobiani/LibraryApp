using Model.Helper.Attributes;

namespace Model.Author
{
    public class EditAuthorModel
    {

        public int? AuthorId { get; set; }

        [Required(ErrorMessage = "სავალდებულო ველი")]
        [StringLength(100, ErrorMessage = "To long to be true - maximum {1} characters")]
        public string? Firstname { get; set; }

        [Required(ErrorMessage = "სავალდებულო ველი")]
        [StringLength(100, ErrorMessage = "To long to be true - maximum {1} characters")]
        public string? Lastname { get; set; }

        public DateTime? Birthdate { get; set; }

        [AllowedMimeType("application/pdf", ErrorMessage = "ფაილის ტიპი უნდა იყოს pdf")]
        public IFormFile? Cv { get; set; }

        public bool HasCv { get; set; } = true; //vicodet aqvs tu ara Tu aqvs gamovachent links Tu ara ara

        public string? CvToken { get; set; }

        public bool HasImage { get; set; } = false;
        public Byte[]? ImageArray { get; set; }
        [AllowedMimeType("image/jpeg", "image/png", ErrorMessage = "ფაილის ტიპი უნდა იყოს jpg, jpeg, png")]
        public IFormFile? Image { get; set; }

		// [Required(ErrorMessage = "Required field")]
		//[DataType(DataType.Date, ErrorMessage = "Incorrect format")]
		//public string Birthdate { get; set; }
	}
}
