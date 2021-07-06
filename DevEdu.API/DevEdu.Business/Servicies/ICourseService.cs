namespace DevEdu.Business.Servicies
{
    public interface ICourseService
    {
        void AddTagToTopic(int topicId, int tagId);
        void DeleteTagFromTopic(int topicId, int tagId);
    }
}