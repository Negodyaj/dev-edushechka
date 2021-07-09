using System;

namespace DevEdu.DAL.Models
{
    public class CommentDto : BaseDto
    {
        public string Text { get; set; }
        public UserDto User { get; set; }
        public string Date { get; set; }

        public override bool Equals(object obj)
        {
            return obj is CommentDto dto &&
                   Id == dto.Id &&
                   //   IsDeleted == dto.IsDeleted &&
                   User == dto.User &&
                   Text == dto.Text;
        }
    }
}