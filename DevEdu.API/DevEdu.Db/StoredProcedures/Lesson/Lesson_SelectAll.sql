CREATE PROCEDURE [dbo].[Lesson_SelectAll]
AS
BEGIN
	SELECT Id, Date, TeacherComment, TeacherId FROM dbo.Lesson
	WHERE [IsDeleted] = 0
END