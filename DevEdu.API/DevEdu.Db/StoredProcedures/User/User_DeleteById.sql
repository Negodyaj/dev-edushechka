CREATE PROCEDURE [dbo].[User_DeleteById]
	@Id int
AS
    UPDATE [User]
    SET
    [IsDeleted] = 1
    WHERE [Id] = @Id