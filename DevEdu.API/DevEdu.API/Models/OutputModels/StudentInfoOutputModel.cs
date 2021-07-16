using System.Collections.Generic;
using DevEdu.DAL.Enums;

namespace DevEdu.API.Models.OutputModels
{
    public class StudentInfoOutputModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Photo { get; set; }
    }
}