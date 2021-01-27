using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw3_6.Models.DTOs.Responses
{
    public class EnrollStudentResponse
    {
        public string IndexNumber { get; set; }
        public int IdEnrollment { get; set; }
        public int Semester { get; set; }
        public string Studies { get; set; }
        public DateTime StartDate { get; set; }
    }
}
