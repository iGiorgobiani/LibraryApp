

namespace Model.Book
{
    public class AddBookModel
    {
        [Required(ErrorMessage = "სავალდებულო ველი")]
        [StringLength(100, ErrorMessage = "შეგიძლიათ შეიყვანოთ არაუმეტეს {1} სიმბოლო")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "სავალდებულო ველი")]
        [Range(0, 9999, ErrorMessage = "შეგიძლიათ შეიყვანოთ არაუმეტეს {1} სიმბოლო")]
        public int? Year { get; set; }

		[Required(ErrorMessage = "სავალდებულო ველი")]
		public List<int>? SelectedGenreIds { get; set; }
		[Required(ErrorMessage = "სავალდებულო ველი")]
		public List<int>? SelectedAuthorIds { get; set; }

    }
}

