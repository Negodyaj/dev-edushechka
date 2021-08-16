using System;

namespace DevEdu.API.Models
{
    public class UserInfoShortOutputModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Photo { get; set; }

        public override bool Equals(object obj)
        {
            return obj is UserInfoOutPutModel model &&
                   Id == model.Id &&
                   FirstName == model.FirstName &&
                   LastName == model.LastName &&
                   Email == model.Email &&
                   Photo == model.Photo;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, FirstName, LastName, Email, Photo);
        }
    }
}