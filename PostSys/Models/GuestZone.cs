using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PostSys.Models
{
	public class GuestZone
	{
		public int Id { get; set; }
		public string Name
		{
			get
			{
				return Faculty.Name + "_" + Guest.UserName;
			}
		}

		public string GuestId { get; set; }
		public ApplicationUser Guest { get; set; }

		public int FacultyId { get; set; }
		public Faculty Faculty { get; set; }
	}
}