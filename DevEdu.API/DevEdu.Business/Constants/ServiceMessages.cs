namespace DevEdu.Business.Constants
{
    public static class ServiceMessages
    {
        public const string EntityNotFoundMessage = "{0} with id = {1} was not found";
        public const string EntityNotFoundByUserId = "{0} by user id = {1} was not found";
        public const string UserOnLessonNotFoundMessage = "User with id = {0} not found on lesson with id = {1}";
        public const string UserInGroupNotFoundMessage = "User with id = {0} not found in group with id = {1}";
        public const string UserHasNoAccessMessage = "The user with id = {0} does not have access to the data of this group";
        public const string EntityDoesntHaveAcessMessage = "{0} with id = {1} does not have access to the {2} with id = {3}";
        public const string SamePositionsInCourseTopics = "the same positions of topics in the course";
        public const string SameTopicsInCourseTopics = "the same topics in the course";
        public const string UserHasNoAccessMessage = "The user with id = {0} does not have access to the data of this group";
        public const string EntityNotFound = "Entity Not Found";
        public const string PaymentDeleted = "This payment is deleted";
        public const string UserWithRoleDoesntBelongToGroup = "{0} with id {1} doesn`t belong to group with id {2}";
        public const string UserWithRoleDoesntAuthorizeToGroup = "User {0} is not authorized to do any actions with group {1} as {2}";
        public const string UserDoesntBelongToGroup = "User with id {0} doesn`t belong to group with id {1}";
        public const string UserDoesntHaveRole = "User {0} doesn`t have role {1}";
        public const string MinimumAllowedValueMessage = "{0} less than allowed value = {1}";
        public const string MinimumAllowedValueWithTwoParamsMessage = "{0} or {1} less than allowed value = {2}";
        public const string EntityWithEmailNotFoundMessage = "{0} with email = {1} was not found";
    }
}