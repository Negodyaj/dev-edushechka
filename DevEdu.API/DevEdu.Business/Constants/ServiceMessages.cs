namespace DevEdu.Business.Constants
{
    public static class ServiceMessages
    {
        public const string EntityNotFoundMessage = "{0} with id = {1} was not found";
        public const string SamePositionsInCourseTopics = "the same positions of topics in the course";
        public const string SameTopicsInCourseTopics = "the same topics in the course";
        public const string EntityNotFound = "Entity Not Found";
        public const string PaymentDeleted = "This payment is deleted";
        public const string UserWithRoleDoesntBelongToGroup = "{0} with id {1} doesn`t belong to group with id {2}";
        public const string UserWithRoleDoesntAuthorizeToGroup = "User {0} is not authorized to do any actions with group {1} as {2}";
        public const string UserDoesntBelongToGroup = "User with id {0} doesn`t belong to group with id {1}";
        public const string UserDoesntHaveRole = "User {0} doesn`t have role {1}";
    }
}