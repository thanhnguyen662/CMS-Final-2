using PostSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PostSys.Controllers
{
	public class FacultiesController : Controller
	{
		private ApplicationDbContext _context;

		public FacultiesController()
		{
			_context = new ApplicationDbContext();
		}


		// GET: Faculties
		[Authorize(Roles = "Marketing Manager")]
		public ActionResult ListFaculty()
		{
			var getAllFaculties = _context.Faculties.ToList();

			return View(getAllFaculties);
		}

		[Authorize(Roles = "Marketing Manager")]
		[HttpGet]
		public ActionResult CreateFaculty()
		{
			return View();
		}

		[Authorize(Roles = "Marketing Manager")]
		[HttpPost]
		public ActionResult CreateFaculty(Faculty faculty)
		{
			var newFaculty = new Faculty
			{
				Name = faculty.Name
			};

			_context.Faculties.Add(newFaculty);
			_context.SaveChanges();

			return RedirectToAction("ListFaculty");
		}

		[Authorize(Roles = "Marketing Manager")]
		[HttpGet]
		public ActionResult DeleteFaculty(int id)
		{
			var facultyInDb = _context.Faculties.SingleOrDefault(f => f.Id == id);

			_context.Faculties.Remove(facultyInDb);
			_context.SaveChanges();

			return RedirectToAction("ListFaculty");
		}
	}
}