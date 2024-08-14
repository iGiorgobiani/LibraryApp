using DataAccess.EF;
using Model.Genre;

namespace LibraryApp.Controllers
{
    public class GenreController : Controller
    {

        public IActionResult GenreShow(GenreShowModel model, int? page)
        {
            LibraryContext context = new LibraryContext();

            var queryResult = context.Genres
                .Include(x => x.BookGenres)
                .AsQueryable();

            if (!string.IsNullOrEmpty(model.Name))
            {
                queryResult = queryResult.Where(x => x.Name.Contains(model.Name));
            }

            int pageNumber = page ?? 1;
            int numberOfItemsPerPage = 10;
            model.Total = queryResult.Count();

            model.Genres = queryResult.Select(x => new GenreListItem()
            {
                Id = x.GenreId,
                Name = x.Name
            }).OrderByDescending(x => x.Id).ToPagedList(pageNumber, numberOfItemsPerPage);


            return View(model);
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
