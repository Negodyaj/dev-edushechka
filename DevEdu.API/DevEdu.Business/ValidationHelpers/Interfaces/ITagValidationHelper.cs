using DevEdu.DAL.Models;
using System.Threading.Tasks;

namespace DevEdu.Business.ValidationHelpers
{
    public interface ITagValidationHelper
    {
        Task<TagDto> GetTagByIdAndThrowIfNotFoundAsync(int tagId);
    }
}