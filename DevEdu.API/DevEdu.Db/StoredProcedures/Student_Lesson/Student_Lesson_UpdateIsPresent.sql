CREATE PROCEDURE [dbo].[Student_Lesson_UpdateIsPresent]
    @UserId int,
    @LessonId int,
    @IsPresent bit
	
AS
    UPDATE [Student_Lesson]
    SET
    [IsPresent] = @IsPresent  
    WHERE [UserId] = @UserId AND [LessonId] = @LessonId