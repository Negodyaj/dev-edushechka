CREATE PROCEDURE [dbo].[Student_Lesson_Delete]
	@UserId int,
    @LessonId int
AS
    DELETE  FROM [Student_Lesson]      
    WHERE [UserId] = @UserId AND [LessonId] = @LessonId