CREATE PROCEDURE [dbo].[Lesson_SelectById]
	@Id int
AS
BEGIN
	SELECT Date, TeacherComment, TeacherId, IsDeleted FROM dbo.Lesson
	WHERE (Id = @Id)
END