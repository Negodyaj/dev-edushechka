namespace DevEdu.Business.Servicies
{
    public interface ICourseService
    {
        int AddCourseMaterialReference(int courseId, int materialId);
        int RemoveCourseMaterialReference(int courseId, int materialId);
    }
}