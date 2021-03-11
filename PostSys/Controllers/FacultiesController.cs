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
		[ValidateAntiForgeryToken]
		public ActionResult CreateFaculty(Faculty faculty)
		{
			var isExistFaculty = _context.Faculties.Any(c => c.Name == faculty.Name);
			if(isExistFaculty == true)
			{
				return View("~/Views/ErrorValidations/Exist.cshtml");
			}

			var newFaculty = new Faculty
			{
				Name = faculty.Name
			};

			if(newFaculty.Name == null)
			{
				return View("~/Views/ErrorValidations/Null.cshtml");
			}

			_context.Faculties.Add(newFaculty);
			_context.SaveChanges();

			return RedirectToAction("ListFaculty");
		}

		[Authorize(Roles = "Marketing Manager")]
		[HttpGet]
		public ActionResult DeleteFaculty(int id)
		{
			var facultyInDb = _context.Faculties.SingleOrDefault(f => f.Id == id);
			
			if(facultyInDb == null)
			{
				return View("~/Views/ErrorValidations/Null.cshtml");
			}

			_context.Faculties.Remove(facultyInDb);
			_context.SaveChanges();

			return RedirectToAction("ListFaculty");
		}
	}
}