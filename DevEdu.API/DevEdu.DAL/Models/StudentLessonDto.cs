namespace DevEdu.DAL.Models
{
	public class StudentLessonDto
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public int LessonId { get; set; }
		public string Feedback { get; set; }
		public bool IsPresent { get; set; }
		public string AbsenceReason { get; set; }



	}
}