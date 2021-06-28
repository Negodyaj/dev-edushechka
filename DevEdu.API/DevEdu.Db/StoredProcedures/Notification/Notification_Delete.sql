CREATE PROCEDURE [dbo].[Notification_Delete]
		@Id int
AS
    UPDATE [Notification]
    SET
    [IsDeleted] = 1
    WHERE [Id] = @Id
