using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevEdu.DAL.Models;

namespace DevEdu.API.Models.OutputModels
{
    public class TaskInfoOutputModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Description { get; set; }
        public string Links { get; set; }
        public bool IsRequired { get; set; }
        public List<TagInfoOutputModel> Tags { get; set; }
    }
}
