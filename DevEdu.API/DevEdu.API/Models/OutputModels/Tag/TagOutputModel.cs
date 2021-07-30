using System;
using System.ComponentModel;

namespace DevEdu.API.Models
{
    public class TagOutputModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        public override bool Equals(object obj)
        {
            return obj is TagOutputModel model &&
                   Id == model.Id &&
                   Name == model.Name &&
                   IsDeleted == model.IsDeleted;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, IsDeleted);
        }
    }
}