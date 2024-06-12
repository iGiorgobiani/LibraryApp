using LibraryApp.Database;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace LibraryApp.Models.Book
{
    public class AddBookModel
    {
        [Required(ErrorMessage = "Required field")]
        [StringLength(100, ErrorMessage = "To long to be true - maximum {1} characters")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Range(0, 9999, ErrorMessage = "To long to be true - maximum {1} characters")]
        public int? Year { get; set; }

        public virtual Genre? Genre { get; set; }

        public List<int>? SelectedGenreIds { get; set; }
        public IEnumerable<SelectListItem>? GenreSelectList { get; set; }
    }
}

