using Microsoft.AspNet.Identity;
using PostSys.Models;
using PostSys.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace PostSys.Controllers
{
	public class DeadlinesController : Controller
	{
		private ApplicationDbContext _context;

		public DeadlinesController()
		{
			_context = new ApplicationDbContext();
		}

		[Authorize(Roles = "Marketing Manager")]
		public ActionResult ListDeadline()
		{
			var getAllDeadline = _context.Deadlines.ToList();

			return View(getAllDeadline);
		}

		[Authorize(Roles = "Marketing Coordinator")]
		public ActionResult CreateDeadline()
		{
			return View();
		}

		[Authorize(Roles = "Marketing Coordinator")]
		[HttpPost]
		public ActionResult CreateDeadline(Deadline deadline)
		{
			var getUserName = User.Identity.GetUserName();

			if (deadline.StartDate >= deadline.EndDate)
			{
				return View("~/Views/ErrorValidations/Null.cshtml");
			}

			var createNewDeadline = new Deadline
			{
				CreateBy = getUserName,
				StartDate = deadline.StartDate,
				EndDate = deadline.EndDate
			};

			if(deadline.StartDate == null || deadline.EndDate == null)
			{
				return View("~/Views/ErrorValidations/Null.cshtml");
			}

			_context.Deadlines.Add(createNewDeadline);
			_context.SaveChanges();

			return RedirectToAction("ManageMyDeadline");
		}

		[Authorize(Roles = "Marketing Coordinator")]
		public ActionResult ManageMyDeadline()
		{
			var getCurrentCoordinator = User.Identity.GetUserName();

			var getMyDeadline = _context.Deadlines.Where(u => u.CreateBy == getCurrentCoordinator)
												  .ToList();

			return View(getMyDeadline);
		}

		[Authorize(Roles = "Marketing Coordinator")]

		public ActionResult DeleteDeadline(int id)
		{
			var deadlineInDb = _context.Deadlines.SingleOrDefault(i => i.Id == id);

			_context.Deadlines.Remove(deadlineInDb);
			_context.SaveChanges();

			if (User.IsInRole("Marketing Coordinator"))
			{
				return RedirectToAction("ManageMyDeadline");
			}

			return View();
		}

		[Authorize(Roles = "Marketing Coordinator")]
		[HttpGet]
		public ActionResult AddAssignment()
		{
			var getClass = _context.Classes.Include(c => c.Coordinator).ToList();
			var getStudent = _context.Users.ToList();

			var getCourse = _context.Courses.Include(c => c.Class).Include(s => s.Student).ToList();


			///////////////////

			var CurrentCoor = User.Identity.GetUserId();
			var CourseList = (from cl in _context.Classes
							  where cl.CoordinatorId.Contains(CurrentCoor)
							  join c in _context.Courses
							  on cl.Id equals c.ClassId
							  select c).ToList();
			List<AddAssignmentViewModel> addAsignment = new List<AddAssignmentViewModel>();
			foreach (var item in CourseList)
			{
				int numb = _context.Assignments.Where(m => m.CourseId == item.Id).Count();
				int n = numb + 1;

				addAsignment.Add(new AddAssignmentViewModel()
				{
					CourseId = item.Id,
					CourseName = item.Name,
					AssignmentName = "Assignment " + n,
					ClassName = item.Class.Name,
					StudentName = item.Student.UserName,
				});
			}
			return View(addAsignment);
		}

		[Authorize(Roles = "Marketing Coordinator")]
		[HttpPost]
		public ActionResult AddAssignment(List<AddAssignmentViewModel> addAsignment, int id)
		{
			var getDeadlineId = _context.Deadlines.SingleOrDefault(i => i.Id == id);
			foreach (AddAssignmentViewModel add in addAsignment)
			{
				if (add.IsSelected)
				{

					var newAssignment = new Assignment
					{
						CourseId = add.CourseId,
						Name = add.AssignmentName,
						DeadlineId = getDeadlineId.Id,
					};

					_context.Assignments.Add(newAssignment);
					_context.SaveChanges();
				}

			}

			return RedirectToAction("ManageMyDeadline");
		}
	}
}