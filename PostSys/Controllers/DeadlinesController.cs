using Microsoft.AspNet.Identity;
using PostSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PostSys.Controllers
{
	public class DeadlinesController : Controller
	{
		private ApplicationDbContext _context;

		public DeadlinesController()
		{
			_context = new ApplicationDbContext();
		}

		public ActionResult ListDeadline()
		{
			var getAllDeadline = _context.Deadlines.ToList();

			return View(getAllDeadline);
		}

		public ActionResult CreateDeadline()
		{
			return View();
		}

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

			_context.Deadlines.Add(createNewDeadline);
			_context.SaveChanges();

			return RedirectToAction("ListDeadline");
		}

		public ActionResult ManageMyDeadline()
		{
			var getCurrentCoordinator = User.Identity.GetUserName();

			var getMyDeadline = _context.Deadlines.Where(u => u.CreateBy == getCurrentCoordinator)
												  .ToList();

			return View(getMyDeadline);
		}

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
	}
}