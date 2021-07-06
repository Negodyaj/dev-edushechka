using System;

namespace DevEdu.DAL.Models
{
    public class UserDto : BaseDto
    {
        public string FisrtName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string ContractNumber { get; set; }
        public int CityId { get; set; }
        public DateTime BirthDate { get; set; }
        public string GitHubAccount { get; set; }
        public string Photo { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime ExileDate { get; set; }

        public override bool Equals(object obj)
        {
            var item = obj as UserDto;

            if (item == null)
            {
                return false;
            }

            return this.Id.Equals(item.Id);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}