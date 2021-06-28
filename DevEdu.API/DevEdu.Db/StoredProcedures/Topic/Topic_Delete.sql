CREATE PROCEDURE [dbo].[Topic_Delete]
	@Id int
AS
    UPDATE [Topic]
    SET
    [IsDeleted] = 1
    WHERE [Id] = @Id
