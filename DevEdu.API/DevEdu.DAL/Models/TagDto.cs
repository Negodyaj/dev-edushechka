using System;

namespace DevEdu.DAL.Models
{
    public class TagDto : BaseDto
    {
        public string Name { get; set; }

        public override bool Equals(object obj)
        {

            return obj != null &&
              obj is TagDto tagDto &&
              Id == tagDto.Id &&
              Name == tagDto.Name;
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(base.GetHashCode());
            hashCode.Add(Id);
            hashCode.Add(Name);
            return hashCode.ToHashCode();
        }
    }
}