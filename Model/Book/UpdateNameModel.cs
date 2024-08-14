

namespace Model.Book;

public class UpdateNameModel
{
    public int BookId { get; set; }

    [Required(ErrorMessage = "სავალდებულო ველი")]
    [StringLength(100, ErrorMessage = "შეგიძლიათ შეიყვანოთ არაუმეტეს {1} სიმბოლო")]
    public string Name { get; set; }
}
