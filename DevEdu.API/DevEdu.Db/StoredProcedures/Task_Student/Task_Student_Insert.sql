CREATE PROCEDURE dbo.Task_Student_Insert
	@TaskId int,
	@StudentId int,
	@Answer nvarchar(500)
AS
BEGIN
	DECLARE @Status_ToReview int = 1
	INSERT INTO dbo.Task_Student (TaskId, StudentId, StatusId, Answer)
	VALUES (@TaskId, @StudentId, @Status_ToReview, @Answer)
	SELECT @@IDENTITY
END