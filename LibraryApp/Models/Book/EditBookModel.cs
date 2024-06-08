using LibraryApp.Database;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models.Book
{
	public class EditBookModel
	{
		public int BookId { get; set; }
		public string Name { get; set; } = null!;
		public int? Pages { get; set; }
		public int? Year { get; set; }
		public List<BookGenreModel> Genres { get; set; }

	}
}

public class BookGenreModel
{
	public string Name { get; set; }
}
