CREATE PROCEDURE dbo.Student_Homework_UpdateStatusId
	@Id int,
	@StatusId int,
	@CompletedDate datetime = null
AS
BEGIN
	UPDATE Student_Homework
	SET 
		StatusId = @StatusId,
		CompletedDate = @CompletedDate
	WHERE Id = @Id
END