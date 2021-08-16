using System.Collections.Generic;

namespace DevEdu.Business.ValidationHelpers
{
    public interface ITagValidationHelper
    {
        void CheckTagExistence(int tagId);
    }
}