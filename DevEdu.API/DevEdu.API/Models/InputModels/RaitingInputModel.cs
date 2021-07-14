using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.API.Models.InputModels
{
    public class RaitingInputModel
    {
        public int UserId { get; set; }
        public int RaitingTypeId { get; set; }
        public int Raiting { get; set; }
    }
}
