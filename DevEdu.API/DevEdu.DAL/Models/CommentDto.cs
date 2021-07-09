using System;

namespace DevEdu.DAL.Models
{
    public class CommentDto : BaseDto, IEquatable<CommentDto>
    {
        public string Text { get; set; }
        public UserDto User { get; set; }

        //public override bool Equals(object obj)
        //{
        //    return obj is CommentDto dto &&
        //           Id == dto.Id &&
        //        //   IsDeleted == dto.IsDeleted &&
        //           UserId == dto.UserId &&
        //           Text == dto.Text;
        //}

        public bool Equals(CommentDto other)
        {
            return Id == other.Id &&
                //   IsDeleted == dto.IsDeleted &&
                   UserId == other.UserId &&
                   Text == other.Text;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + Id.GetHashCode();
            hash = hash * 23 + UserId.GetHashCode();
            hash = hash * 23 + Text.GetHashCode();
            return hash;
        }
    }
}