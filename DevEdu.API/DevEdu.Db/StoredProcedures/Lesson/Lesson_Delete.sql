CREATE PROCEDURE [dbo].[Lesson_Delete]
	@Id int
AS
    UPDATE [Lesson]
    SET
    [IsDeleted] = 1
    WHERE [Id] = @Id
