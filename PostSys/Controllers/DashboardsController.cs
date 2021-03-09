using PostSys.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PostSys.Controllers
{
	public class DashboardsController : Controller
	{
		private ApplicationDbContext _context;

		public DashboardsController()
		{
			_context = new ApplicationDbContext();
		}
		// GET: Dashboards
		[HttpGet]
		public ActionResult Index()
		{
			//Get all post
			var listPost = _context.Posts.ToList();
			//Get all publication
			var listPublication = _context.Publications.ToList();
			//Create new list
			List<int> dataPostInFac = new List<int>();
			List<int> dataPostPub = new List<int>();
			/////////////fill faculty

			//data
			var getAsm = _context.Assignments.ToList();
			var getCourse = _context.Courses.ToList();
			var getClasses = _context.Classes.ToList();
			var getFaculties = _context.Faculties.ToList();
			var facultytype = listPost.Select(p => p.Assignment.Course.Class.Faculty.Name).Distinct();


			//count post of each faculty
			foreach (var item in facultytype)
			{
				dataPostInFac.Add(listPost.Count(x => x.Assignment.Course.Class.Faculty.Name == item));
				dataPostPub.Add(listPublication.Count(x1 => x1.Post.Assignment.Course.Class.Faculty.Name == item));
			}

			var analysisData = dataPostInFac;
			var analysisData1 = dataPostPub;
			//X
			ViewBag.FACULTYTYPE = facultytype;
			//Y
			ViewBag.ANALYSISDATA = analysisData.ToList();
			//Y1
			ViewBag.ANALYSISDATA1 = analysisData1.ToList();

			return View();
		}

		public ActionResult PieChart()
		{
			//Get all post
			var listPost = _context.Posts.ToList();

			//Create new list
			List<int> dataPostInFac = new List<int>();

			/////////////fill faculty
			//data
			var getAsm = _context.Assignments.ToList();
			var getCourse = _context.Courses.ToList();
			var getClasses = _context.Classes.ToList();
			var getFaculties = _context.Faculties.ToList();
			var facultytype = listPost.Select(p => p.Assignment.Course.Class.Faculty.Name).Distinct();

			//count post of each faculty
			foreach (var item in facultytype)
			{
				dataPostInFac.Add(listPost.Count(x => x.Assignment.Course.Class.Faculty.Name == item));
			}
			var analysisData = dataPostInFac;
			//X
			ViewBag.FACULTYTYPE = facultytype;
			//Y
			ViewBag.ANALYSISDATA = analysisData.ToList();
			return View();
		}
	}
}