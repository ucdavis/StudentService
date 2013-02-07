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
            StartDate = section.StartDate;
            EndDate = section.EndDate;
            StartTime = section.StartTime;
            EndTime = section.EndTime;
            DaysOfWeek = section.DaysOfWeek;
        }
        public string SectionType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string DaysOfWeek { get; set; }
    }
}