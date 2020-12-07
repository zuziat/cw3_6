using cw3_6.Models;
using System.Collections.Generic;


namespace cw3_6.DAL
{
    public interface IDbService
    {
        public IEnumerable<Student> GetStudents();
    }
}