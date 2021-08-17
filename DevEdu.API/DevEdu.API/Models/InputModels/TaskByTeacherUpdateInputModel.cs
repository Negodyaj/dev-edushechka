namespace DevEdu.API.Models
{
    public class TaskByTeacherUpdateInputModel : TaskInputModel
    {
        public HomeworkInputModel Homework { get; set; }
    }
}