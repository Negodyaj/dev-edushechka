CREATE PROCEDURE [dbo].[Lesson_Insert]
	@Date datetime,
	@TeacherComment nvarchar(500),
	@TeacherId int,
	@LinkToRecord nvarchar(150)
AS
BEGIN
	INSERT INTO dbo.Lesson (Date, TeacherComment, TeacherId, LinkToRecord)
	VALUES (@Date, @TeacherComment, @TeacherId, @LinkToRecord)
	SELECT @@IDENTITY
END