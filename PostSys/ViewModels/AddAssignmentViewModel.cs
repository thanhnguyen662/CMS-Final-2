using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PostSys.ViewModels
{
	public class AddAssignmentViewModel
	{
        public string CourseName { get; set; }
        public int CourseId { get; set; }
        public string ClassName { get; set; }
        public string StudentName { get; set; }
        public string AssignmentName { get; set; }
        public bool IsSelected { get; set; }
    }
}