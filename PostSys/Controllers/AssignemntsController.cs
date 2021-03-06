using PostSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace PostSys.Controllers
{
	public class AssignemntsController : Controller
	{
		private ApplicationDbContext _context;

		public AssignemntsController()
		{
			_context = new ApplicationDbContext();
		}

		//Manager
		public ActionResult ListAssignment()
		{
			var getAssignment = _context.Assignments.Include(c => c.Course).Include(d => d.Deadline).ToList();
			
			return View(getAssignment);
		}

		/*public ActionResult ManageMyAssignemntCoordinator()
		{
			var getCurrentCoordinatorUserName = User.Identity.GetUserName();

			var getCourse = _context.Courses.ToList();
			var getClass = _context.Classes.ToList();
			var getCoordinator = _context.Users.ToList();
			var getDeadline = _context.Deadlines.ToList();

			var getMyAssignmentCoordinator = _context.Assignments.Where(u => u.Course.Class.Coordinator.UserName == getCurrentCoordinatorUserName)
																 .Include(c => c.Course)
																 .ToList();
			return View(getMyAssignmentCoordinator);
		}

		public ActionResult ManageMyAssignemntStudent()
		{
			var getCurrentStudentUserName = User.Identity.GetUserName();

			var getCourse = _context.Courses.ToList();
			var getClass = _context.Classes.ToList();
			var getStudent = _context.Users.ToList();
			var getDeadline = _context.Deadlines.ToList();

			var getMyAssignmentStudent = _context.Assignments.Where(u => u.Course.Student.UserName == getCurrentStudentUserName)
																 .Include(c => c.Course)
																 .ToList();
			return View(getMyAssignmentStudent);
		}*/

		public ActionResult ManageMyAssignemt()
		{
			var getUserName = User.Identity.GetUserName();

			var getCourse = _context.Courses.ToList();
			var getClass = _context.Classes.ToList();
			var getCoordinator = _context.Users.ToList();
			var getDeadline = _context.Deadlines.ToList();

			if(User.IsInRole("Marketing Coordinator"))
			{
				var getMyAssignmentCoordinator = _context.Assignments.Where(u => u.Course.Class.Coordinator.UserName == getUserName)
																 .Include(c => c.Course)
																 .ToList();
				return View(getMyAssignmentCoordinator);
			}

			if (User.IsInRole("Student"))
			{
				var getMyAssignmentStudent = _context.Assignments.Where(u => u.Course.Student.UserName == getUserName)
																	 .Include(c => c.Course)
																	 .ToList();
				return View(getMyAssignmentStudent);
			}

			return View();
		}
	}
}