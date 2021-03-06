using PostSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PostSys.ViewModels
{
	public class CoordinatorFacultyViewModel
	{
		public Class Class { get; set; }
		public IEnumerable<Faculty> Faculties { get; set; }
		public IEnumerable<ApplicationUser> Coordinators { get; set; }
	}
}