using System.Collections.Generic;

namespace StudentService.Models
{
    public class Course
    {
        public Course(dynamic course)
        {
            Subject = course.Subject;
            CourseNumb = course.CourseNumb;
            Name = course.Name;
        }

        public string Subject { get; set; }
        public string CourseNumb { get; set; }
        public string Name { get; set; }
        public IEnumerable<Section> Sections { get; set; }
    }
}