using cw3_6.DAL;
using cw3_6.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace cw3.Controllers
{

    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {

        private readonly IDbService _dbService;

        public StudentsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public IActionResult GetStudent()
        {
            var list = new List<Student>();
            using(var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s18919;Integrated Security=True"))
            using(var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select * from Student";

                con.Open();
                SqlDataReader dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var st = new Student();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.IndexNumber = dr["IndexNumber"].ToString();
                    st.IdEnrollment = dr["IdEnrollment"].ToString();

                    list.Add(st);
                }
                
            }
          
            return Ok(list);
        }
       // [HttpGet("{indexNumber}")]

        //public IActionResult GetStudent(string indexNumber)
        //{
        //    var list = new List<Student>();
        //    using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s18919;Integrated Security=True"))
        //    using (var com = new SqlCommand())
        //    {
        //        com.Connection = con;
        //        com.CommandText = "select * from Student where indexnumber='"+indexNumber+"'";

        //        con.Open();
        //        SqlDataReader dr = com.ExecuteReader();
        //        if (dr.Read())
        //        {
        //            var st = new Student();
        //            st.FirstName = dr["FirstName"].ToString();
        //            st.LastName = dr["LastName"].ToString();
        //            st.IndexNumber = dr["IndexNumber"].ToString();

        //            return Ok(st);
        //        }

        //    }

        //    return NotFound();
        //}

        [HttpGet("{indexNumber}")]

        public IActionResult GetStudent(string indexNumber)
        {

        

            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s18919;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select IdEnrollment from Student where indexnumber=@id";
                //SqlParameter par = new SqlParameter();
                //par.Value = indexNumber;
                //par.ParameterName = "index";
                com.Parameters.AddWithValue("index", indexNumber);

                con.Open();
                SqlDataReader dr = com.ExecuteReader();
                if (dr.Read())
                {
                    var st = new Student();
                    st.IdEnrollment = dr["IdEnrollment"].ToString();
                    

                    return Ok(st.IdEnrollment);
                }

            }

            return NotFound();
        }


        [HttpPost]

        public IActionResult CreateStudent(Student student)
        {
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            return Ok(student);
        }

        [HttpPut("{id}")]

        public IActionResult ModifyStudent(int id)
        {


            return Ok("Aktualizacja dokończona");

        }

        [HttpDelete("{id}")]

        public IActionResult DeleteStudent(int id)
        {

            return Ok("Usuwanie ukończone");

        }

    }
}

