using PostSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PostSys.ViewModels
{
	public class CourseDeadlineViewModel
	{
		public Assignment Assignment { get; set; }
		public Course Course { get; set; }
		public IEnumerable<Deadline> Deadlines { get; set; }
		public IEnumerable<Course> Courses { get; set; }

		
	}
}