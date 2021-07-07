using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
