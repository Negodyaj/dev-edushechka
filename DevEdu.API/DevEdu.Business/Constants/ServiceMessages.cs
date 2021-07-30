namespace DevEdu.Business.Constants
{
    public static class ServiceMessages
    {
        public const string EntityNotFoundMessage = "{0} with id = {1} was not found";
        public const string SamePositionsInCourseTopics = "the same positions of topics in the course";
        public const string SameTopicsInCourseTopics = "the same topics in the course";
        public const string EntityNotFound = "Entity Not Found";
        public const string PaymentDeleted = "This payment is deleted";
        public const string LessonTopicReferenceNotFound = "No reference between lesson with id = {0} and topic with id = {1} was found";
        public const string LessonTopicReferenceAlreadyExists = "Reference between lesson with id = {0} and topic with id = {1} already exists";
        public const string DuplicateCoursesValuesProvided = "Courses with same Ids were provided";
        public const string DuplicateGroupsValuesProvided = "Groups with same Ids were provided";
        public const string DuplicateTagsValuesProvided = "Tags with same Ids were provided";
    }
}