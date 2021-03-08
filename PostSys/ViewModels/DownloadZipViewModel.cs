using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PostSys.ViewModels
{
    public class DownloadZipViewmodel
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public bool IsSelected { get; set; }
        public string PostName { get; set; }
        public string AssignmentName { get; set; }
        public string CourseName { get; set; }
        public string StudentName { get; set; }
        //Manager
        public string CoordinatorName { get; set; }
        public string FacultyName { get; set; }
    }
}