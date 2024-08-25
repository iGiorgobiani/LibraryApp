using BusinessLogic.IServices;
using DataAccess.EF;
using Model.Genre;


namespace BusinessLogic.Services
{
    public class GenreService : IGenreService
    {
        public GenreShowModel GenreShow(GenreShowModel model, int? page)
        {
            try
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


                return model;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
