namespace DevEdu.Business.Services
{
    public interface ICourseService
    {
        void AddTagToTopic(int topicId, int tagId);
        void DeleteTagFromTopic(int topicId, int tagId);
    }
}