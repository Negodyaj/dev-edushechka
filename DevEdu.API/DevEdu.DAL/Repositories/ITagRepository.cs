namespace DevEdu.DAL.Repositories
{
    public interface ITagRepository
    {
        void DeleteTagFromMaterial(int materialId, int tagId);
        void DeleteTagFromTagTask(int taskId, int tagId);
        int AddTagToMaterial(int materialId, int tagId);
        int AddTagToTagTask(int taskId, int tagId);
    }
}