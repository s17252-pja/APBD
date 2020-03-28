using Lect_3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lect_3.Services
{
    public class MockDbService : IDbService
    {
        private static IList<Student> _students;

        static MockDbService()
        {
            _students = new List<Student>
            {
                new Student{IdStudent=1, FirstName="Jan", LastName="Kowalski", IndexNumber="s1234"},
                new Student{IdStudent=2, FirstName="Anna", LastName="Malewska", IndexNumber="s3446"},
                new Student{IdStudent=3, FirstName="Malewska", LastName="Andrzejewicz", IndexNumber="s5644"}
            };
        }

        public object Response { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


        public Student Get(int id)
        {
            return _students.SingleOrDefault(student => student.IdStudent == id);
        }

        public IEnumerable<Student> GetStudents()
        {
            return _students;
        }

        public bool Delete(int id)
        {
            var student = _students.SingleOrDefault(student => student.IdStudent == id);
            if (student == null)
            {
                return false;
            }
            _students.Remove(student);
            return true;
        }

        public bool Put(int id, Student student)
        {
            var result = _students.SingleOrDefault(student => student.IdStudent == id);
            
            if (result == null)
            {
                return false;    
            }
            _students.Remove(result);
            _students.Add(student);
            return true;
        }

    }
}
