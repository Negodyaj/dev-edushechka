CREATE PROCEDURE [dbo].[Notification_SelectById]
		@Id int
AS
	SELECT * FROM [Notification]
	WHERE ([Id] = @Id AND [IsDeleted]=0)