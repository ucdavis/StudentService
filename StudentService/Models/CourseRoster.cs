namespace StudentService.Models
{
    public class CourseRoster
    {
        public Person[] Students { get; set; }
        public Person[] Instructors { get; set; }
    }    
}