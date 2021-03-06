using PostSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using PostSys.ViewModels;

namespace PostSys.Controllers
{
	public class CoursesController : Controller
	{
		private ApplicationDbContext _context;

		public CoursesController()
		{
			_context = new ApplicationDbContext();
		}
		// GET: Courses
		public ActionResult ListCourses()
		{
			var getClass = _context.Classes.Include(c => c.Coordinator).ToList();
			var getStudent = _context.Users.ToList();

			var getCourse = _context.Courses.Include(c => c.Class).Include(s => s.Student).ToList();

			return View(getCourse);
		}

		public ActionResult CreateCourse()
		{
			var listClass = _context.Classes.Include(c => c.Coordinator).ToList();

			var role = (from r in _context.Roles where r.Name.Contains("Marketing Coordinator") select r)
															 .FirstOrDefault();
			var listStudent = _context.Users.Where(x => x.Roles.Select(y => y.RoleId)
									  .Contains(role.Id))
									  .ToList();

			var dropDownListStudentClass = new StudentClassViewModel
			{
				Classes = listClass,
				Students = listStudent
			};

			return View(dropDownListStudentClass);
		}

		[HttpPost]
		public ActionResult CreateCourse(Course course)
		{
			var newCourse = new Course
			{
				Name = course.Name,
				ClassId = course.ClassId,
				StudentId = course.StudentId
			};
			_context.Courses.Add(newCourse);
			_context.SaveChanges();

			return RedirectToAction("ListCourses");
		}


		public ActionResult DeleteCourse(int id)
		{
			var courseInDb = _context.Courses.SingleOrDefault(i => i.Id == id);

			_context.Courses.Remove(courseInDb);
			_context.SaveChanges();

			return RedirectToAction("ListCourses");
		}
	}
}