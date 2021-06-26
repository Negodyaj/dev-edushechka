CREATE PROCEDURE [dbo].[Comment_Delete]
	@Id int
AS
    UPDATE [Comment]
    SET
    [IsDeleted] = 1
    WHERE [Id] = @Id