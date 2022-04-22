CREATE PROCEDURE dbo.Student_Homework_UpdateAnswer
	@Id int,
	@Answer nvarchar(500),
	@Status int
AS
BEGIN
	UPDATE Student_Homework
	SET 
		Answer = @Answer,
		AnswerDate = getdate(),
		StatusId = @Status
	WHERE Id = @Id AND IsDeleted = 0
END