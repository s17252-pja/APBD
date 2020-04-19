using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wyklad4.Models;

namespace Wyklad4
{
    public class Student
    {
        public string IndexNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BirthDate { get; set; }
        public Enrollment Enrollment { get; set; }
    }
}
