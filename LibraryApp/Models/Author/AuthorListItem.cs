using LibraryApp.Models.Book;

namespace LibraryApp.Models.Author
{
    public class AuthorListItem

    {
        public int AuthorId { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public DateTime? Birthdate { get; set; }

        public int Booknumber {  get; set; }
    }
}
