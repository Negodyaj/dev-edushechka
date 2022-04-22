CREATE PROCEDURE dbo.Student_Homework_UpdateStatusId
	@Id int,
	@StatusId int,
	@CompletedDate datetime = null,
	@Rating int
AS
BEGIN
	UPDATE Student_Homework
	SET 
		StatusId = @StatusId,
		CompletedDate = @CompletedDate,
		Rating=@Rating
	WHERE Id = @Id
END