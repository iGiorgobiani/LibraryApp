﻿using System;
using System.Collections.Generic;

namespace LibraryApp.Database;

public partial class BookGenre
{
    public int BookGenreId { get; set; }

    public int? BookId { get; set; }

    public int? GenreId { get; set; }
}
