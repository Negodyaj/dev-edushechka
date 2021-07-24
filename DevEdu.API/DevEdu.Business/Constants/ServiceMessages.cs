namespace DevEdu.Business.Constants
{
    public static class ServiceMessages
    {
        public const string EntityNotFoundMessage = "{0} with id = {1} was not found";

        public const string TaskNotFoundMessage = "task with id = {0} was not found";

        public const string TopicNotFoundMessage = "topic with id = {0} was not found";

        public const string TagNotFoundMessage = "tag with id = {0} was not found";

        public const string UserNotRelatedToTaskMessage = "user with id = {0} doesn't relate to task with id = {1}";

        public const string NotFounAnyTaskMessage = "not found any task";
    }
}