using DevEdu.DAL.Enums;
using System;
using System.Collections.Generic;

namespace DevEdu.API.Models.OutputModels.User
{
    public class UserFullInfoOutPutModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string RegistrationDate { get; set; }
        public string ContractNumber { get; set; }
        public string BirthDate { get; set; }
        public string GitHubAccount { get; set; }
        public string Photo { get; set; }
        public string PhoneNumber { get; set; }
        public string ExileDate { get; set; }
        public City City { get; set; }
        public List<Role> Roles { get; set; }

        public override bool Equals(object obj)
        {
            return obj is UserFullInfoOutPutModel model &&
                   Id == model.Id &&
                   FirstName == model.FirstName &&
                   LastName == model.LastName &&
                   Patronymic == model.Patronymic &&
                   Email == model.Email &&
                   Username == model.Username &&
                   RegistrationDate == model.RegistrationDate &&
                   ContractNumber == model.ContractNumber &&
                   BirthDate == model.BirthDate &&
                   GitHubAccount == model.GitHubAccount &&
                   Photo == model.Photo &&
                   PhoneNumber == model.PhoneNumber &&
                   ExileDate == model.ExileDate &&
                   City == model.City &&
                   EqualityComparer<List<Role>>.Default.Equals(Roles, model.Roles);
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Id);
            hash.Add(FirstName);
            hash.Add(LastName);
            hash.Add(Patronymic);
            hash.Add(Email);
            hash.Add(Username);
            hash.Add(RegistrationDate);
            hash.Add(ContractNumber);
            hash.Add(BirthDate);
            hash.Add(GitHubAccount);
            hash.Add(Photo);
            hash.Add(PhoneNumber);
            hash.Add(ExileDate);
            hash.Add(City);
            hash.Add(Roles);
            return hash.ToHashCode();
        }
    }
}
