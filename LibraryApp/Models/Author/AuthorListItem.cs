using LibraryApp.Models.Author.Attributes;
using LibraryApp.Models.Book;
using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models.Author
{
    public class AuthorListItem

    {
        public int AuthorId { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public DateTime? Birthdate { get; set; }

		public IFormFile? Cv { get; set; }

		public int Booknumber {  get; set; }
        
        [AllowedMimeType("image/jpeg", "image/png", ErrorMessage = "ფაილის ფორმატი უნდა იყოს: jpg, jpeg, png")]
        public byte[]? ImageArray { get; set; }
    }
}
