using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Database;

public partial class BookGenre
{
	[Key]
    public int BookGenreId { get; set; }

    public int? BookId { get; set; }

    public int? GenreId { get; set; }

    public virtual Book? Book { get; set; }

    public virtual Genre? Genre { get; set; }
}
