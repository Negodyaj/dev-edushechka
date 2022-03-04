﻿using DevEdu.API.Common;
using DevEdu.DAL.Enums;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static DevEdu.API.Common.ValidationMessage;

namespace DevEdu.API.Models
{
    public class UserInsertInputModel
    {
        [Required(ErrorMessage = FirstNameRequired)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = LastNameRequired)]
        public string LastName { get; set; }

        [Required(ErrorMessage = PatronymicRequired)]
        public string Patronymic { get; set; }

        [Required(ErrorMessage = EmailRequired)]
        [EmailAddress(ErrorMessage = WrongEmailFormat)]
        public string Email { get; set; }

        [Required(ErrorMessage = UsernameRequired)]
        public string Username { get; set; }

        [Required(ErrorMessage = PasswordRequired)]
        [MinLength(8, ErrorMessage = WrongFormatPassword)]
        public string Password { get; set; }

        [Required(ErrorMessage = CityRequired)]
        public City City { get; set; }

        [Required(ErrorMessage = BirthDateRequired)]
        [CustomDateFormatAttribute(ErrorMessage = WrongFormatBirthDate)]
        public string BirthDate { get; set; }

        public string GitHubAccount { get; set; }

        [Url(ErrorMessage = WrongFormatPhoto)]
        public string Photo { get; set; }

        [Required(ErrorMessage = PhoneNumberRequired)]
        public string PhoneNumber { get; set; }
    }
}