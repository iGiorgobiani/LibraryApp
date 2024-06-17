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

//BOOKGENRE

		public List<BookGenreModel>? Genres { get; set; } //To show what genres book have (according to addbookmodel)
        public List<int>? SelectedGenreIds { get; set; } 
		public List<int>? SelectedRemoveIds { get; set; }

//BOOKAUTHOR
		public List<BookAuthorEditModel>? Authors { get; set; } //To show which authors book have (according to addbookmodel)
		public List<int>? SelectedAuthorIds { get; set; }
		public List<int>? SelectedRemoveAuthorIds { get; set; }


	}
}

public class BookGenreModel
{
	public string Name { get; set; }

	public int? BookId { get; set; }	

	public int? GenreId { get; set; }
}

public class BookAuthorEditModel
{
	public string Lastname { get; set; }

	public int? BookId { get; set; }

	public int? AuthorId { get; set; }

}
