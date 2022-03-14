namespace DevEdu.Business.Constants
{
    public static class ServiceMessages
    {
        public const string EntityNotFoundMessage = "{0} with id = {1} was not found";
        public const string EntityNotFoundByUserId = "{0} by user id = {1} was not found";
        public const string UserInGroupNotFoundMessage = "User with id = {0} not found in group with id = {1}";
        public const string UserHasNoAccessMessage = "The user with id = {0} does not have access to the data of this group";
        public const string UserHasNoAccessGetInfoMessage = "The user with id = {0} does not have access to the data of this user";
        public const string EntityDoesntHaveAcessMessage = "{0} with id = {1} does not have access to the {2} with id = {3}";
        public const string SamePositionsInCourseTopics = "the same positions of topics in the course";
        public const string SameTopicsInCourseTopics = "the same topics in the course";
        public const string EntityNotFound = "Entity Not Found";
        public const string PaymentDeleted = "This payment is deleted";
        public const string LessonTopicReferenceNotFound = "No reference between lesson with id = {0} and topic with id = {1} was found";
        public const string LessonTopicReferenceAlreadyExists = "Reference between lesson with id = {0} and topic with id = {1} already exists";
        public const string DuplicateValuesProvided = "Duplicate Ids for {0} were provided";
        public const string AccessToMaterialDenied = "User with id = {0} doesn't have access to material with id = {1}";
        public const string UserWithRoleDoesntAuthorizeToGroup = "User with id = {0} doesn't belong to group {1} as {2}";
        public const string UserWithRoleDoesntBelongToGroup = "{0} with id {1} doesn`t belong to group with id {2}";
        public const string UserDoesntBelongToGroup = "User with id {0} doesn`t belong to group with id {1}";
        public const string EntityWithEmailNotFoundMessage = "{0} with email = {1} was not found";
        public const string UserAndTeacherAreNotSame = "User with id = {0} and teacher with id = {1} are not the same";
        public const string UserDoesntBelongToLesson = "User with id {0} doesn`t belong to lesson with id {1}";
        public const string AccessToNotificationDenied = "User with id = {0} doesn't have access to this notification ";
        public const string MoreOnePropertyHaveAValueMessage = "Only one property ({0}, {1} or {2}) should have a value";
        public const string WrongPassword = "Wrong password";
        public const string HomeworkStatusCantBeChanged = "Homework status can't be changed";
        public const string HomeworkStatusCantBeChangedByThisUser = "Homework status can't be changed by user with this role";
        public const string HomeworkStatusCantBeChangedOnThisStatus = "Current homework status can't be changed on this status";
    }
}