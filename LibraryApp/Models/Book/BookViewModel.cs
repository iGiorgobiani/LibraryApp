using LibraryApp.Models.Author;
using X.PagedList;

namespace LibraryApp.Models.Book;

public class BookViewModel
{
    //Filters
    public string? Name { get; set; }

    public string? Firstname { get; set; }

    public string? Lastname { get; set; }
	public int Total { get; set; }

	public IPagedList<BookListItem>? Books { get; set; }
}
