CREATE PROCEDURE [dbo].[Task_Delete]
	@Id int
AS
	UPDATE [Task]
    SET
    [IsDeleted] = 1
    WHERE [Id] = @Id