CREATE PROCEDURE dbo.Comment_Insert
	@UserId				int,
	@StudentHomeworkId  int,
	@Text				nvarchar(max)
AS
BEGIN
	INSERT INTO dbo.Comment (UserId, StudentHomeworkId, [Text], [Date])
	VALUES (@UserId, @StudentHomeworkId, @Text, GETDATE())
	SELECT @@IDENTITY
END