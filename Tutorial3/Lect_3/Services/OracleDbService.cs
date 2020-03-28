using Lect_3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lect_3.Services
{
    public class OracleDbService : IDbService
    {
        public object Response { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Student Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Student> GetStudents()
        {
            //implement real communication with db
            return null;
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool Put(int id, Student student)
        {
            throw new NotImplementedException();
        }
    }
}
