using System.Collections.Generic;

namespace DevEdu.Business.ValidationHelpers
{
    public interface ICourseValidationHelper
    {
        void CheckCourseExistence(int courseId);
    }
}