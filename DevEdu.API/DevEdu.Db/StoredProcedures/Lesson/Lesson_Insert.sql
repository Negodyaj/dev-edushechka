CREATE PROCEDURE [dbo].[Lesson_Insert]
	@Date datetime,
	@TeacherComment nvarchar(500),
	@TeacherId int
AS
BEGIN
	INSERT INTO [Lesson] ([Date], [TeacherComment], [TeacherId])
	VALUES (@Date, @TeacherComment, @TeacherId)
	SELECT @@IDENTITY
END