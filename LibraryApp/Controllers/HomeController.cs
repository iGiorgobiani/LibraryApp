using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Models.Book;
using LibraryApp.Models.Author;
using X.PagedList;
using Microsoft.AspNetCore.Mvc.Rendering;
using LibraryApp.Database;

namespace LibraryApp.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index(BookViewModel model, int? page)
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
			}).ToPagedList(pageNumber, numberOfItemsPerPage);

			return View(model);
		}

		public IActionResult AddBook()
		{
			LibraryContext context = new LibraryContext();

			var model = new AddBookModel
			{
				GenreSelectList = context.Genres.Select(g => new System.Web.Mvc.SelectListItem
				{
					Value = g.GenreId.ToString(),
					Text = g.Name
				}).ToList(),
				SelectedGenreIds = new List<int>(), // Initializing SelectedGenreIds to an empty list

				AuthorSelectList = context.Authors.Select(a => new System.Web.Mvc.SelectListItem
				{
					Value = a.AuthorId.ToString(),
					Text = a.Firstname
				})
			};

			ViewBag.GenreSelectList = new SelectList(context.Genres, "GenreId", "Name");
			ViewBag.SelectedGenreIds = model.SelectedGenreIds; // Assign SelectedGenreIds to ViewBag
			ViewBag.AuthorSelectList = new SelectList(context.Authors, "AuthorId", "Firstname");
            ViewBag.SelectedAuthorList = model.SelectedAuthorIds; // Assign SelectedGenreIds to ViewBag

            return View();
		}


		[HttpPost]
		public IActionResult AddBook(AddBookModel model)
		{
			LibraryContext context = new LibraryContext();
			ViewBag.GenreSelectList = new SelectList(context.Genres, "GenreId", "Name");
			ViewBag.SelectedGenreIds = model.SelectedGenreIds;
            ViewBag.AuthorSelectList = new SelectList(context.Authors, "AuthorId", "Firstname");
            ViewBag.SelectedAuthorList = model.SelectedAuthorIds; // Assign SelectedGenreIds to ViewBag

            if (ModelState.IsValid)
			{
				var queryResult = context.Books
				.Include(x => x.BookGenres)
				.Include(x => x.BookAuthors)
				.ThenInclude(x => x.Author)
				.AsQueryable();


				var book = new Book
				{
					Name = model.Name,
					Year = model.Year,
					BookGenres = new List<BookGenre>()
				};

				foreach (var genreId in model.SelectedGenreIds)
				{
					var bookGenre = new BookGenre
					{
						BookId = book.BookId,
						GenreId = genreId
					};
					book.BookGenres.Add(bookGenre);
				}

				foreach (var authorId in model.SelectedAuthorIds)
				{
					var bookAuthor = new BookAuthor
					{
						BookId = book.BookId,
						AuthorId = authorId
					};
					book.BookAuthors.Add(bookAuthor);
				}
				context.Books.Add(book);
				context.SaveChanges();

				return RedirectToAction("Index");
			}

			return View(model);
		}

		//
		public IActionResult EditBook(int bookId)
		{
			LibraryContext context = new LibraryContext();
			if (context.Books.Any(x => x.BookId == bookId))
			{
				var book = context.Books.SingleOrDefault(x => x.BookId == bookId);

				//ViewBag.GenreSelectList = new SelectList(context.Genres, "GenreId", "Name");
				//	ViewBag.GenreSelectList = context.Genres
				//.Select(g => new SelectListItem
				//{
				//	Value = g.GenreId.ToString(),
				//	Text = g.Name
				//})
				//.ToList();

				var model = new EditBookModel()
				{
					BookId = book.BookId,
					Name = book.Name,
					Year = book.Year,
					Pages = book.Pages,
				//	GenreId = book.GenreId
				};
				return View(model);

			}
			return RedirectToAction("Index");

		}


		[HttpPost]
		public IActionResult EditBook(EditBookModel model)
		{
			LibraryContext context = new LibraryContext();
			//ViewBag.GenreSelectList = new SelectList(context.Genres, "GenreId", "Name");


			if (ModelState.IsValid)
			{


				var book = context.Books.SingleOrDefault(x => x.BookId == model.BookId);

				book.Name = model.Name;
				book.Year = model.Year;
				book.Pages = model.Pages;
				//book.GenreId = model.GenreId;


				context.SaveChanges();

				return RedirectToAction("Index");
			}
			return View(model);
		}


		public IActionResult RemoveBook(int bookId)
		{

			var pageOne = HttpContext.Request.Protocol; //ამას გამოაქვს HTTP/ + გვერდი
			var pageOneUpdated = pageOne.Substring(5); //ვშლი HTTP/-ს

			LibraryContext context = new LibraryContext();


			var book = context.Books
			   .Include(x => x.BookAuthors)
			   .Include(x => x.BookGenres)
				.SingleOrDefault(x => x.BookId == bookId);
			//context.BookAuthors.RemoveRange(book.BookAuthors);
			context.BookGenres.RemoveRange(book.BookGenres);
			context.Books.Remove(book);
			context.Books.Remove(book);
			_ = context.SaveChanges();


			//var page = (context.Books.Count() +9) / 10;

			return RedirectToAction("Index", "Home", new { page = pageOneUpdated });
		}

		public IActionResult Authors(AuthorsViewModel model, int? page)
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
				Booknumber = x.BookAuthors.Count
			}).ToPagedList(pageNumber, numberOfItemsPerPage);


			return View(model);
		}

		public IActionResult AddAuthor()
		{

			return View();
		}

		[HttpPost]
		public IActionResult AddAuthor(AddAuthorModel model)
		{
			if (ModelState.IsValid)
			{
				LibraryContext context = new LibraryContext();

				context.Authors.Add(new Author()
				{
					Firstname = model.Firstname,
					Lastname = model.Lastname,
					Birthdate = model.Birthdate,
				});
				context.SaveChanges();

				return RedirectToAction("Authors");
			}
			return View(model);
		}
		//
		public IActionResult EditAuthor(int authorId)
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
					//Birthdate = string.Format("{0: mm.DD.yyyy}" , author.Birthdate)


				};
				return View(model);
			}
			return RedirectToAction("Authors");

		}
		[HttpPost]
		public IActionResult EditAuthor(EditAuthorModel model)
		{
			if (ModelState.IsValid)
			{
				LibraryContext context = new LibraryContext();

				var author = context.Authors.SingleOrDefault(x => x.AuthorId == model.AuthorId);

				author.Firstname = model.Firstname;
				author.Lastname = model.Lastname;
				//author.Birthdate = model.Birthdate;

				context.SaveChanges();

				return RedirectToAction("Authors");
			}
			return View(model);
		}

		public IActionResult RemoveAuthor(int authorId)
		{

			LibraryContext context = new LibraryContext();

			var author = context.Authors
				.Include(x => x.BookAuthors)
				.SingleOrDefault(x => x.AuthorId == authorId);
			context.BookAuthors.RemoveRange(author.BookAuthors);
			context.Authors.Remove(author);
			context.SaveChanges();

			return RedirectToAction("Authors");
		}





	}
}

