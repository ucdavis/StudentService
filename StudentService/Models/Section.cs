using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentService.Models
{
    public class Section
    {
        public Section(dynamic section)
        {
            SectionType = section.SectionType;
            Crn = section.Crn;
            Sequence = section.Sequence;
        }

        public string SectionType { get; set; }
        public int Crn { get; set; }
        public string Sequence { get; set; }
        public IEnumerable<Classtime> Classtimes { get; set; }
    }
}