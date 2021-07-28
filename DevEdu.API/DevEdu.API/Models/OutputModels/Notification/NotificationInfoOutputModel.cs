using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevEdu.DAL.Enums;

namespace DevEdu.API.Models.OutputModels
{
    public class NotificationInfoOutputModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public UserInfoOutPutModel User { get; set; }
        public GroupInfoOutputModel Group { get; set; }
        public int RoleId { get; set; }
        public string Date { get; set; }
        public bool IsDeleted { get; set; }
    }
}
