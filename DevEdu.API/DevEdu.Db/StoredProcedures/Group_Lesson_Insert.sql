CREATE PROCEDURE [dbo].[Group_Lesson_Insert]
	@LessonId int,
	@GroupId int
AS
	INSERT [dbo].[Group_Lesson_Insert]
	OUTPUT INSERTED.ID, INSERTED.GroupId, INSERTED.LessonId
	VALUES(
	 @LessonId
    ,@GroupId
	)