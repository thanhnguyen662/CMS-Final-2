using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PostSys.Models
{
	public class Post
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public int AssignmentId { get; set; }
		public Assignment Assignment { get; set; }

		
		public byte[] File { get; set; }
		
		public string UrlFile { get; set; }
		public string NameOfFile { get; set; }

		public DateTime PostDate { get; set; }
	}
}