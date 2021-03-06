using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PostSys.Models
{
	public class Assignment
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public int DeadlineId { get; set; }
		public Deadline Deadline { get; set; }

		public int CourseId { get; set; }
		public Course Course { get; set; }
	}
}