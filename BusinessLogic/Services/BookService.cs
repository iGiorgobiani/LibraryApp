using BusinessLogic.IServices;
using DataAccess.EF;
using Model.Author;
using Model.Book;



namespace BusinessLogic.Services
{
    public class BookService : IBookService
    {
        public BookViewModel GetBooks(BookViewModel model, int? page)
        {
            try
            {
                LibraryContext context = new LibraryContext();

                var queryResult = context.Books
                    .Include(x => x.BookGenres)
                    .ThenInclude(x => x.Genre)
                    .Include(x => x.BookAuthors)
                    .ThenInclude(x => x.Author)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(model.Name))
                {
                    queryResult = queryResult.Where(x => x.Name.Contains(model.Name));
                }

                if (!string.IsNullOrEmpty(model.Firstname))
                {
                    queryResult = queryResult.Where(x => x.BookAuthors.Any(a => a.Author.Firstname.Contains(model.Firstname)));
                }

                if (!string.IsNullOrEmpty(model.Lastname))
                {
                    queryResult = queryResult.Where(x => x.BookAuthors.Any(a => a.Author.Lastname.Contains(model.Lastname)));
                }

                int pageNumber = page ?? 1;
                int numberOfItemsPerPage = 10;
                model.Total = queryResult.Count();

                model.Books = queryResult.Select(x => new BookListItem()
                {
                    BookId = x.BookId,
                    Name = x.Name,
                    Year = x.Year,
                    Genres = x.BookGenres.Select(g => new GenreViewModel()
                    {
                        Name = g.Genre.Name
                    }).ToList(),
                    Authors = x.BookAuthors.Select(a => new AuthorViewModel()
                    {
                        Firstname = a.Author.Firstname,
                        Lastname = a.Author.Lastname
                    }).ToList()
                }).OrderByDescending(x => x.Name).ToPagedList(pageNumber, numberOfItemsPerPage);

                return model;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}