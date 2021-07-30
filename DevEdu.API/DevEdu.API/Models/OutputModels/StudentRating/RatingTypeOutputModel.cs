using System;

namespace DevEdu.API.Models
{
    public class RatingTypeOutputModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }

        public override bool Equals(object obj)
        {
            return obj is RatingTypeOutputModel model &&
                   Id == model.Id &&
                   Name == model.Name &&
                   Weight == model.Weight;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Weight);
        }
    }
}
