CREATE PROCEDURE [dbo].[Lesson_Insert]
	@Date datetime,
	@TeacherComment nvarchar(500),
	@TeacherId int
AS
	INSERT INTO [Lesson] ([Date], [TeacherComment], [TeacherId])
	VALUES (@Date, @TeacherComment, @TeacherId)
	SELECT @@IDENTITY
