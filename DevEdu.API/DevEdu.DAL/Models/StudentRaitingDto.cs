using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEdu.DAL.Models
{
    public class StudentRaitingDto
    {
        public int Id { get; set; }
        public UserDto User { get; set; }
        public RaitingTypeDto RaitingType { get; set; }
        public int Raiting { get; set; }
    }
}
