﻿using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace PostSys.Models
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext()
			: base("DefaultConnection", throwIfV1Schema: false)
		{
		}

		public DbSet<Faculty> Faculties { get; set; }
		public DbSet<Class> Classes { get; set; }
		public DbSet<Course> Courses { get; set; }
		public DbSet<Deadline> Deadlines { get; set; }
		public DbSet<Assignment> Assignments { get; set; }

		public static ApplicationDbContext Create()
		{
			return new ApplicationDbContext();
		}
	}
}