using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Controllers
{
	public class GenreBook : Controller
	{
		// GET: GenreBook
		public ActionResult Index()
		{
			return View();
		}

		// GET: GenreBook/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: GenreBook/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: GenreBook/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: GenreBook/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: GenreBook/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: GenreBook/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: GenreBook/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}
	}
}
