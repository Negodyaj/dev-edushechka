using System;
using DevEdu.DAL.Enums;

namespace DevEdu.API.Models.OutputModels
{
    public class GroupOutputMiniModel
    {
        public int Id { get; set; }
        public GroupStatus GroupStatus { get; set; }
        public DateTime StartDate { get; set; }
    }
}