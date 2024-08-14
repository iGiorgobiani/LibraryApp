using BusinessLogic.IServices;
using DataAccess.EF;
using Model.Author;


namespace LibraryApp.Controllers
{
    [Authorize]
    public class AuthorController : Controller
    {
        private readonly IAuthorService _authorService;
        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        public IActionResult Authors(AuthorsViewModel model, int? page)
        {
            var resultModel = _authorService.GetAuthors(model, page);

            return View(resultModel);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AddAuthor()
        {

            return View();
        }

        //[HttpPost]
        //[Authorize(Roles = "Admin")]
        //public IActionResult AddAuthor(AddAuthorModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        LibraryContext context = new LibraryContext();

        //        var author = new Author

        //        {
        //            Firstname = model.Firstname,
        //            Lastname = model.Lastname,
        //            Birthdate = model.Birthdate,
        //        };

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
        //                var fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(model.Image.FileName);
        //                var filePath = _imageDirectory + fileName;
        //                model.Image.CopyTo(ms);
        //                System.IO.File.WriteAllBytes(filePath, ms.ToArray());
        //                author.ImagePath = fileName;
        //            }
        //        }

        //        context.Authors.Add(author);

        //        context.SaveChanges();
        //        TempData["Success"] = "ავტორი დაემატა წარმატებით";
        //        //return RedirectToAction("Authors");
        //        return RedirectToAction("EditAuthor", new { authorId = author.AuthorId });
        //    }
        //    TempData["Error"] = "ავტორის დამატება ვერ მოხერხდა";
        //    return View(model);
        //}
        ////
        //[Authorize(Roles = "Admin, Editor")]
        //public IActionResult EditAuthor(int authorId)
        //{
        //    LibraryContext context = new LibraryContext();
        //    if (context.Authors.Any(x => x.AuthorId == authorId))
        //    {
        //        var author = context.Authors.SingleOrDefault(x => x.AuthorId == authorId);

        //        var model = new EditAuthorModel()
        //        {
        //            AuthorId = author.AuthorId,
        //            Firstname = author.Firstname,
        //            Lastname = author.Lastname,
        //            Birthdate = author.Birthdate,
        //            //Birthdate = string.Format("{0: mm.DD.yyyy}" , author.Birthdate)
        //            HasCv = author.Cv != null,
        //            CvToken = author.CvToken,
        //            HasImage = author.ImagePath != null,
        //            ImageArray = author.ImagePath != null ? System.IO.File.ReadAllBytes(_imageDirectory + author.ImagePath) : null

        //        };
        //        return View(model);
        //    }
        //    return RedirectToAction("Authors");

        //}
        //[HttpPost]
        //[Authorize(Roles = "Admin, Editor")]
        //public IActionResult EditAuthor(EditAuthorModel model)
        //{
        //    if (ModelState.IsValid)
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

        //[Authorize(Roles = "Admin")]
        //public IActionResult RemoveAuthor(int authorId)
        //{

        //    LibraryContext context = new LibraryContext();

        //    var author = context.Authors
        //        .Include(x => x.BookAuthors)
        //        .SingleOrDefault(x => x.AuthorId == authorId);
        //    context.BookAuthors.RemoveRange(author.BookAuthors);
        //    context.Authors.Remove(author);
        //    context.SaveChanges();

        //    return RedirectToAction("Authors");
        //}

        //[Authorize(Roles = "Admin, Editor")]
        //public IActionResult ViewFile(int authorId, string cvToken)
        //{

        //    LibraryContext context = new LibraryContext();

        //    var authorExists = context.Authors.Any(x => x.AuthorId == authorId);

        //    if (string.IsNullOrEmpty(cvToken) || !authorExists)
        //    {
        //        return NotFound();
        //    }

        //    var author = context.Authors.SingleOrDefault(x => x.AuthorId == authorId);

        //    if (author.Cv == null)
        //    {
        //        return NotFound();
        //    }


        //    return File(author.Cv, "application/pdf");

        //}

        //[Authorize(Roles = "Admin, Editor")]
        //public IActionResult DownloadFile(int authorId, string cvToken)
        //{

        //    LibraryContext context = new LibraryContext();

        //    var authorExists = context.Authors.Any(x => x.AuthorId == authorId);

        //    if (string.IsNullOrEmpty(cvToken) || !authorExists)
        //    {
        //        return NotFound();
        //    }

        //    var author = context.Authors.SingleOrDefault(x => x.AuthorId == authorId);

        //    if (author.Cv == null)
        //    {
        //        return NotFound();
        //    }

        //    var fileName = $"{author.Firstname} {author.Lastname} - CV.pdf";
        //    return File(author.Cv, "application/pdf", fileName);

        //}
    }
}
