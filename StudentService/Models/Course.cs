using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentService.Models
{
    public class Course
    {
        public Course(dynamic course)
        {
            Crn = course.Crn;
            Subject = course.Subject;
            CourseNumb = course.CourseNumb;
            Sequence = course.Sequence;
            Name = course.Name;
        }

        public int Crn { get; set; }
        public string Subject { get; set; }
        public string CourseNumb { get; set; }
        public string Sequence { get; set; }
        public string Name { get; set; }
        public IEnumerable<Section> Sections { get; set; }
    }
}