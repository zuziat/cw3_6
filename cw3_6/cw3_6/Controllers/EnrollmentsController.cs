using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using cw3_6.DTOs.Requests;
using cw3_6.Models;
using cw3_6.Models.DTOs.Requests;
using cw3_6.Models.DTOs.Responses;
using cw3_6.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace cw3_6.Controllers
{

    //controller API vs MVC 

    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {

        private IStudentDbService _service;
        public EnrollmentsController(IStudentDbService service)
        {
            _service = service;
        }


        [HttpPost]
        [Authorize(Roles = "employee")]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            _service.EnrollStudent(request);

            var response = new EnrollStudentResponse();
           // response.LastName = st.LastName;


            return Ok(response);
        }

        [HttpPost("/api/enrollments/promotions")]
        [Authorize(Roles = "employee")]

        public IActionResult PromoteStudents(PromoteStudentRequest request)
        {
            try
            {
                var response = _service.PromoteStudents(request);

                if (response != null)
                {
                    return Created("", response);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (SqlException exc)
            {
                return BadRequest();
            }
        }
    }
}
