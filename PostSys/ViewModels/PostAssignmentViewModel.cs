using PostSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PostSys.ViewModels
{
	public class PostAssignmentViewModel
	{
		public Post Post { get; set; }
		public Assignment Assignment { get; set; }

		public IEnumerable<Assignment> Assignments { get; set; }
		public IEnumerable<Post> Posts { get; set; }

		public int StatusPost { get; set; }
	}
}