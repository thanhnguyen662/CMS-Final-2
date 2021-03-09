using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PostSys.Models
{
	public class Class
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }
		

		public string CoordinatorId { get; set; }
		public ApplicationUser Coordinator { get; set; }

		public int FacultyId { get; set; }
		public Faculty Faculty { get; set; }

		public string EnrollmentKey { get; set; }
	}
}