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

        public IActionResult AddAuthor(AddAuthorModel model)
        {
            var resultModel = _authorService.AddAuthor(model);

            return View(resultModel);
        }

        public IActionResult EditAuthor(EditAuthorModel model, int? authorId)
        {
            var resultModel = _authorService.EditAuthor(authorId);

            return View(resultModel);
        }

        [HttpPost]
        public IActionResult EditAuthor(EditAuthorModel model)
        {
            var resultModel = _authorService.EditAuthor(model);

            return View(resultModel);
        }

        public IActionResult RemoveAuthor(int authorId)
        {
            _authorService.RemoveAuthor(authorId);
            return RedirectToAction("Authors");
        }


        [Authorize(Roles = "Admin, Editor")]
        public IActionResult ViewFile(int authorId, string cvToken)
        {

            LibraryContext context = new LibraryContext();

            var authorExists = context.Authors.Any(x => x.AuthorId == authorId);

            if (string.IsNullOrEmpty(cvToken) || !authorExists)
            {
                return NotFound();
            }

            var author = context.Authors.SingleOrDefault(x => x.AuthorId == authorId);

            if (author.Cv == null)
            {
                return NotFound();
            }


            return File(author.Cv, "application/pdf");

        }

        [Authorize(Roles = "Admin, Editor")]
        public IActionResult DownloadFile(int authorId, string cvToken)
        {

            LibraryContext context = new LibraryContext();

            var authorExists = context.Authors.Any(x => x.AuthorId == authorId);

            if (string.IsNullOrEmpty(cvToken) || !authorExists)
            {
                return NotFound();
            }

            var author = context.Authors.SingleOrDefault(x => x.AuthorId == authorId);

            if (author.Cv == null)
            {
                return NotFound();
            }

            var fileName = $"{author.Firstname} {author.Lastname} - CV.pdf";
            return File(author.Cv, "application/pdf", fileName);

        }
    }


}
