using PostSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PostSys.ViewModels
{
	public class GuestFacultyViewModel
	{
		public GuestZone GuestZone { get; set; }
		public IEnumerable<Faculty> Faculties { get; set; }
		public IEnumerable<ApplicationUser> Guests { get; set; }
		public Faculty Faculty { get; set; }
		public ApplicationUser Guest { get; set; }
	}
}