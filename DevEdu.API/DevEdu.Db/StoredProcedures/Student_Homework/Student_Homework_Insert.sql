CREATE PROCEDURE dbo.Student_Homework_Insert
	@HomeworkId int,
	@StudentId int,
	@Answer nvarchar(500)
AS
BEGIN
	DECLARE @Status_ToReview int = 1
	INSERT INTO dbo.Student_Homework (HomeworkId, StudentId, StatusId, Answer)
	VALUES (@HomeworkId, @StudentId, @Status_ToReview, @Answer)
	SELECT @@IDENTITY
END