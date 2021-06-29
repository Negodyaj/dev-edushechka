CREATE PROCEDURE dbo.Student_Lesson_UpdateIsPresent
    @UserId int,
    @LessonId int,
    @IsPresent bit
	
AS
BEGIN
    UPDATE dbo.Student_Lesson
    SET
    IsPresent = @IsPresent  
    WHERE UserId = @UserId AND LessonId = @LessonId
END