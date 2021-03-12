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
using PostSys.ViewModels;
using System.IO;
using Ionic.Zip;

namespace PostSys.Controllers
{
	public class PostsController : Controller
	{
		private ApplicationDbContext _context;

		public PostsController()
		{
			_context = new ApplicationDbContext();
		}

		[Authorize(Roles = "Marketing Manager")]
		//manager
		public ActionResult ListPost()
		{
			var listAssignment = _context.Assignments.Include(d => d.Deadline).Include(c => c.Course).ToList();

			var getPost = _context.Posts.Include(a => a.Assignment).ToList();
			
			return View(getPost);
		}

		[Authorize(Roles = "Marketing Coordinator, Student")]
		public ActionResult DeletePost(int id)
		{
			var courseInDb = _context.Posts.SingleOrDefault(c => c.Id == id);


			string rootFolder = Server.MapPath("~/Files/");

			var listFileName = _context.Posts.Where(p => p.Id == id).Select(p => p.NameOfFile).ToList();
			string nameOfFile = listFileName[0];
			if(nameOfFile != null)
			{
				System.IO.File.Delete(Path.Combine(rootFolder, nameOfFile));

				_context.Posts.Remove(courseInDb);
				_context.SaveChanges();

				return RedirectToAction("ManageMyPost");
			}
			else
			{
				_context.Posts.Remove(courseInDb);
				_context.SaveChanges();

				return RedirectToAction("ManageMyPost");
			}
		}
	

		[Authorize(Roles = "Marketing Coordinator, Student")]
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

		[Authorize(Roles = "Student")]
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

		[Authorize(Roles = "Marketing Manager")]
		public ActionResult ListPublication()
		{
			var getAssignmentCourse = _context.Assignments.Include(c => c.Course).ToList();
			var getClassCoordinator = _context.Classes.Include(c => c.Coordinator).ToList();

			var getPublication = _context.Publications.Include(p => p.Post).ToList();

			return View(getPublication);
		}

		[Authorize(Roles = "Marketing Coordinator")]
		public ActionResult CreatePublication(int id)
		{
			var getPostId = _context.Posts.SingleOrDefault(i => i.Id == id);

			//get postid
			var isExistPostId = _context.Publications.Where(m => m.PostId == id)
													 .Select(m => m.PostId)
													 .FirstOrDefault();

			var newPublication = new Publication
			{
				PostId = getPostId.Id
			};

			if(newPublication.PostId == isExistPostId)
			{
				return View("~/Views/ErrorValidations/Exist.cshtml");
			}

			_context.Publications.Add(newPublication);
			_context.SaveChanges();
			return RedirectToAction("ManageMyPost");
		}

		[Authorize(Roles = "Marketing Coordinator")]
		public ActionResult DeletePublication(int id)
		{
			var publicationInDb = _context.Publications.SingleOrDefault(i => i.Id == id);

			if(publicationInDb == null)
			{
				return View("~/Views/ErrorValidations/Exist.cshtml");
			}

			_context.Publications.Remove(publicationInDb);
			_context.SaveChanges();

			return RedirectToAction("ManageMyPublication");
		}

		[Authorize(Roles = "Marketing Coordinator, Student")]
		public ActionResult ManageMyPublication()
		{
			var getCurrentUserName = User.Identity.GetUserName();
			/*var getPostAssignment = _context.Posts.Include(a => a.Assignment).ToList();
			var getCourseClass = _context.Courses.Include(c => c.Class).Include(s => s.Student).ToList();
			var getClassCoordinator = _context.Classes.Include(c => c.Coordinator).ToList();*/

			if(User.IsInRole("Marketing Coordinator"))
			{
				var getMyPublication = _context.Publications.Where(u => u.Post.Assignment.Course.Class.Coordinator.UserName == getCurrentUserName)
															.Include(p => p.Post.Assignment.Course.Class.Coordinator)
															.ToList();
				return View(getMyPublication);
			}

			if (User.IsInRole("Student"))
			{
				var getMyPublication = _context.Publications.Where(u => u.Post.Assignment.Course.Student.UserName == getCurrentUserName)
															.Include(p => p.Post.Assignment.Course.Student)
															.ToList();
				return View(getMyPublication);
			}

			return View();
		}

		[Authorize(Roles = "Marketing Manager")]
		public ActionResult ListComment()
		{
			var getComment = _context.Comments.Include(p => p.Post.Assignment.Course.Class.Coordinator).ToList();
			var getCourseClassStudent = _context.Courses.Include(s => s.Student).Include(c => c.Class).ToList();
			
			/*var getPostAssignment = _context.Posts.Include(a => a.Assignment).ToList();
			var getAssignmentCourse = _context.Assignments.Include(c => c.Course).ToList();*/
			/*var getClassCoordinator = _context.Classes.Include(c => c.Coordinator).ToList();*/

			return View(getComment);
		}

		[Authorize(Roles = "Marketing Coordinator")]
		public ActionResult CreateComment()
		{

			return View();
		}


		[Authorize(Roles = "Marketing Coordinator")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CreateComment(Comment comment, int id)
		{
			var postInDb = _context.Posts.SingleOrDefault(i => i.Id == id);

			var isExist = _context.Comments.Any(d => d.Description == comment.Description);
			if(isExist == true)
			{
				return View("~/Views/ErrorValidations/Exist.cshtml");
			}

			var newComment = new Comment
			{
				Description = comment.Description,
				PostId = postInDb.Id
			};

			_context.Comments.Add(newComment);
			_context.SaveChanges();

			return RedirectToAction("ManageMyComment");
		}

		[Authorize(Roles = "Marketing Coordinator, Student")]
		public ActionResult ManageMyComment()
		{
			var getCurrentUserName = User.Identity.GetUserName();

			/*var getPostAssignment = _context.Posts.Include(a => a.Assignment).ToList();
			var getAssignmentCourse = _context.Assignments.Include(c => c.Course).ToList();
			var getCourseClassStudent = _context.Courses.Include(s => s.Student).Include(c => c.Class).ToList();
			var getClassCoordinator = _context.Classes.Include(c => c.Coordinator).ToList();*/

			if (User.IsInRole("Marketing Coordinator"))
			{
				var getMyCommentCoordinator = _context.Comments.Where(u => u.Post.Assignment.Course.Class.Coordinator.UserName == getCurrentUserName)
															   .Include(p => p.Post.Assignment.Course.Class.Coordinator)
															   .ToList();

				return View(getMyCommentCoordinator);
			}

			if (User.IsInRole("Student"))
			{
				var getMyCommentCoordinator = _context.Comments.Where(u => u.Post.Assignment.Course.Student.UserName == getCurrentUserName)
															   .Include(p => p.Post.Assignment.Course.Student)
															   .ToList();

				return View(getMyCommentCoordinator);
			}

			return View();
		}

		[Authorize(Roles = "Marketing Coordinator")]
		public ActionResult DeleteComment(int id)
		{
			var commentInDb = _context.Comments.SingleOrDefault(i => i.Id == id);

			_context.Comments.Remove(commentInDb);
			_context.SaveChanges();

			return RedirectToAction("ManageMyComment");
		}

		//Download zip 
		//Coordinator & Marketing Manager
		[Authorize(Roles = "Marketing Coordinator, Marketing Manager")]
		[HttpGet]
		public ActionResult DownloadZip()
		{
			//get current user (coor / manager) 
			var currentUser = User.Identity.GetUserName();
			string[] filePaths = Directory.GetFiles(Server.MapPath("~/Files/"));
			List<DownloadZipViewmodel> fileViewmodels = new List<DownloadZipViewmodel>();
			foreach (string filePath in filePaths)
			{
				//Get filename
				string fileName = Path.GetFileName(filePath);
				//Split tail
				string[] splitTail = fileName.Split('.');
				//get file name without tail 
				string headfile = splitTail[0];
				//Get element
				string[] splitElement = headfile.Split('-');
				//Get student name 
				string studentName = splitElement[0];
				//Get AssignmentName 
				string assignmentName = splitElement[1];
				////Get PostName////
				var postNameList = _context.Posts.Where(m => m.NameOfFile.Contains(fileName)).Select(m => m.Name).ToList();
				string postName = postNameList[0];
				///Get Course Name ///
				var assignmentIdList = _context.Posts.Where(m => m.NameOfFile.Contains(fileName)).Select(m => m.AssignmentId).ToList();
				int assignmentId = assignmentIdList[0];
				var courseNameList = (from ass in _context.Assignments
									  where ass.Id == assignmentId
									  join co in _context.Courses
									  on ass.CourseId equals co.Id
									  select co.Name).ToList();
				string courseName = courseNameList[0];
				///Get Coordinator Name 
				var classIdList = _context.Courses.Where(m => m.Name.Contains(courseName)).Select(m => m.ClassId).ToList();
				int classId = classIdList[0];
				var classCoor = (from cl in _context.Classes
								 where cl.Id == classId
								 join cor in _context.Users
								 on cl.CoordinatorId equals cor.Id
								 select cor.UserName).ToList();
				string coorName = classCoor[0];

				///Get enddate 
				var postIdList = _context.Posts.Where(m => m.NameOfFile.Contains(fileName)).Select(m => m.Id).ToList();
				int postId = postIdList[0];
				var enddatelist = _context.Posts.Where(m => m.Id == postId)
								.Include(m => m.Assignment.Deadline)
								.Select(m => m.Assignment.Deadline.EndDate)
								.ToList();
				var enddate = enddatelist[0];
				//Get faculty name
				var classfar = (from cl in _context.Classes
								where cl.Id == classId
								join fa in _context.Faculties
								on cl.FacultyId equals fa.Id
								select fa.Name).ToList();
				string facultyName = classfar[0];
				if (User.IsInRole("Marketing Coordinator"))
				{
					if (coorName == currentUser)
					{
						fileViewmodels.Add(new DownloadZipViewmodel()
						{
							/*FileName = Path.GetFileName(filePath),*/
							FileName = Path.GetExtension(filePath),
							FilePath = filePath,
							PostName = postName,
							AssignmentName = assignmentName,
							StudentName = studentName,
						});
					}
				}
				if (User.IsInRole("Marketing Manager"))
				{
					if (DateTime.Now > enddate)
					{
						fileViewmodels.Add(new DownloadZipViewmodel()
						{
							/*FileName = Path.GetFileName(filePath),*/
							FileName = Path.GetExtension(filePath),
							FilePath = filePath,
							PostName = postName,
							AssignmentName = assignmentName,
							StudentName = studentName,
							CourseName = courseName,
							CoordinatorName = coorName,
							FacultyName = facultyName
						});
					}
				}
			}
			return View(fileViewmodels);
		}

		[Authorize(Roles = "Marketing Coordinator, Marketing Manager")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult DownloadZip(List<DownloadZipViewmodel> files)
		{
			using (ZipFile zip = new ZipFile())
			{
				zip.AlternateEncodingUsage = ZipOption.AsNecessary;
				zip.AddDirectoryByName("Files");
				foreach (DownloadZipViewmodel file in files)
				{
					if (file.IsSelected)
					{
						zip.AddFile(file.FilePath, "Files");
					}
				}
				string zipName = String.Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
				using (MemoryStream memoryStream = new MemoryStream())
				{
					zip.Save(memoryStream);
					return File(memoryStream.ToArray(), "application/zip", zipName);
				}
			}
		}
	}
}