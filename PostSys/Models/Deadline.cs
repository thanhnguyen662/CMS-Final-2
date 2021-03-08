using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PostSys.Models
{
	public class Deadline
	{
		public int Id { get; set; }
		public string CreateBy { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }


		public string Name
		{
			get { return StartDate.ToShortDateString() + " - " + EndDate.ToShortDateString(); }
		}
	}
}