using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.API.Models.OutputModels
{
    public class RaitingOutputModel
    {
        public int Id { get; set; }
        public int UserId { get; set; } // change to UserOutputModel
        public RaitingTypeOutputModel RaitingType { get; set; }
        public int Raiting { get; set; }
    }
}
