CREATE PROCEDURE [dbo].[Task_Student_Insert]
	@TaskId int,
	@StudentId int
	-- + Answer
AS
BEGIN
	DECLARE @Status_ToReview int = 1
	INSERT INTO Task_Student ([TaskId], [StudentId], [StatusId])
	VALUES (@TaskId, @StudentId, @Status_ToReview)
	SELECT @@IDENTITY
END