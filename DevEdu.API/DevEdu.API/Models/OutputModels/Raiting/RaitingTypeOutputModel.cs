using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.API.Models.OutputModels
{
    public class RaitingTypeOutputModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }

        public override bool Equals(object obj)
        {
            return obj is RaitingTypeOutputModel model &&
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
