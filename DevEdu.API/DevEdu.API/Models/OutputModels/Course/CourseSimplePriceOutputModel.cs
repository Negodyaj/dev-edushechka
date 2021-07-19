using System.Collections.Generic;

namespace DevEdu.API.Models.OutputModels
{
    public class CourseSimplePriceOutputModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<GroupMethodistOutputModel> Groups { get; set; }
    }
}
