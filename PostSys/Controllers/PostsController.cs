﻿using PostSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Configuration;
using System.Net.Mail;
using System.Text;
using System.Net;
using Microsoft.AspNet.Identity;
using PostSys.ViewModels;

namespace PostSys.Controllers
{
	public class PostsController : Controller
	{
		private ApplicationDbContext _context;

		public PostsController()
		{
			_context = new ApplicationDbContext();
		}

		//manager
		public ActionResult ListPost()
		{
			var listAssignment = _context.Assignments.Include(d => d.Deadline).Include(c => c.Course).ToList();

			var getPost = _context.Posts.Include(a => a.Assignment).ToList();
			
			return View(getPost);
		}

		public ActionResult DeletePost(int id)
		{
			var postInDb = _context.Posts.SingleOrDefault(i => i.Id == id);

			_context.Posts.Remove(postInDb);
			_context.SaveChanges();

			return RedirectToAction("ManageMyPost");
		}

		public ActionResult ManageMyPost()
		{
			var getDeadline = _context.Deadlines.ToList();
			var getCourse = _context.Courses.ToList();

			var getCurrentUserName = User.Identity.GetUserName();

			if(User.IsInRole("Marketing Coordinator"))
			{
				var getMyPostCoordinator = _context.Posts.Where(u => u.Assignment.Course.Class.Coordinator.UserName == getCurrentUserName)
														 .Include(a => a.Assignment)
														 .ToList();
				return View(getMyPostCoordinator);
			}

			if (User.IsInRole("Student"))
			{
				var getMyPostStudent = _context.Posts.Where(u => u.Assignment.Course.Student.UserName == getCurrentUserName)
													 .Include(a => a.Assignment)
													 .ToList();

				return View(getMyPostStudent);
			}

			return View();
		}

		public FileResult DownloadFile(Post post)
		{

			var getFileById = _context.Posts.SingleOrDefault(c => c.Id == post.Id);

			return File(getFileById.File, "file", getFileById.UrlFile);
		}


		public bool SendEmail(string toEmail, string emailSubject, string emailBody)
		{

			var senderEmail = ConfigurationManager.AppSettings["SenderEmail"].ToString();
			var senderPassword = ConfigurationManager.AppSettings["SenderPassword"].ToString();

			SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
			smtpClient.EnableSsl = true;
			smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
			smtpClient.UseDefaultCredentials = false;
			smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);

			MailMessage mailMessage = new MailMessage(senderEmail, toEmail, emailSubject, emailBody);
			mailMessage.IsBodyHtml = true;
			mailMessage.BodyEncoding = UTF8Encoding.UTF8;

			smtpClient.Send(mailMessage);

			return true;

		}

		public ActionResult SendEmailToUser()
		{
			bool result = false;

			var currentUser = User.Identity.GetUserId();
			var getUserName = User.Identity.GetUserName();
			//Get current Assignment Name
			var CourseAssignment = (from c in _context.Courses
									where c.StudentId.Contains(currentUser)
									join ass in _context.Assignments
									on c.Id equals ass.CourseId
									select ass.Name).ToList();
			var AssignmentName = CourseAssignment[0];

			//Get current Coordinator Email
			var CourseClass = (from c in _context.Courses
							   where c.StudentId.Contains(currentUser)
							   join cl in _context.Classes
							   on c.ClassId equals cl.Id
							   select cl.CoordinatorId).ToList();
			var CoorId = CourseClass[0];

			var coordinator = _context.Users.Where(m => m.Id.Contains(CoorId)).Select(m => m.Email).ToList();
			var coordinatorEmail = coordinator[0];

			//Get current Course name
			var course = _context.Courses.Where(m => m.StudentId.Contains(currentUser)).Select(m => m.Name).ToList();
			var courseName = course[0];


			result = SendEmail($"{coordinatorEmail}", "Notification Email", $"Student: {getUserName} <br> Assignemnt: {AssignmentName} <br> Course: {courseName} <br> Already submit a post");


			return Json(result, JsonRequestBehavior.AllowGet);
		}

		public ActionResult ListPublication()
		{
			var getAssignmentCourse = _context.Assignments.Include(c => c.Course).ToList();
			var getClassCoordinator = _context.Classes.Include(c => c.Coordinator).ToList();

			var getPublication = _context.Publications.Include(p => p.Post).ToList();

			return View(getPublication);
		}

		public ActionResult CreatePublication(int id)
		{
			var getPostId = _context.Posts.SingleOrDefault(i => i.Id == id);

			var newPublication = new Publication
			{
				PostId = getPostId.Id
			};

			_context.Publications.Add(newPublication);
			_context.SaveChanges();
			return RedirectToAction("ManageMyPost");
		}

		public ActionResult DeletePublication(int id)
		{
			var publicationInDb = _context.Publications.SingleOrDefault(i => i.Id == id);

			_context.Publications.Remove(publicationInDb);
			_context.SaveChanges();

			return RedirectToAction("ManageMyPublication");
		}

		public ActionResult ManageMyPublication()
		{
			var getCurrentUserName = User.Identity.GetUserName();
			var getPostAssignment = _context.Posts.Include(a => a.Assignment).ToList();
			var getCourseClass = _context.Courses.Include(c => c.Class).Include(s => s.Student).ToList();
			var getClassCoordinator = _context.Classes.Include(c => c.Coordinator).ToList();

			if(User.IsInRole("Marketing Coordinator"))
			{
				var getMyPublication = _context.Publications.Where(u => u.Post.Assignment.Course.Class.Coordinator.UserName == getCurrentUserName)
															.Include(p => p.Post)
															.ToList();
				return View(getMyPublication);
			}

			if (User.IsInRole("Student"))
			{
				var getMyPublication = _context.Publications.Where(u => u.Post.Assignment.Course.Student.UserName == getCurrentUserName)
															.Include(p => p.Post)
															.ToList();
				return View(getMyPublication);
			}

			return View();
		}

		public ActionResult ListComment()
		{
			var getComment = _context.Comments.Include(p => p.Post).ToList();

			var getPostAssignment = _context.Posts.Include(a => a.Assignment).ToList();
			var getAssignmentCourse = _context.Assignments.Include(c => c.Course).ToList();
			var getCourseClassStudent = _context.Courses.Include(s => s.Student).Include(c => c.Class).ToList();
			var getClassCoordinator = _context.Classes.Include(c => c.Coordinator).ToList();

			return View(getComment);
		}

		public ActionResult CreateComment()
		{

			return View();
		}

		[HttpPost]
		public ActionResult CreateComment(Comment comment, int id)
		{
			var postInDb = _context.Posts.SingleOrDefault(i => i.Id == id);

			var newComment = new Comment
			{
				Description = comment.Description,
				PostId = postInDb.Id
			};

			_context.Comments.Add(newComment);
			_context.SaveChanges();

			return RedirectToAction("ListComment");
		}

		public ActionResult ManageMyComment()
		{
			var getCurrentUserName = User.Identity.GetUserName();

			var getPostAssignment = _context.Posts.Include(a => a.Assignment).ToList();
			var getAssignmentCourse = _context.Assignments.Include(c => c.Course).ToList();
			var getCourseClassStudent = _context.Courses.Include(s => s.Student).Include(c => c.Class).ToList();
			var getClassCoordinator = _context.Classes.Include(c => c.Coordinator).ToList();

			if (User.IsInRole("Marketing Coordinator"))
			{
				var getMyCommentCoordinator = _context.Comments.Where(u => u.Post.Assignment.Course.Class.Coordinator.UserName == getCurrentUserName)
															   .Include(p => p.Post)
															   .ToList();

				return View(getMyCommentCoordinator);
			}

			if (User.IsInRole("Student"))
			{
				var getMyCommentCoordinator = _context.Comments.Where(u => u.Post.Assignment.Course.Student.UserName == getCurrentUserName)
															   .Include(p => p.Post)
															   .ToList();

				return View(getMyCommentCoordinator);
			}

			return View();
		}
		public ActionResult DeleteComment(int id)
		{
			var commentInDb = _context.Comments.SingleOrDefault(i => i.Id == id);

			_context.Comments.Remove(commentInDb);
			_context.SaveChanges();

			return RedirectToAction("ManageMyComment");
		}
	}
}