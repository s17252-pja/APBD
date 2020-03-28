using Lect_3.Models;
using System.Collections.Generic;

namespace Lect_3.Services
{
    public interface IDbService
    {
        object Response { get; set; }

        //Always good to use abstraction
        public IEnumerable<Student> GetStudents();
        public Student Get(int id);
        public bool Delete(int id);
        public bool Put(int id, Student student);
    }
}
