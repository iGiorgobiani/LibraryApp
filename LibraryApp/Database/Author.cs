using System;
using System.Collections.Generic;

namespace LibraryApp.Database;

public partial class Author
{
    public int AuthorId { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public DateTime? Birthdate { get; set; }

    public byte[]? Cv {  get; set; }

    public string? CvToken { get; set; }
    public string? ImagePath { get; set; }

	public virtual ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();

}
