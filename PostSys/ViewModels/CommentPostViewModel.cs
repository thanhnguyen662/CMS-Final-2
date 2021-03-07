using PostSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PostSys.ViewModels
{
	public class CommentPostViewModel
	{
		public Post Post { get; set; }
		public Comment Comment { get; set; }
	}
}