using System;
using System.ComponentModel.DataAnnotations;

namespace DevEdu.API.Models.InputModels
{
    public class GroupInputModel
    {
        [Required]
        public int CourseId { get; set; }
        public DateTime StartDate { get; set; }
        public string Timetable { get; set; }
        public decimal PaymentPerMonth { get; set; }
    }
}