CREATE PROCEDURE [dbo].[Notification_SelectAllByUserId]
@UserId int
AS
	SELECT * FROM [Notification]
	WHERE ([UserId] = @UserId AND [IsDeleted]=0)