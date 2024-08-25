
using Model.Book;

namespace BusinessLogic.IServices;

public interface IBookService
{
    BookViewModel GetBooks(BookViewModel model, int? page);

}
