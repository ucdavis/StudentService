namespace StudentService.Models
{
    public class CourseRoster
    {
        public int Crn { get; set; }
        public Person[] Students { get; set; }
        public Person[] Instructors { get; set; }
    }    
}