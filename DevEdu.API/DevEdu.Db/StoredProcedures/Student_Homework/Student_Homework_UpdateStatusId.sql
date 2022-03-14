CREATE PROCEDURE dbo.Student_Homework_UpdateStatusId
	@Id int,
	@StatusId int,
	@CompletedDate datetime = null,
	@AnswerDate datetime = null
AS
BEGIN
	UPDATE Student_Homework
	SET 
		StatusId = @StatusId,
		CompletedDate = @CompletedDate,
		AnswerDate = COALESCE(@AnswerDate, [AnswerDate])
	WHERE Id = @Id
END