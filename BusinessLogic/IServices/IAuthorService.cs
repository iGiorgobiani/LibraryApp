
using Model.Author;

namespace BusinessLogic.IServices;

public interface IAuthorService
{
    AuthorsViewModel GetAuthors(AuthorsViewModel model, int? page);

    AddAuthorModel AddAuthor(AddAuthorModel model);

    EditAuthorModel EditAuthor(int? authorId);

    EditAuthorModel EditAuthor(EditAuthorModel model);

    void RemoveAuthor(int authorId);

}
