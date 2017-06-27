using System.Data.SqlClient;

namespace StudentService.Models
{
    public class Student
    {
        public string Pidm { get; set; }
        public string StudentId  { get; set; }
        public string FirstName { get; set; }
        public string Mi { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public decimal EarnedUnits { get; set; }
        public decimal CurrentUnits { get; set; }
        public string Major { get; set; }
        public string LastTerm { get; set; }
        public string Astd { get; set; }
        public string LoginId { get; set; }
        public bool Sja { get; set; }
    }
}