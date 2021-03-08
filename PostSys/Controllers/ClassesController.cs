using PostSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using PostSys.ViewModels;
using Microsoft.AspNet.Identity;

namespace PostSys.Controllers
{
	public class ClassesController : Controller
	{
		private ApplicationDbContext _context;

		public ClassesController()
		{
			_context = new ApplicationDbContext();
		}

		// GET: Classes
		public ActionResult ListClasses()
		{
			var getFaculies = _context.Faculties.ToList();
			var getCoordinator = _context.Users.ToList();

			var getClasses = _context.Classes.Include(f => f.Faculty).Include(c => c.Coordinator).ToList();

			return View(getClasses);
		}

		[HttpGet]
		public ActionResult CreateClass()
		{
			var role = (from r in _context.Roles where r.Name.Contains("Marketing Coordinator") select r)
															 .FirstOrDefault();
			var listCoordinator = _context.Users.Where(x => x.Roles.Select(y => y.RoleId)
									  .Contains(role.Id))
									  .ToList();

			var listFaculty = _context.Faculties.ToList();

			var dropDownListCoordinatorFaculty = new CoordinatorFacultyViewModel
			{
				Faculties = listFaculty,
				Coordinators = listCoordinator,
			};

			return View(dropDownListCoordinatorFaculty);
		}

		[HttpPost]
		public ActionResult CreateClass(Class @class)
		{
			var newClass = new Class
			{
				Name = @class.Name,
				FacultyId = @class.FacultyId,
				CoordinatorId = @class.CoordinatorId
			};

			_context.Classes.Add(newClass);
			_context.SaveChanges();
			
			return RedirectToAction("ListClasses");
		}

		[HttpGet]
		public ActionResult DeleteClass(int id)
		{
			var classInDb = _context.Classes.SingleOrDefault(i => i.Id == id);

			_context.Classes.Remove(classInDb);
			_context.SaveChanges();

			return RedirectToAction("ListClasses");
		}

		/*[HttpGet]
		public ActionResult MyClass()
		{
			var getcurrentCoordinatorId = User.Identity.GetUserId();
			var getMyCourse = _context.Classes.Where(c => c.CoordinatorId == getcurrentCoordinatorId)
											  .Include(c => c.Coordinator)
											  .Include(f => f.Faculty);

			return View(getMyCourse);
		}*/
	}
}