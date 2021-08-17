namespace DevEdu.API.Models
{
    public class TaskByTeacherInputModel : TaskInputModel
    {
        public HomeworkInputModel Homework { get; set; }
        public int GroupId { get; set; }
    }
}