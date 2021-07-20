using System;
using System.Collections.Generic;

namespace DevEdu.DAL.Models
{
    public class CommentDto : BaseDto
    {
        public string Text { get; set; }
        public UserDto User { get; set; }
        public DateTime Date { get; set; }

        public override bool Equals(object obj)
        {
            return obj is CommentDto dto &&
                   Id == dto.Id &&
                   IsDeleted == dto.IsDeleted &&
                   Text == dto.Text &&
                   EqualityComparer<UserDto>.Default.Equals(User, dto.User) &&
                   Date == dto.Date;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, IsDeleted, Text, User, Date);
        }
    }
}