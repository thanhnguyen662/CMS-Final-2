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
		[Authorize(Roles = "Marketing Manager")]
		public ActionResult ListClasses()
		{
			var getFaculies = _context.Faculties.ToList();
			var getCoordinator = _context.Users.ToList();

			var getClasses = _context.Classes.Include(f => f.Faculty).Include(c => c.Coordinator).ToList();

			return View(getClasses);
		}

		[Authorize(Roles = "Marketing Manager")]
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

		[Authorize(Roles = "Marketing Manager")]
		[HttpPost]
		public ActionResult CreateClass(Class @class)
		{
			var newClass = new Class
			{
				Name = @class.Name,
				FacultyId = @class.FacultyId,
				CoordinatorId = @class.CoordinatorId,
				EnrollmentKey = @class.EnrollmentKey
			};

			_context.Classes.Add(newClass);
			_context.SaveChanges();
			
			return RedirectToAction("ListClasses");
		}

		[Authorize(Roles = "Marketing Manager")]
		[HttpGet]
		public ActionResult DeleteClass(int id)
		{
			var classInDb = _context.Classes.SingleOrDefault(i => i.Id == id);

			_context.Classes.Remove(classInDb);
			_context.SaveChanges();

			return RedirectToAction("ListClasses");
		}


		[Authorize(Roles = "Marketing Manager")]
		public ActionResult AssignClass()
		{
			return View();
		}

		[Authorize(Roles = "Marketing Manager")]
		[HttpPost]
		public ActionResult AssignClass(Course course, int id)
		{
			var classId = _context.Classes.SingleOrDefault(i => i.Id == id);
			var currentStudent = User.Identity.GetUserId();

			//get EnrollmentKey from ClassId
			var classIds = _context.Classes.Where(m => m.Id == id).Select(m => m.EnrollmentKey).ToList();
			var enrollKey = classIds[0];
			var enrollKeyLength = enrollKey.Length;

			//get FacultyName from ClassId
			var facultyNames = _context.Classes.Where(m => m.Id == id)
											   .Include(f => f.Faculty)
											   .Select(m => m.Faculty.Name)
											   .ToList();
			var facultyName = facultyNames[0];									   


			var newAssign = new Course
			{
				Name = facultyName + "_" + "Course",
				ClassId = classId.Id,
				StudentId = currentStudent,
				EnrollmentKey = course.EnrollmentKey
			};

			if (newAssign.EnrollmentKey.Contains(enrollKey) && newAssign.EnrollmentKey.Length == enrollKeyLength)
			{
				_context.Courses.Add(newAssign);
				_context.SaveChanges();

				return RedirectToAction("ListClasses");
			}

			return View("~/Views/ErrorValidations/Null.cshtml");
		}
	}
}