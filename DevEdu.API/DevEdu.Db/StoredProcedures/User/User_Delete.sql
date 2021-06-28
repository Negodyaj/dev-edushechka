CREATE PROCEDURE [dbo].[User_Delete]
	@Id int
AS
    UPDATE [User]
    SET
    [IsDeleted] = 1
    WHERE [Id] = @Id