﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEdu.DAL.Models
{
    public class RaitingTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }

        public override bool Equals(object obj)
        {
            return obj is RaitingTypeDto dto &&
                   Id == dto.Id &&
                   Name == dto.Name &&
                   Weight == dto.Weight;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Weight);
        }
    }
}