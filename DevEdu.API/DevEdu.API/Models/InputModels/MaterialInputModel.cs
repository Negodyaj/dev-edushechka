﻿using System.ComponentModel.DataAnnotations;
using static DevEdu.API.Common.ValidationMessage;

namespace DevEdu.API.Models
{
    public class MaterialInputModel
    {
        [Required(ErrorMessage = ContentRequired)]
        public string Content { get; set; }

        [MaxLength(200, ErrorMessage = LinkRequired)]
        public string Link { get; set; }
    }
}