using cw3_6.DTOs.Requests;
using cw3_6.DTOs.Responses;
using cw3_6.Models.DTOs.Requests;
using cw3_6.Models.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw3_6.Services
{
    public class OracleStudentDbService : IStudentDbService
    {

        public PromoteStudentResponse PromoteStudents(PromoteStudentRequest request)
        {
            throw new NotImplementedException();
        }

        EnrollStudentResponse IStudentDbService.EnrollStudent(EnrollStudentRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
