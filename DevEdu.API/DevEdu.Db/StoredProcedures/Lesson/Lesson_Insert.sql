CREATE PROCEDURE [dbo].[Lesson_Insert]
	@Date datetime,
	@TeacherId int,
	@LinkToRecord nvarchar(150)
AS
BEGIN
	INSERT INTO dbo.Lesson (Date, TeacherId, LinkToRecord)
	VALUES (@Date, @TeacherId, @LinkToRecord)
	SELECT @@IDENTITY
END