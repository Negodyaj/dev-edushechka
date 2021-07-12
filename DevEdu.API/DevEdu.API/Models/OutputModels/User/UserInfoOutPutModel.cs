using DevEdu.DAL.Enums;
using System;
using System.Collections.Generic;

namespace DevEdu.API.Models.OutputModels.User
{
    public class UserInfoOutPutModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Photo { get; set; }
        public List<Role> Roles { get; set; }

        public override bool Equals(object obj)
        {
            return obj is UserInfoOutPutModel model &&
                   Id == model.Id &&
                   FirstName == model.FirstName &&
                   LastName == model.LastName &&
                   Email == model.Email &&
                   Photo == model.Photo &&
                   EqualityComparer<List<Role>>.Default.Equals(Roles, model.Roles);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, FirstName, LastName, Email, Photo, Roles);
        }
    }
}
