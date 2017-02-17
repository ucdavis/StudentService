using System.Data.SqlClient;

namespace StudentService.Models
{
    public class Student
    {
        public Student(dynamic student)
        {
            Pidm = student.pidm;
            StudentId = student.studentId;
            FirstName = student.firstName;
            Mi = student.mi;
            LastName = student.lastName;
            Email = student.email;
            EarnedUnits = student.earnedUnits;
            CurrentUnits = student.currentUnits;
            Major = student.major;
            LastTerm = student.lastTerm;
            Astd = student.astd;
            LoginId = student.loginId;
            Sja = student.sja;
        }

        public Student(SqlDataReader reader)
        {
            Pidm = reader[0] as string;
            StudentId = reader[1] as string;
            FirstName = reader[2] as string;
            Mi = reader[3] as string;
            LastName = reader[4] as string;
            Email = reader[5] as string;
            EarnedUnits = (decimal)reader[6];
            CurrentUnits = (decimal)reader[7];
            Major = reader[8] as string;
            LastTerm = reader[9] as string;
            Astd = reader[10] as string;
            LoginId = reader[11] as string;
            Sja = false;
        }

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