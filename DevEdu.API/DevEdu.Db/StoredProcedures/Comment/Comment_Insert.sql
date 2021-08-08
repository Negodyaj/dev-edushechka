CREATE PROCEDURE dbo.Comment_Insert
	@UserId			int,
	@LessonId		int null,
	@TaskStudentId  int null,
	@Text			nvarchar(max)
AS
BEGIN
	INSERT INTO dbo.Comment (UserId, LessonId, TaskStudentId, [Text], [Date])
	VALUES (@UserId, @LessonId, @TaskStudentId, @Text, GETDATE())
	SELECT @@IDENTITY
END