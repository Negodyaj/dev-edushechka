namespace DevEdu.Business.Constants
{
    public static class ServiceMessages
    {
        public const string EntityWithIdNotFoundMessage = "{0} with id = {1} was not found";
        public const string EntityNotFoundByUserIdMessage = "{0} by user id = {1} was not found";
        public const string UserInGroupNotFoundMessage = "User with id = {0} not found in group with id = {1}";
        public const string UserHasNoAccessMessage = "The user with id = {0} does not have access to the data of this group";
        public const string EntityDoesntHaveAccessMessage = "{0} with id = {1} does not have access to the {2} with id = {3}";
        public const string SamePositionsInCourseTopicsMessage = "the same positions of topics in the course";
        public const string SameTopicsInCourseTopicsMessage = "the same topics in the course";
        public const string EntityNotFoundMessage = "Entity Not Found";
        public const string PaymentDeletedMessage = "This payment is deleted";
        public const string LessonTopicReferenceNotFoundMessage = "No reference between lesson with id = {0} and topic with id = {1} was found";
        public const string LessonTopicReferenceAlreadyExistsMessage = "Reference between lesson with id = {0} and topic with id = {1} already exists";
        public const string DuplicateValuesProvidedMessage = "Duplicate Ids for {0} were provided";
        public const string AccessToMaterialDeniedMessage = "User with id = {0} doesn't have access to material with id = {1}";
        public const string UserWithRoleDoesntAuthorizeToGroupMessage = "User with id = {0} doesn't belong to group {1} as {2}";
        public const string UserWithRoleDoesntBelongToGroupMessage = "{0} with id {1} doesn`t belong to group with id {2}";
        public const string UserDoesntBelongToGroupMessage = "User with id {0} doesn`t belong to group with id {1}";
        public const string EntityWithEmailNotFoundMessage = "{0} with email = {1} was not found";
        public const string UserAndTeacherAreNotSameMessage = "User with id = {0} and teacher with id = {1} are not the same";
        public const string UserDoesntBelongToLessonMessage = "User with id {0} doesn`t belong to lesson with id {1}";
        public const string AccessToNotificationDeniedMessage = "User with id = {0} doesn't have access to this notification ";
        public const string MoreOnePropertyHaveAValueMessage = "Only one property ({0}, {1} or {2}) should have a value";
        public const string WrongPasswordMessage = "WrongPassword";
        public const string AdminCanAddRolesToUserMessage = "Only the {0} can add other roles to User";
    }
}