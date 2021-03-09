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
	public class CoursesController : Controller
	{
		private ApplicationDbContext _context;

		public CoursesController()
		{
			_context = new ApplicationDbContext();
		}
		// GET: Courses
		[Authorize(Roles = "Marketing Manager")]
		public ActionResult ListCourses()
		{
			var getClass = _context.Classes.Include(c => c.Coordinator).ToList();
			var getStudent = _context.Users.ToList();

			var getCourse = _context.Courses.Include(c => c.Class).Include(s => s.Student).ToList();

			return View(getCourse);
		}

		/*public ActionResult CreateCourse()
		{
			var listClass = _context.Classes.Include(c => c.Coordinator).ToList();

			var role = (from r in _context.Roles where r.Name.Contains("Student") select r)
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
			var getClass = _context.Classes.Include(f => f.Faculty).ToList();
			var getCurrentId = User.Identity.GetUserId();

			var currentClassList = _context.Classes.Where(m => m.CoordinatorId.Contains(getCurrentId))
												   .Select(m => m.Id)
												   .ToList();
			var classId = currentClassList[0]; 


			var newCourse = new Course
			{
				Name = course.Name,
				ClassId = classId,
				StudentId = course.StudentId
			};
			_context.Courses.Add(newCourse);
			_context.SaveChanges();

			return RedirectToAction("ListCourses");
		}*/

		[Authorize(Roles = "Marketing Coordinator")]
		public ActionResult DeleteCourse(int id)
		{
			var courseInDb = _context.Courses.SingleOrDefault(i => i.Id == id);

			_context.Courses.Remove(courseInDb);
			_context.SaveChanges();

			if (User.IsInRole("Marketing Coordinator"))
			{
				return RedirectToAction("ManageMyCourse");
			}

			return View();
		}


		[HttpGet]
		[Authorize(Roles = "Marketing Coordinator, Student")]
		//Coordinator
		public ActionResult ManageMyCourse()
		{
			var getCurrentCoordinatorUserName = User.Identity.GetUserName();
			var getUser = _context.Users.ToList();

			if (User.IsInRole("Marketing Coordinator"))
			{
				var getCourseOfCoordinator = _context.Courses.Where(c => c.Class.Coordinator.UserName == getCurrentCoordinatorUserName).Include(c => c.Class).ToList();

				return View(getCourseOfCoordinator);
			}

			if (User.IsInRole("Student"))
			{
				var getCourseOfCoordinator = _context.Courses.Where(c => c.Student.UserName == getCurrentCoordinatorUserName).Include(c => c.Class).ToList();

				return View(getCourseOfCoordinator);
			}

			return View();
		}


		/*//Coordinator
		public ActionResult CreateAssignment(int id)
		{
			var getCourseInDb = _context.Courses.SingleOrDefault(c => c.Id == id);

			var getCurrentUser = User.Identity.GetUserName();
			var getMyDeadline = _context.Deadlines.Where(u => u.CreateBy == getCurrentUser).ToList();

			var dropDownListDeadlineCourse = new CourseDeadlineViewModel
			{
				Course = getCourseInDb,
				Deadlines = getMyDeadline,
			};

			return View(dropDownListDeadlineCourse);
		}

		[HttpPost]
		public ActionResult CreateAssignment(Assignment assignment, int id)
		{
			var getCourseInDb = _context.Courses.SingleOrDefault(c => c.Id == id);

			var newAssignment = new Assignment
			{
				CourseId = getCourseInDb.Id,
				Name = assignment.Name,
				DeadlineId = assignment.DeadlineId,
			};

			_context.Assignments.Add(newAssignment);
			_context.SaveChanges();

			return RedirectToAction("ManageMyCourse");
		}*/
	}
}