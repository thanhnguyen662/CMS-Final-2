using PostSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PostSys.ViewModels
{
	public class StudentClassViewModel
	{
		public Course Course { get; set; }
		public IEnumerable<Class> Classes { get; set; }
		public IEnumerable<ApplicationUser> Students { get; set; }
	}
}