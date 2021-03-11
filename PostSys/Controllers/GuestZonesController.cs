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
	public class GuestZonesController : Controller
	{
		private ApplicationDbContext _context;

		public GuestZonesController()
		{
			_context = new ApplicationDbContext();
		}

		[Authorize(Roles = "Marketing Manager")]
		public ActionResult ListGuest()
		{
			var listGuest = _context.GuestZones.Include(g => g.Guest).Include(f => f.Faculty).ToList();

			return View(listGuest);
		}

		[Authorize(Roles = "Marketing Manager")]
		[HttpGet]
		public ActionResult CreateGuest()
		{
			var role = (from r in _context.Roles where r.Name.Contains("Guest") select r)
															 .FirstOrDefault();
			var listGuest = _context.Users.Where(x => x.Roles.Select(y => y.RoleId)
									      .Contains(role.Id))
									      .ToList();

			var listFaculty = _context.Faculties.ToList();

			var dropDownListFaculty = new GuestFacultyViewModel
			{
				Faculties = listFaculty,
				Guests = listGuest
			};

			return View(dropDownListFaculty);
		}

		[Authorize(Roles = "Marketing Manager")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CreateGuest(GuestZone guestZone)
		{
			var isExistGuest = _context.GuestZones.Any(n => n.FacultyId == guestZone.FacultyId || 
													   n.GuestId == guestZone.GuestId);
			if(isExistGuest == true)
			{
				return View("~/Views/ErrorValidations/Exist.cshtml");
			}

			var newGuest = new GuestZone
			{
				FacultyId = guestZone.FacultyId,
				GuestId = guestZone.GuestId			
			};

			if(newGuest.GuestId == null || newGuest.Name == null)
			{
				return View("~/Views/ErrorValidations/Exist.cshtml");
			}

			_context.GuestZones.Add(newGuest);
			_context.SaveChanges();

			return RedirectToAction("ListGuest");
		}

		[Authorize(Roles = "Marketing Manager")]
		public ActionResult DeleteGuest(int id)
		{
			var guestInDb = _context.GuestZones.SingleOrDefault(i => i.Id == id);

			if (guestInDb == null)
			{
				return View("~/Views/ErrorValidations/Null.cshtml");
			}

			_context.GuestZones.Remove(guestInDb);
			_context.SaveChanges();

			return RedirectToAction("ListGuest");
		}

		[Authorize(Roles = "Guest")]
		public ActionResult GuestZoneFaculty()
		{
			var getListPublication = _context.Publications.ToList();
			var getStudent = _context.Users.ToList();

			var getUser = User.Identity.GetUserId();
			var getGuestInFacultyIds = _context.GuestZones.Where(m => m.GuestId == getUser)
														  .Include(f => f.Faculty)
														  .Include(u => u.Guest)
														  .Select(m => m.FacultyId)
														  .ToList();
			var getGuestInFacultyId = getGuestInFacultyIds[0];

			var guestZoneFaculty = _context.Publications.Where(n => n.Post.Assignment.Course.Class.Faculty.Id == getGuestInFacultyId)
													    .Include(p => p.Post.Assignment.Course.Class.Faculty)
												        .ToList();

			return View(guestZoneFaculty);
		}

	}
}