
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

    public AddAuthorModel AddAuthor(AddAuthorModel model)
    {
        try
        {
            LibraryContext context = new LibraryContext();

            var author = new Author

            {
                Firstname = model.Firstname,
                Lastname = model.Lastname,
                Birthdate = model.Birthdate,
            };

            if (model.Cv != null && model.Cv.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    model.Cv.CopyTo(ms);
                    var file = ms.ToArray();
                    author.Cv = file;
                    author.CvToken = Guid.NewGuid().ToString();
                }

            }

            if (model.Image != null && model.Image.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    var fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(model.Image.FileName);
                    var filePath = _imageDirectory + fileName;
                    model.Image.CopyTo(ms);
                    System.IO.File.WriteAllBytes(filePath, ms.ToArray());
                    author.ImagePath = fileName;
                }
            }

            context.Authors.Add(author);

            context.SaveChanges();

            return model;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public EditAuthorModel EditAuthor(int? authorId)
    {
        try
        { 
        LibraryContext context = new LibraryContext();

        if (context.Authors.Any(x => x.AuthorId == authorId))
        {
            var author = context.Authors.SingleOrDefault(x => x.AuthorId == authorId);

            var model = new EditAuthorModel()
            {
                AuthorId = author.AuthorId,
                Firstname = author.Firstname,
                Lastname = author.Lastname,
                Birthdate = author.Birthdate,
                //Birthdate = string.Format("{0: mm.DD.yyyy}" , author.Birthdate)
                HasCv = author.Cv != null,
                CvToken = author.CvToken,
                HasImage = author.ImagePath != null,
                ImageArray = author.ImagePath != null ? System.IO.File.ReadAllBytes(_imageDirectory + author.ImagePath) : null

            };

                return model;
        }  
        else
        {
            Console.WriteLine("No authorId found");
            return null; // or return a default EditAuthorModel
        }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public EditAuthorModel EditAuthor(EditAuthorModel model)
    {
        LibraryContext context = new LibraryContext();
        var author = context.Authors.SingleOrDefault(x => x.AuthorId == model.AuthorId);

        author.Firstname = model.Firstname;
        author.Lastname = model.Lastname;
        author.Birthdate = model.Birthdate;

        if (model.Cv != null && model.Cv.Length > 0)
        {
            using (var ms = new MemoryStream())
            {
                model.Cv.CopyTo(ms);
                var file = ms.ToArray();
                author.Cv = file;
                author.CvToken = Guid.NewGuid().ToString();
            }

        }

        if (model.Image != null && model.Image.Length > 0)
        {
            using (var ms = new MemoryStream())
            {

                var fileName = author.ImagePath != null ? author.ImagePath : Guid.NewGuid().ToString() + System.IO.Path.GetExtension(model.Image.FileName);
                var filePath = _imageDirectory + fileName;
                model.Image.CopyTo(ms);
                System.IO.File.WriteAllBytes(filePath, ms.ToArray());
                author.ImagePath = fileName;
                //author.ImagePath = fileName;
            }
        }

        context.SaveChanges();
        return model;
    }

    public void RemoveAuthor(int authorId)
    {

        LibraryContext context = new LibraryContext();

        var author = context.Authors
            .Include(x => x.BookAuthors)
            .SingleOrDefault(x => x.AuthorId == authorId);
        context.BookAuthors.RemoveRange(author.BookAuthors);
        context.Authors.Remove(author);
        context.SaveChanges();

        return;
    }

}




    //[HttpPost]
    //[Authorize(Roles = "Admin, Editor")]
    //public IActionResult EditAuthor(EditAuthorModel model)
    //{
    //    //if (ModelState.IsValid)
    //    {
    //        LibraryContext context = new LibraryContext();

//        var author = context.Authors.SingleOrDefault(x => x.AuthorId == model.AuthorId);

//        author.Firstname = model.Firstname;
//        author.Lastname = model.Lastname;
//        author.Birthdate = model.Birthdate;

//        if (model.Cv != null && model.Cv.Length > 0)
//        {
//            using (var ms = new MemoryStream())
//            {
//                model.Cv.CopyTo(ms);
//                var file = ms.ToArray();
//                author.Cv = file;
//                author.CvToken = Guid.NewGuid().ToString();
//            }

//        }

//        if (model.Image != null && model.Image.Length > 0)
//        {
//            using (var ms = new MemoryStream())
//            {

//                var fileName = author.ImagePath != null ? author.ImagePath : Guid.NewGuid().ToString() + System.IO.Path.GetExtension(model.Image.FileName);
//                var filePath = _imageDirectory + fileName;
//                model.Image.CopyTo(ms);
//                System.IO.File.WriteAllBytes(filePath, ms.ToArray());
//                author.ImagePath = fileName;
//                //author.ImagePath = fileName;
//            }
//        }

//        context.SaveChanges();
//        TempData["Success"] = "ავტორის რედაქტირება განხორციელდა წარმატებით";
//        return RedirectToAction("EditAuthor", new { authorId = author.AuthorId });
//    }
//    TempData["Error"] = "ავტორის რედაქტირება ვერ განხორციელდა";
//    return View(model);
//}


