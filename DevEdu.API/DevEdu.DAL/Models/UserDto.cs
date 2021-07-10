using System;
using System.Collections.Generic;
using DevEdu.DAL.Enums;

namespace DevEdu.DAL.Models
{
    public class UserDto : BaseDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string ContractNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public string GitHubAccount { get; set; }
        public string Photo { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime ExileDate { get; set; }
        public City CityId { get; set; }
        public List<Role> Roles { get; set; }

        public override bool Equals(object obj)
        {
            return obj is UserDto dto &&
                   Id == dto.Id &&
                   IsDeleted == dto.IsDeleted &&
                   FirstName == dto.FirstName &&
                   LastName == dto.LastName &&
                   Patronymic == dto.Patronymic &&
                   Email == dto.Email &&
                   Username == dto.Username &&
                   Password == dto.Password &&
                   RegistrationDate == dto.RegistrationDate &&
                   ContractNumber == dto.ContractNumber &&
                   BirthDate == dto.BirthDate &&
                   GitHubAccount == dto.GitHubAccount &&
                   Photo == dto.Photo &&
                   PhoneNumber == dto.PhoneNumber &&
                   ExileDate == dto.ExileDate &&
                   CityId == dto.CityId &&
                   EqualityComparer<List<Role>>.Default.Equals(Roles, dto.Roles);
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Id);
            hash.Add(IsDeleted);
            hash.Add(FirstName);
            hash.Add(LastName);
            hash.Add(Patronymic);
            hash.Add(Email);
            hash.Add(Username);
            hash.Add(Password);
            hash.Add(RegistrationDate);
            hash.Add(ContractNumber);
            hash.Add(BirthDate);
            hash.Add(GitHubAccount);
            hash.Add(Photo);
            hash.Add(PhoneNumber);
            hash.Add(ExileDate);
            hash.Add(CityId);
            hash.Add(Roles);
            return hash.ToHashCode();
        }
    }
}