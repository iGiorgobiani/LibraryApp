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
		public int? GenreId { get; set; }

		//public IEnumerable<SelectListItem> Genres { get; set; }

		//public virtual Genre? Genre { get; set; }
		//public virtual ICollection<BookAuthor> BookAuthors { get; set; }
	}
}
