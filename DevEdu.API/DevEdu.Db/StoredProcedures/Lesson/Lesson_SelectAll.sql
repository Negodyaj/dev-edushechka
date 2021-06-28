CREATE PROCEDURE [dbo].[Lesson_SelectAll]
AS
	SELECT * FROM [Lesson]
	WHERE [IsDeleted] = 0
