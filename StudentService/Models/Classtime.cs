using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentService.Models
{
    public class Classtime
    {
        public Classtime(dynamic classTime)
        {
            ClassType = classTime.SectionType;
            StartDate = classTime.StartDate;
            EndDate = classTime.EndDate;
            StartTime = classTime.StartTime;
            EndTime = classTime.EndTime;
            DaysOfWeek = classTime.DaysOfWeek;
        }

        public string ClassType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string DaysOfWeek { get; set; }
    }
}