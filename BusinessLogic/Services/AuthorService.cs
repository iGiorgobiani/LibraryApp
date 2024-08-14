
using BusinessLogic.IServices;
using Model.Author;
using DataAccess.EF;


namespace BusinessLogic.Services;

public class AuthorService : IAuthorService
{
    private readonly string _imageDirectory = @"C:\\Users\\giorg\\source\\repos\\LibraryApp\\LibraryApp\\wwwroot\\Images\";

    public AuthorsViewModel GetAuthors(AuthorsViewModel model, int? page)
    {
        try
        {
            LibraryContext context = new LibraryContext();

            var queryResult = context.Authors
                .Include(x => x.BookAuthors)
                .AsQueryable();


            if (!string.IsNullOrEmpty(model.Firstname))
            {
                queryResult = queryResult.Where(x => x.Firstname.Contains(model.Firstname));
            }

            if (!string.IsNullOrEmpty(model.Lastname))
            {
                queryResult = queryResult.Where(x => x.Lastname.Contains(model.Lastname));
            }

            int pageNumber = page ?? 1;
            int numberOfItemsPerPage = 10;
            model.Total = queryResult.Count();

            model.Authors = queryResult.Select(x => new AuthorListItem()
            {
                AuthorId = x.AuthorId,
                Firstname = x.Firstname,
                Lastname = x.Lastname,
                Birthdate = x.Birthdate,
                Booknumber = x.BookAuthors.Count,
                ImageArray = x.ImagePath != null ? System.IO.File.ReadAllBytes(_imageDirectory + x.ImagePath) : System.IO.File.ReadAllBytes(_imageDirectory + "avatar.png")
            }).OrderByDescending(x => x.AuthorId).ToPagedList(pageNumber, numberOfItemsPerPage);

            return model;
        }
        catch (Exception ex) 
        {
            return null;
        }
    }
}
