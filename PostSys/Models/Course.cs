using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PostSys.Models
{
	public class Course
	{
		public int Id { get; set; }

		public string Name { get; set; }
		

		public int ClassId { get; set; }
		public Class Class { get; set; }

		public string StudentId { get; set; }
		public ApplicationUser Student { get; set; }

		public string EnrollmentKey { get; set; }
	}
}