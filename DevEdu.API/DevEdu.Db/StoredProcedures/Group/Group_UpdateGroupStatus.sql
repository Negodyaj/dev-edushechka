CREATE PROCEDURE dbo.Group_UpdateGroupStatus
	@Id int,
	@CourseId int
AS
BEGIN
	UPDATE dbo.[Group]
	SET [CourseId] = @CourseId
	WHERE Id = @Id
END