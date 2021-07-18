using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.API.Models.OutputModels
{
    public class GroupInfoOutputModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StartDate { get; set; }
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
    }
}
