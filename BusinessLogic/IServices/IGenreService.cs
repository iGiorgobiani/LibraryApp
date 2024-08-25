
using Model.Genre;

namespace BusinessLogic.IServices;

public interface IGenreService
{
    GenreShowModel GenreShow(GenreShowModel model, int? page);

}
