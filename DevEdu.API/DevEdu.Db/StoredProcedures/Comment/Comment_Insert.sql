CREATE PROCEDURE dbo.Comment_Insert
	@UserId				int,
	@LessonId			int null,
	@StudentHomeworkId  int null,
	@Text				nvarchar(max)
AS
BEGIN
	INSERT INTO dbo.Comment (UserId, LessonId, StudentHomeworkId, [Text], [Date])
	VALUES (@UserId, @LessonId, @StudentHomeworkId, @Text, GETDATE())
	SELECT @@IDENTITY
END