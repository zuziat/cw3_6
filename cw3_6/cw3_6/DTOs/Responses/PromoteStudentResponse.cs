﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw3_6.DTOs.Responses
{
    public class PromoteStudentResponse
    {


        public int IdEnrollment { get; set; }

        public int Semester { get; set; }

        public string Studies { get; set; }

        public DateTime StartDate { get; set; }
    }
}
