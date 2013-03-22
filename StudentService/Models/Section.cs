using System.Collections.Generic;

namespace StudentService.Models
{
    public class Section
    {
        public Section(dynamic section)
        {
            Crn = section.Crn;
            Sequence = section.Sequence;
        }

        public int Crn { get; set; }
        public string Sequence { get; set; }
        public IEnumerable<Classtime> Classtimes { get; set; }
    }
}