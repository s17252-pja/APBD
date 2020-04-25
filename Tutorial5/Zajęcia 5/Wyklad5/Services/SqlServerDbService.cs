using Wyklad5.Helpers;
using Wyklad5.Models;
using Wyklad5.DTOs.RequestModels;
using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Wyklad5.Services
{
    public class SqlServerDbService : IStudentDbService
    {
        private const string ConnStr = "Data Source=db-mssql;Initial Catalog=s17252;Integrated Security=True";

        public IEnumerable<Student> GetStudents()
        {
            var list = new List<Student>();
            using (var con = new SqlConnection(ConnStr))
            {
                using var cmd = new SqlCommand
                {
                    Connection = con,
                    CommandText = @"SELECT s.IndexNumber, s.FirstName, s.LastName, s.BirthDate, s.IdEnrollment 
                                    FROM Student s;"
                };

                con.Open();
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var student = new Student
                    {
                        IndexNumber = dr["IndexNumber"].ToString(),
                        FirstName = dr["FirstName"].ToString(),
                        LastName = dr["LastName"].ToString(),
                        BirthDate = DateTime.Parse(dr["BirthDate"].ToString()),
                        IdEnrollment = int.Parse(dr["IdEnrollment"].ToString())
                    };
                    list.Add(student);
                }
            }
            return list;
        }
        public Student GetStudent(string indexNumber)
        {
            using var con = new SqlConnection(ConnStr);
            using var cmd = new SqlCommand
            {
                Connection = con,
                CommandText = @"SELECT s.IndexNumber,
                                       s.FirstName,
                                       s.LastName,
                                       s.BirthDate,
                                       s.IdEnrollment 
                                FROM Student s
                                WHERE s.IndexNumber = @indexNumber;"
            };

            cmd.Parameters.AddWithValue("indexNumber", indexNumber);

            con.Open();
            using var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                return new Student
                {
                    IndexNumber = dr["IndexNumber"].ToString(),
                    FirstName = dr["FirstName"].ToString(),
                    LastName = dr["LastName"].ToString(),
                    BirthDate = DateTime.Parse(dr["BirthDate"].ToString()),
                    IdEnrollment = int.Parse(dr["IdEnrollment"].ToString())
                };
            }
            else
                return null;
        }
        public void CreateStudent(string indexNumber, string firstName, string lastName, DateTime birthDate, int idEnrollment, SqlConnection sqlConnection = null, SqlTransaction transaction = null)
        {
            using var cmd = new SqlCommand
            {
                CommandText = @"INSERT INTO Student(IndexNumber, FirstName, LastName, BirthDate, IdEnrollment)
                                VALUES (@IndexNumber, @FirstName, @LastName, @BirthDate, @IdEnrollment);"
            };
            cmd.Parameters.AddWithValue("IndexNumber", indexNumber);
            cmd.Parameters.AddWithValue("FirstName", firstName);
            cmd.Parameters.AddWithValue("LastName", lastName);
            cmd.Parameters.AddWithValue("BirthDate", birthDate);
            cmd.Parameters.AddWithValue("IdEnrollment", idEnrollment);

            if (sqlConnection == null)
            {
                using var con = new SqlConnection(ConnStr);
                con.Open();
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
            }
            else
            {
                cmd.Connection = sqlConnection;
                cmd.Transaction = transaction;
                cmd.ExecuteNonQuery();
            }
        }
        public bool CheckIfEnrollmentExists(string studies, int semester)
        {
            using var con = new SqlConnection(ConnStr);
            using var cmd = new SqlCommand
            {
                Connection = con,
                CommandText = @"SELECT e.IdEnrollment
                                FROM Enrollment e JOIN Studies s ON e.IdStudy = s.IdStudy
                                WHERE s.Name = @Name AND e.Semester = @Semester;"
            };
            cmd.Parameters.AddWithValue("Name", studies);
            cmd.Parameters.AddWithValue("Semester", semester);

            con.Open();
            using var dr = cmd.ExecuteReader();
            return dr.Read();
        }

        #region methods with logic
        public Enrollment CreateStudentWithStudies(StudentWithStudiesRequest request)
        {
            using var con = new SqlConnection(ConnStr);
            con.Open();
            using var transaction = con.BeginTransaction();

            //check if studies exists
            if (!CheckIfStudiesExists(request.Studies, con, transaction))
            {
                transaction.Rollback();
                throw new DbServiceException(DbServiceExceptionTypeEnum.NotFound, "Studies does not exist.");
            }

            //get (or create and get) the enrollment
            var enrollment = GetNewestEnrollment(request.Studies, 1, con, transaction);
            if (enrollment == null)
            {
                CreateEnrollment(request.Studies, 1, DateTime.Now, con, transaction);
                enrollment = GetNewestEnrollment(request.Studies, 1, con, transaction);
            }

            //check if provided index number is unique
            if (GetStudent(request.IndexNumber) != null)
            {
                transaction.Rollback();
                throw new DbServiceException(DbServiceExceptionTypeEnum.ValueNotUnique, $"Index number ({request.IndexNumber}) is not unique.");
            }

            //create a student and commit the transaction
            CreateStudent(request.IndexNumber, request.FirstName, request.LastName, request.BirthDate, enrollment.IdEnrollment, con, transaction);
            transaction.Commit();

            //return Enrollment object
            return enrollment;
        }
        #endregion

        #region private helpers
        private bool CheckIfStudiesExists(string name, SqlConnection sqlConnection, SqlTransaction transaction)
        {
            using var cmd = new SqlCommand
            {
                Connection = sqlConnection,
                Transaction = transaction,
                CommandText = @"SELECT 1 from Studies s WHERE s.Name = @name;"
            };
            cmd.Parameters.AddWithValue("name", name);
            using var dr = cmd.ExecuteReader();
            return dr.Read();
        }
        private Enrollment GetNewestEnrollment(string studiesName, int semester, SqlConnection sqlConnection, SqlTransaction transaction)
        {
            using var cmd = new SqlCommand
            {
                Connection = sqlConnection,
                Transaction = transaction,
                CommandText = @"SELECT TOP 1 e.IdEnrollment, e.IdStudy, e.StartDate
                                FROM Enrollment e JOIN Studies s ON e.IdStudy=s.IdStudy
                                WHERE e.Semester = @Semester AND s.Name = @Name
                                ORDER BY IdEnrollment DESC;"
            };

            cmd.Parameters.AddWithValue("Name", studiesName);
            cmd.Parameters.AddWithValue("Semester", semester);

            using var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                return new Enrollment
                {
                    IdEnrollment = int.Parse(dr["IdEnrollment"].ToString()),
                    Semester = semester,
                    IdStudy = int.Parse(dr["IdStudy"].ToString()),
                    StartDate = DateTime.Parse(dr["StartDate"].ToString()),
                };
            }
            return null;
        }
        private void CreateEnrollment(string studiesName, int semester, DateTime startDate, SqlConnection sqlConnection, SqlTransaction transaction)
        {
            using var cmd = new SqlCommand
            {
                Connection = sqlConnection,
                Transaction = transaction,
                CommandText = @"INSERT INTO Enrollment(IdEnrollment, IdStudy, StartDate, Semester)
                                VALUES ((SELECT ISNULL(MAX(e.IdEnrollment)+1,1) FROM Enrollment e), 
		                                (SELECT s.IdStudy FROM Studies s WHERE s.Name = @Name), 
		                                @StartDate,
		                                @Semester);"
            };

            cmd.Parameters.AddWithValue("Name", studiesName);
            cmd.Parameters.AddWithValue("Semester", semester);
            cmd.Parameters.AddWithValue("StartDate", startDate);
            cmd.ExecuteNonQuery();
        }

        public Enrollment PromoteStudents(string studies, int semester)
        {
            using var con = new SqlConnection(ConnStr);
            using var cmd = new SqlCommand
            {
                Connection = con,
                CommandType = CommandType.StoredProcedure,
                CommandText = @"sp_promoteStudents"
            };
            cmd.Parameters.AddWithValue("@Studies", studies);
            cmd.Parameters.AddWithValue("@Semester", semester);
            con.Open();
            using var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                return new Enrollment
                {
                    IdEnrollment = int.Parse(dr["IdEnrollment"].ToString()),
                    Semester = semester,
                    IdStudy = int.Parse(dr["IdStudy"].ToString()),
                    StartDate = DateTime.Parse(dr["StartDate"].ToString())
                };
            }
            else
                throw new DbServiceException(DbServiceExceptionTypeEnum.ProcedureError, "something went wrong");
        }
        #endregion


    }
}
