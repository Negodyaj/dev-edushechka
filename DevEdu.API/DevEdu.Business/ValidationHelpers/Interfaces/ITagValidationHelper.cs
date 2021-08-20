using DevEdu.DAL.Models;

namespace DevEdu.Business.ValidationHelpers
{
    public interface ITagValidationHelper
    {
        TagDto CheckTagExistence(int tagId);
    }
}