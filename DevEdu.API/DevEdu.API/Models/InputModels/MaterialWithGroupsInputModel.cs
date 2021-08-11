﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static DevEdu.API.Common.ValidationMessage;

namespace DevEdu.API.Models.InputModels
{
    public class MaterialWithGroupsInputModel : MaterialWithTagsInputModel
    {
        [MinLength(1, ErrorMessage = GroupsRequired)]
        public List<int> GroupsIds { get; set; }
    }
}