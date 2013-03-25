namespace StudentService.Models
{
    public class Person
    {
        public Person(dynamic person)
        {
            Pidm = person.Id;
            FirstName = person.FirstName;
            LastName = person.LastName;
            LoginId = person.LoginId;
            Email = person.Email;
        }

        public int Pidm { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LoginId { get; set; }
        public string Email { get; set; }
    }
}