using System.Collections.Generic;

namespace StudentService.Models
{
    public class Section
    {
        public Section(dynamic section)
        {
            TermCode = section.TermCode;
            Crn = section.Crn;
            Sequence = section.Sequence;
        }

        public int TermCode { get; set; }
        public int Crn { get; set; }
        public string Sequence { get; set; }
        public IEnumerable<Classtime> Classtimes { get; set; }
    }
}