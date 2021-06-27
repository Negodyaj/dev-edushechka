CREATE PROCEDURE [dbo].[Student_Lesson_SellectByID]
    @UserId int,
    @LessonId int
AS
 	SELECT * FROM [Student_Lesson]
    WHERE [UserId] = @UserId AND [LessonId] = @LessonId