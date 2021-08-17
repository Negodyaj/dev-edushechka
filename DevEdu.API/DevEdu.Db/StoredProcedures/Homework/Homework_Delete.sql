CREATE PROCEDURE dbo.Homework_Delete
	@Id int
AS
BEGIN
	DELETE FROM dbo.Homework
	WHERE Id = @Id
END