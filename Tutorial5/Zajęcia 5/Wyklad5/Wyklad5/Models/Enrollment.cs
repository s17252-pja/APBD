using System;
using Wyklad5.Models;

namespace Wyklad5.Models
{
    public class Enrollment
    {
        public int IdEnrollment { get; set; }
        public int Semester { get; set; }
        public DateTime StartDate { get; set; }
        public Studies Study { get; set; }

    }
}