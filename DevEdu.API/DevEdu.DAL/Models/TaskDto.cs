using System;
using System.Collections.Generic;

namespace DevEdu.DAL.Models
{
    public class TaskDto : BaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Links { get; set; }
        public bool IsRequired { get; set; }
        public List<TagDto> Tags { get; set; }
        public List<StudentAnswerOnTaskDto> StudentAnswers { get; set; }
        public List<CourseDto> Courses { get; set; }


        public override bool Equals(object obj)
        {
            return Equals((TaskDto)obj);
        }

        private bool Equals(TaskDto actual)
        {
            return actual != null
                   && Id == actual.Id
                   && Name == actual.Name
                   && Description == actual.Description
                   && Links == actual.Links
                   && IsRequired == actual.IsRequired;
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(base.GetHashCode());
            hashCode.Add(Id);
            hashCode.Add(Name);
            hashCode.Add(Description);
            hashCode.Add(Links);
            hashCode.Add(IsRequired);
            return hashCode.ToHashCode();
            
        }
    }
}