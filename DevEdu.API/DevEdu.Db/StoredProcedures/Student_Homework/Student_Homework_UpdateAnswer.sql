CREATE PROCEDURE dbo.Student_Homework_UpdateAnswer
	@Id int,
	@Answer nvarchar(500)
AS
BEGIN
	UPDATE Student_Homework
	SET 
		Answer = @Answer
	WHERE Id = @Id AND IsDeleted = 0
END