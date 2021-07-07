    {
﻿using System;

namespace DevEdu.DAL.Models
{
    public class LessonDto : BaseDto
    {
        public DateTime Date { get; set; }
        public string TeacherComment { get; set; }
        public int TeacherId { get; set; }
    }
}