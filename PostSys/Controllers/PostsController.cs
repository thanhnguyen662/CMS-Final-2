using PostSys.Models;
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

namespace PostSys.Controllers
{
	public class PostsController : Controller
	{
		private ApplicationDbContext _context;

		public PostsController()
		{
			_context = new ApplicationDbContext();
		}

		// manager
		public ActionResult ListPost()
		{
			var getPost = _context.Posts.Include(a => a.Assignment).ToList();
			
			return View(getPost);
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
	}
}