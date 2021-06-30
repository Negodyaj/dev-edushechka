CREATE PROCEDURE [dbo].[Lesson_Delete]
	@Id int
AS
BEGIN
    UPDATE dbo.Lesson
    SET
    IsDeleted = 1
    WHERE Id = @Id
END