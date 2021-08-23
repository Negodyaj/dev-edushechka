using DevEdu.DAL.Models;

namespace DevEdu.Business.ValidationHelpers
{
    public interface ITagValidationHelper
    {
        TagDto GetTagByIdAndThrowIfNotFound(int tagId);
    }
}