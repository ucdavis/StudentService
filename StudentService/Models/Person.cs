namespace StudentService.Models
{
    public class Person
    {
        public Person(dynamic person)
        {
            FirstName = person.FirstName;
            LastName = person.LastName;
            LoginId = person.LoginId;
            Email = person.Email;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LoginId { get; set; }
        public string Email { get; set; }
    }
}