CREATE PROCEDURE dbo.Student_Homework_Insert
	@HomeworkId int,
	@StudentId int,
	@Answer nvarchar(500),
	@Rating int
AS
BEGIN
	DECLARE @Status_ToReview int = 1
	INSERT INTO dbo.Student_Homework (HomeworkId, StudentId, StatusId, Answer, Rating)
	VALUES (@HomeworkId, @StudentId, @Status_ToReview, @Answer, @Rating)
	SELECT @@IDENTITY
END