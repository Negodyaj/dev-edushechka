namespace DevEdu.API.Models.InputModels
{
    public class TaskByTeacherUpdateInputModel : TaskInputModel
    {
        public HomeworkInputModel Homework { get; set; }
    }
}