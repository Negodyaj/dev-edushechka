namespace DevEdu.Business.Services
{
    public interface IMaterialService
    {
        void AddTagToMaterial(int materialId, int tagId);
        void DeleteTagFromMaterial(int materialId, int tagId);
    }
}