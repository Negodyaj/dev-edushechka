CREATE PROCEDURE dbo.Student_Lesson_Delete
	@UserId int,
    @LessonId int
AS
BEGIN
    DELETE  FROM dbo.Student_Lesson      
    WHERE UserId = @UserId AND LessonId = @LessonId
END