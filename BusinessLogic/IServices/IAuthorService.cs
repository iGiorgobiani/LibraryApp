
using Model.Author;

namespace BusinessLogic.IServices;

public interface IAuthorService
{
    AuthorsViewModel GetAuthors(AuthorsViewModel model, int? page);

}
