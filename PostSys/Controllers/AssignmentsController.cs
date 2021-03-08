using PostSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using System.IO;
using PostSys.ViewModels;

namespace PostSys.Controllers
{
	public class AssignmentsController : Controller
	{
		private ApplicationDbContext _context;

		public AssignmentsController()
		{
			_context = new ApplicationDbContext();
		}

		//Manager
		public ActionResult ListAssignment()
		{
			var getAssignment = _context.Assignments.Include(c => c.Course).Include(d => d.Deadline).ToList();
			
			return View(getAssignment);
		}

		public ActionResult DeleteAssignment(int id)
		{
			var assignmentInDb = _context.Assignments.SingleOrDefault(i => i.Id == id);

			_context.Assignments.Remove(assignmentInDb);
			_context.SaveChanges();

			return RedirectToAction("ManageMyAssignment");
		}

		public ActionResult ManageMyAssignment()
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

		private bool ValidateExtension(string extension)
		{
			extension = extension.ToLower();
			switch (extension)
			{
				case ".jpg":
					return true;
				case ".jpeg":
					return true;
				case ".png":
					return true;
				case ".doc":
					return true;
				case ".docx":
					return true;
				default:
					return false;
			}
		}

		[HttpGet]
		public ActionResult SubmitPost(int id)
		{
			var assignemntInDb = _context.Assignments.SingleOrDefault(i => i.Id == id);

			////////check validation: Deadline currentdate > EndDate
			//Find Enddate in currentDeadline
			int status = 1; // st=1 => can submit /// st=0 => can't submit
			var endDateList = (from ass in _context.Assignments
							   where ass.Id == id
							   join d in _context.Deadlines
							   on ass.DeadlineId equals d.Id
							   select d.EndDate).ToList();
			var endDate = endDateList[0];

			//check deadline
			if (DateTime.Now > endDate) //error
			{
				status = 0;
			}

			//

			var newPostAssignmentViewModel = new PostAssignmentViewModel
			{
				Assignment = assignemntInDb,
				StatusPost = status
			};

			return View(newPostAssignmentViewModel);
		}

		
		[HttpPost]
		public ActionResult SubmitPost([Bind(Include = "Name, Status, File, UrlFile, PostDate, NameOfFile")] HttpPostedFileBase file, Post post, Assignment assignment, int id)
		{
			string extension = Path.GetExtension(file.FileName);

			if (!ValidateExtension(extension))
			{
				return View("~/Views/ErrorValidations/Exist.cshtml");
			}

			if (file != null && file.ContentLength > 0)
			{
				//
				//Get Assignment Name
				var objAssignment = (from ass in _context.Assignments where ass.Id == id select ass.Name).ToList();
				var assignmentName = objAssignment[0];

				//Get Course Name
				var objCourse = (from ass in _context.Assignments
								 where ass.Id == id
								 join c in _context.Courses
								 on ass.CourseId equals c.Id
								 select c.Name).ToList();
				var courseName = objCourse[0];

				//
				var userName = User.Identity.GetUserName();
				string prepend = userName + "-" + assignmentName + "-";

				post.File = new byte[file.ContentLength]; // image stored in binary formate
				file.InputStream.Read(post.File, 0, file.ContentLength);
				string fileName = prepend + System.IO.Path.GetFileName(file.FileName);
				string urlImage = Server.MapPath("~/Files/" + fileName);

				post.NameOfFile = fileName;
				
				file.SaveAs(urlImage);

				post.UrlFile = "Files/" + fileName;
			}

			var assignemntInDb = _context.Assignments.SingleOrDefault(i => i.Id == id);

			var newPost = new Post
			{				
				NameOfFile = post.NameOfFile,
				AssignmentId = assignemntInDb.Id,
				Name = post.Name,
				PostDate = DateTime.Now,
				File = post.File,
				UrlFile = post.UrlFile
			};

			_context.Posts.Add(newPost);
			_context.SaveChanges();

			return View("~/Views/Home/Index.cshtml");
		}
	}
}