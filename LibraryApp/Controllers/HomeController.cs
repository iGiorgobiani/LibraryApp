using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Models.Book;
using LibraryApp.Models.Author;
using X.PagedList;
using Microsoft.AspNetCore.Mvc.Rendering;
using LibraryApp.Database;
using System.Net;

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

		private void InitializeAddBook(LibraryContext context)
		{
			var authorSelectList = context.Authors.Select(a => new System.Web.Mvc.SelectListItem
			{
				Value = a.AuthorId.ToString(),
				Text = a.Firstname + " " + a.Lastname
			});

			var genreSelectList = context.Genres.Select(g => new System.Web.Mvc.SelectListItem
			{
				Value = g.GenreId.ToString(),
				Text = g.Name
			});

			ViewBag.GenreSelectList = genreSelectList;
            ViewBag.AuthorSelectList = authorSelectList;

        }

        public IActionResult AddBook()
		{
			LibraryContext context = new LibraryContext();	

			InitializeAddBook(context);

            return View();
		}


		[HttpPost]
		public IActionResult AddBook(AddBookModel model)
		{
			LibraryContext context = new LibraryContext();

			InitializeAddBook(context);

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

				TempData["Success"] = "Book has been added successfully";

				return RedirectToAction("EditBook", new { bookId = book.BookId});
			}

            TempData["Error"] = "Book couldn't be added";

            return View(model);
		}

		//
		public IActionResult EditBook(int bookId)
		{
			LibraryContext context = new LibraryContext();

			if (context.Books.Any(x => x.BookId == bookId))
			{
				var book = context.Books
					.Include(x => x.BookGenres) //სანამ მივხვდებოდი რომ ეს უნდა მექნა 1 სთ დავხარჯე
					.ThenInclude(x => x.Genre)
					.Include(x => x.BookAuthors)
					.ThenInclude(x => x.Author)
					.SingleOrDefault(x => x.BookId == bookId);

				var model = new EditBookModel()
				{
					BookId = book.BookId,
					Name = book.Name,
					Year = book.Year,
					Pages = book.Pages,
					Genres = book.BookGenres.Select(g => new BookGenreModel()
					{
						Name = g.Genre.Name,
						BookId = bookId,
						GenreId = g.Genre.GenreId
					}).ToList(),
					SelectedGenreIds = new List<int>(), // ეს 2 უნდა გავაერთიანო, რომ უკვე მონიშნულად მოქონდეს ისინი, რაც არჩეულია და პირდაპირ იქიდან იშლებოდეს ამდენი მაიმუნობის გარეშე
					SelectedRemoveIds = new List<int>(),
					Authors = book.BookAuthors.Select(a => new BookAuthorEditModel()
					{
						Lastname = a.Author.Lastname,
						AuthorId = a.Author.AuthorId,
						BookId = bookId
					}).ToList(),
					SelectedAuthorIds = new List<int>(),
					SelectedRemoveAuthorIds = new List<int>()
				};

				var alreadyGenres = context.BookGenres
					.Include(x => x.Genre)
					.Where(x => x.BookId == bookId)
					.ToList(); // Get a list of BookGenre objects

				if (alreadyGenres != null)
				{
					foreach (var genre in alreadyGenres)
					{
						model.SelectedGenreIds.Add((int)genre.GenreId);
						model.SelectedRemoveIds.Add((int)genre.GenreId);
					};
				}

                ViewBag.GenreSelectList = new SelectList(context.Genres, "GenreId", "Name");
                ViewBag.SelectedGenreIds = model.SelectedGenreIds; // Assign SelectedGenreIds to ViewBag
				ViewBag.SelectedRemoveIds = model.SelectedRemoveIds;

				var alreadyAuthors = context.BookAuthors
					.Include(x => x.Author)
					.Where(x => x.BookId == bookId)
					.ToList(); // Get a list of BookAuthor objects

				if (alreadyAuthors != null)
				{
					foreach (var author in alreadyAuthors)
					{
						model.SelectedAuthorIds.Add((int)author.AuthorId);
						model.SelectedRemoveAuthorIds.Add((int)author.AuthorId);
					};
				}

				//Author Viewbags
				ViewBag.AuthorSelectList = new SelectList(context.Authors, "AuthorId", "Lastname");
				ViewBag.SelectedAuthorIds = model.SelectedAuthorIds; // Assign SelectedAuthorIds to ViewBag
				ViewBag.SelectedRemoveAuthorIds = model.SelectedRemoveAuthorIds;

				return View(model);

			}
			return RedirectToAction("Index");

		}


        [HttpPost]
		public IActionResult EditBook(EditBookModel model)
		{
			LibraryContext context = new LibraryContext();


			if (ModelState.IsValid)
			{
				var book = context.Books.SingleOrDefault(x => x.BookId == model.BookId);

				book.Name = model.Name;
				book.Year = model.Year;
				book.Pages = model.Pages;

				if (model.SelectedGenreIds != null) //რომ ერორზე არ გადიოდეს თუ არ მონიშნავ
				{
					if (model.SelectedGenreIds != model.SelectedRemoveIds && model.SelectedRemoveIds != null)
					{
						foreach (var genreId in model.SelectedRemoveIds)
						{
							var bookGenre = context.BookGenres
								.SingleOrDefault(g => g.BookId == model.BookId && g.GenreId == genreId);
							context.BookGenres.Remove(bookGenre);
						}
					}
					foreach (var genreId in model.SelectedGenreIds)
					{
						var bookGenre = new BookGenre
						{
							BookId = book.BookId,
							GenreId = genreId
						};
						book.BookGenres.Add(bookGenre);
					}
				}
				if (model.SelectedRemoveIds != null) //რომ ერორზე არ გადიოდეს თუ არ მონიშნა
				{
					foreach (var genreId in model.SelectedRemoveIds)
					{
						var bookGenre = context.BookGenres
							.SingleOrDefault(g => g.BookId == model.BookId && g.GenreId == genreId);
						context.BookGenres.Remove(bookGenre);
					}
				}

				if (model.SelectedAuthorIds != null) //რომ ერორზე არ გადიოდეს თუ არ მონიშნავ
				{
					if (model.SelectedAuthorIds != model.SelectedRemoveAuthorIds && model.SelectedRemoveAuthorIds != null)
						foreach (var authorId in model.SelectedRemoveAuthorIds) //შლის ყველა ძველ ჩანაწერს
						{
							var bookAuthor = context.BookAuthors
								.SingleOrDefault(g => g.BookId == model.BookId && g.AuthorId == authorId);
							context.BookAuthors.Remove(bookAuthor);
						}

					foreach (var authorId in model.SelectedAuthorIds) //ქმნის ახალ ჩანაწერებს
					{
						var bookAuthor = new BookAuthor
						{
							BookId = book.BookId,
							AuthorId = authorId
						};
						book.BookAuthors.Add(bookAuthor);
					}
				}
				
				if (model.SelectedRemoveAuthorIds != null) //რომ ერორზე არ გადიოდეს თუ არ მონიშნა
				{
					foreach (var authorId in model.SelectedRemoveAuthorIds)
					{
						var bookAuthor = context.BookAuthors
							.SingleOrDefault(g => g.BookId == model.BookId && g.AuthorId == authorId);
						context.BookAuthors.Remove(bookAuthor);
					}
				}

				context.SaveChanges();

				return RedirectToAction("Index");
			}
			return View(model);
		}


		public IActionResult RemoveBook(int bookId, string returnUrl)
		{	
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

            return Redirect(returnUrl);
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

