using cw3_6.DTOs.Requests;
using cw3_6.DTOs.Responses;
using cw3_6.Models;
using cw3_6.Models.DTOs.Requests;
using cw3_6.Models.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace cw3_6.Services
{
    public class SqlServerStudentDbService : IStudentDbService
    {
        public EnrollStudentResponse EnrollStudent(EnrollStudentRequest request)
        {
            var st = new Student();
            st.FirstName = request.FirstName;


            using (var con = new SqlConnection(""))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();
                var tran = con.BeginTransaction();
                try
                {
                    com.CommandText = "select IdStudies from studies where name=@name";
                    com.Parameters.AddWithValue("name", request.Studies);

                    var dr = com.ExecuteReader();
                    if (!dr.Read())
                    {
                        tran.Rollback();
                        //  return BadRequest("Studia nie istnieją");
                    }
                    int idStudies = (int)dr["IdStudies"];

                    com.CommandText = "select IdEnrollment from enrollment where Semester=1 AND isStudies =@idStudies";
                    com.Parameters.AddWithValue("idStudies", idStudies);

                    var dr2 = com.ExecuteReader();

                    if (!dr2.Read())
                    {
                        com.CommandText = "INSERT INTO Enrollments(Semester, IdStudy, StartDate) VALUES(@sem, @idstud, @sdate)";
                        com.Parameters.AddWithValue("sem", 1);
                        com.Parameters.AddWithValue("idstud", idStudies);
                        com.Parameters.AddWithValue("sdate", DateTime.Now);
                    }


                    com.CommandText = "INSERT INTO Student(IndexNumber, FirstName, LastName, BirthDate) VALUES(@Index, @Fname, @Lname, @BDate)";
                    com.Parameters.AddWithValue("Index", request.IndexNumber);
                    com.Parameters.AddWithValue("Fname", request.FirstName);
                    com.Parameters.AddWithValue("Lname", request.LastName);
                    com.Parameters.AddWithValue("BDate", request.BirthDate);
                    com.ExecuteNonQuery();

                    tran.Commit();
                }
                catch (SqlException exc)
                {
                    tran.Rollback();
                }
            }
        }



            public PromoteStudentResponse PromoteStudents(PromoteStudentRequest request)
        {

            var response = new PromoteStudentResponse();
            DateTime date = DateTime.Today;
            int success;
            const string ConString = "Data Source=db-mssql;Initial Catalog=s18985;Integrated Security=True";

            using (var con = new SqlConnection(ConString))
            using (var com = new SqlCommand())
            {
                com.Connection = con;

                con.Open();
                var tran = con.BeginTransaction();

                com.CommandText = "execute PromoteStudent @study, @semester";
                com.Parameters.AddWithValue("study", request.Studies);
                com.Parameters.AddWithValue("semester", request.Semester);
                com.Transaction = tran;
                success = com.ExecuteNonQuery();

                var nextSemestr = request.Semester + 1;
                com.CommandText = "select IdEnrollment from enrollment where semester = @nextSemester and idstudy = (select idstudy from studies where name = @study)";
                com.Parameters.AddWithValue("nextSemester", nextSemestr);
                com.Transaction = tran;
                var dr = com.ExecuteScalar();

                response.IdEnrollment = (int)dr;

                com.CommandText = "select StartDate from enrollment where semester = @nextSemester and idstudy = (select idstudy from studies where name = @study)";
                com.Transaction = tran;
                dr = com.ExecuteScalar();

                response.StartDate = (DateTime)dr;

                tran.Commit();

                response.Studies = request.Studies;
                response.Semester = request.Semester + 1;


                if (success > 0)
                {
                    return response;
                    return response;
                }
                else
                {
                    return null;
                }


            }
        }


    }
}
