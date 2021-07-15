using System;

namespace DevEdu.API.Models.OutputModels
{
    public class GroupTaskInfoOutputModel
    {
        public int Id { get; set; }
        public TaskInfoOutputMiniModel Task { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}