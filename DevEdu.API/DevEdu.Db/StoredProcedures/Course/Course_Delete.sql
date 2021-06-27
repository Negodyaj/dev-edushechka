CREATE PROCEDURE [dbo].[Course_Delete]
	@Id int
AS
    UPDATE [Course]
    SET
    [IsDeleted] = 1
    WHERE [Id] = @Id