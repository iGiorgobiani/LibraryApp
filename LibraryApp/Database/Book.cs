using System;
using System.Collections.Generic;

namespace LibraryApp.Database;

public partial class Book
{
    public int BookId { get; set; }

    public string Name { get; set; } = null!;

    public int? Pages { get; set; }

    public int? Year { get; set; }

    public virtual ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();

    public virtual ICollection<BookGenre> BookGenres { get; set; } = new List<BookGenre>();
}
