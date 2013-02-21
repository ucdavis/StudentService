using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentService.Models
{
    public class Person
    {
        public Person(dynamic person)
        {
            Id = person.Id;
            FirstName = person.FirstName;
            LastName = person.LastName;
            LoginId = person.LoginId;
            Email = person.Email;
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Mi { get; set; }
        public string LastName { get; set; }
        public string LoginId { get; set; }
        public string Email { get; set; }
    }
}