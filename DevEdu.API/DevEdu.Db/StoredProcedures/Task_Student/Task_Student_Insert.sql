CREATE PROCEDURE [dbo].[Task_Student_Insert]
	@TaskId int,
	@StudentId int,
	@StatusId int
AS
	INSERT INTO Task_Student ([TaskId], [StudentId], [StatusId])
	VALUES (@TaskId, @StudentId, @StatusId)
	SELECT @@IDENTITY