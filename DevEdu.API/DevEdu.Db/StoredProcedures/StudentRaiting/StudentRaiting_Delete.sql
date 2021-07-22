CREATE PROCEDURE dbo.StudentRaiting_Delete
	@Id int
AS
BEGIN
	DELETE dbo.StudentRaiting
	WHERE Id = @Id
END