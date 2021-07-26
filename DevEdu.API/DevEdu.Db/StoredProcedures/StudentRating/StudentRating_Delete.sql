CREATE PROCEDURE dbo.StudentRating_Delete
	@Id int
AS
BEGIN
	DELETE dbo.StudentRating
	WHERE Id = @Id
END