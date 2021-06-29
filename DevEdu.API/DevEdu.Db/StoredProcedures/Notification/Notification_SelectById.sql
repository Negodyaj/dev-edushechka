CREATE PROCEDURE [dbo].[Notification_SelectById]
	@Id		int
AS
BEGIN
	SELECT Id, Text, Date, UserId, RoleId, IsDeleted
	FROM dbo.Notification
	WHERE ([Id] = @Id AND [IsDeleted]=0)
END