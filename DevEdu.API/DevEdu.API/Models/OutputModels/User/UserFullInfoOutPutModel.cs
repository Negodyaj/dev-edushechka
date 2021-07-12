using DevEdu.DAL.Enums;
using System;
using System.Collections.Generic;

namespace DevEdu.API.Models.OutputModels.User
{
    public class UserFullInfoOutPutModel: UserInfoOutPutModel
    {
        public string Username { get; set; }
        public string RegistrationDate { get; set; }
        public string ContractNumber { get; set; }
        public string BirthDate { get; set; }
        public string GitHubAccount { get; set; }
        public string PhoneNumber { get; set; }
        public string ExileDate { get; set; }
        public City City { get; set; }

        public override bool Equals(object obj)
        {
            return obj is UserFullInfoOutPutModel model &&
                   base.Equals(obj) &&
                   Id == model.Id &&
                   FirstName == model.FirstName &&
                   LastName == model.LastName &&
                   Email == model.Email &&
                   Photo == model.Photo &&
                   EqualityComparer<List<Role>>.Default.Equals(Roles, model.Roles) &&
                   Username == model.Username &&
                   RegistrationDate == model.RegistrationDate &&
                   ContractNumber == model.ContractNumber &&
                   BirthDate == model.BirthDate &&
                   GitHubAccount == model.GitHubAccount &&
                   PhoneNumber == model.PhoneNumber &&
                   ExileDate == model.ExileDate &&
                   City == model.City;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(base.GetHashCode());
            hash.Add(Id);
            hash.Add(FirstName);
            hash.Add(LastName);
            hash.Add(Email);
            hash.Add(Photo);
            hash.Add(Roles);
            hash.Add(Username);
            hash.Add(RegistrationDate);
            hash.Add(ContractNumber);
            hash.Add(BirthDate);
            hash.Add(GitHubAccount);
            hash.Add(PhoneNumber);
            hash.Add(ExileDate);
            hash.Add(City);
            return hash.ToHashCode();
        }
    }
}
