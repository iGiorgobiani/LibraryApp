using BusinessLogic.IServices;
using DataAccess.EF;
using Model.Author;
using Model.Genre;

namespace LibraryApp.Controllers
{
    public class GenreController : Controller
    {
            private readonly IGenreService _genreService;
            public GenreController(IGenreService genreService)
            {
                _genreService = genreService;
            }

            public IActionResult GenreShow(GenreShowModel model, int? page)
            {
                var resultModel = _genreService.GenreShow(model, page);

                return View(resultModel);
            }


            public IActionResult RemoveGenre(int genreId)
        {

            LibraryContext context = new LibraryContext();

            var genre = context.Genres
                .Include(x => x.BookGenres)
                .SingleOrDefault(x => x.GenreId == genreId);
            context.BookGenres.RemoveRange(genre.BookGenres);
            context.Genres.Remove(genre);
            context.SaveChanges();

            return RedirectToAction("GenreShow");
        }
    }
}
