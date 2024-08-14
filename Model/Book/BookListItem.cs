namespace Model.Book;

public class BookListItem
{
    public int BookId { get; set; }

    public string Name { get; set; }

    public int? Year { get; set; }

    public string? Genre { get; set; }

    public List<AuthorViewModel> Authors { get; set; }

    public List<GenreViewModel> Genres { get; set; }
}

public class AuthorViewModel
{
    public string Firstname { get; set; }

    public string Lastname { get; set; }
}

public class GenreViewModel
{
    public string Name { get; set; }
}
