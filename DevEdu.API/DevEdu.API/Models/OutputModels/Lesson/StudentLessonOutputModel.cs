using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.API.Models.OutputModels
{
    public class StudentLessonOutputModel
    {
		public int Id { get; set; }
		public int UserId { get; set; }
		public string Feedback { get; set; }
		public bool IsPresent { get; set; }
		public string AbsenceReason { get; set; }

	}
}
