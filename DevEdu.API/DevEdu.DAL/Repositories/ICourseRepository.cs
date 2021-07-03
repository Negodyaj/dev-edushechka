namespace DevEdu.DAL.Repositories
{
    public interface ICourseRepository
    {
        void AddTagToTopic(int topicId, int tagId);
        void DeleteTagFromTopic(int topicId, int tagId);
    }
}