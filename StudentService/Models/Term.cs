using System;

namespace StudentService.Models
{
    public class Term
    {
        public Term(dynamic term)
        {
            Id = term.Id;
            Name = term.Name;
            Start = term.Start;
            End = term.End;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}