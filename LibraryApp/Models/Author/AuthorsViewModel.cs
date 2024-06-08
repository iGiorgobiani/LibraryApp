using LibraryApp.Models.Book;
using X.PagedList;

namespace LibraryApp.Models.Author
{
    public class AuthorsViewModel
    {
        public string? Firstname { get; set; }

        public string? Lastname { get; set; }

        public int Total {  get; set; }

        public IPagedList<AuthorListItem> Authors { get; set; }
    }

}
