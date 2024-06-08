using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models.Author
{
    public class EditAuthorModel
    {

        public int? AuthorId { get; set; }

        [Required(ErrorMessage = "Required field")]
        [StringLength(100, ErrorMessage = "To long to be true - maximum {1} characters")]
        public string? Firstname { get; set; }

        [Required(ErrorMessage = "Required field")]
        [StringLength(100, ErrorMessage = "To long to be true - maximum {1} characters")]
        public string? Lastname { get; set; }

       // [Required(ErrorMessage = "Required field")]
        //[DataType(DataType.Date, ErrorMessage = "Incorrect format")]
        //public string Birthdate { get; set; }
    }
}
