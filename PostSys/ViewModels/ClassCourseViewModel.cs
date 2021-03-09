using PostSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PostSys.ViewModels
{
	public class ClassCourseViewModel
	{
		public Course Course { get; set; }
		public Class Class { get; set; }
		public ApplicationUser Student { get; set; }

		public string CourseName { get; set; }
		public string StudentId { get; set; }
		public string ClassId { get; set; }
		public string EnrollmentKey { get; set; }
	}
}