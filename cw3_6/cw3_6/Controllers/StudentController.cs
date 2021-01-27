using cw3_6.DAL;
using cw3_6.DTOs.Requests;
using cw3_6.Models;
using cw3_6.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace cw3.Controllers
{

    [ApiController]
    [Route("api/students")]
    [Authorize]
    public class StudentsController : ControllerBase
    {

        public IConfiguration Configuration { get; set; }

        private readonly IStudentDbService _dbService;

        public StudentsController(IConfiguration configuration, IStudentDbService dbService)
        {
            Configuration = configuration;
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

        [HttpPost]
        public IActionResult Login(LoginRequest request)
        {
            try
            {
                var response = _dbService.Login(request);

                if (response != null)
                {
                    var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, response.Login),
                new Claim(ClaimTypes.Role, "student")
            };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken
                    (
                        //issuer: "Gakko",
                        //audience: "Students",
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(10),
                        signingCredentials: creds
                    );

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        RefreshToken = Guid.NewGuid()
                    });

                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception exc)
            {
                return Unauthorized();
            }


        }

    }
}

