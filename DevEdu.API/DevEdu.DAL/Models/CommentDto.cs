namespace DevEdu.DAL.Models
{
    public class CommentDto : BaseDto
    {
        public int UserId { get; set; }
        public string Text { get; set; }
        public UserDto User { get; set; }
        public LessonDto Lesson { get; set; }
        public StudentAnswerOnTaskDto StudentAnswer { get; set; }
    }
}