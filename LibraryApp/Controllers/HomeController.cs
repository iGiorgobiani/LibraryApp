﻿using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Models.Book;
using LibraryApp.Models.Author;
using X.PagedList;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http.Extensions;
using System.Net;
using System.Web;
using LibraryApp.Database;
using LibraryApp.Models.Book;

namespace LibraryApp.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index(BookViewModel model, int? page)
		{
			LibraryContext context = new LibraryContext();

			var queryResult = context.Books
				//.Include(x => x.Genre)
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
				//Genre = x.Genre.Name,
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


			return View();
		}

		[HttpPost]
		public IActionResult AddBook(AddBookModel model)
		{

			if (ModelState.IsValid)
			{
				LibraryContext context = new LibraryContext();

				var queryResult = context.Books
				//.Include(x => x.Genre)
				.Include(x => x.BookAuthors)
				.ThenInclude(x => x.Author)
				.AsQueryable();


				context.Books.Add(new Book()
				{
					Name = model.Name,
					Year = model.Year,
					//Genre = model.Genre
				});
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

				ViewBag.GenreSelectList = new SelectList(context.Genres, "GenreId", "Name");
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
			ViewBag.GenreSelectList = new SelectList(context.Genres, "GenreId", "Name");


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
				.SingleOrDefault(x => x.BookId == bookId);
			//context.BookAuthors.RemoveRange(book.BookAuthors);
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

