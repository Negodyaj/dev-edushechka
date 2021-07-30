using System;
using System.ComponentModel.DataAnnotations;
using static DevEdu.API.Common.ValidationMessage;

namespace DevEdu.API.Models
{
    public class TagInputModel
    {
        [Required(ErrorMessage = NameRequired)]
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            return obj is TagInputModel model &&
                   Name == model.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }
    }
}