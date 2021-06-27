CREATE PROCEDURE [dbo].[Topic_Delite]
	@Id int
AS
    UPDATE [Topic]
    SET
    [IsDeleted] = 1
    WHERE [Id] = @Id
