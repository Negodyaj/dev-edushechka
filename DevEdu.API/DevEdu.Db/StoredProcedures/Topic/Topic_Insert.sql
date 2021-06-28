CREATE PROCEDURE [dbo].[Topic_Insert]
	@Name nvarchar(255),
	@Duration int
AS
	INSERT INTO [Topic] ([Name],[Duration])
	VALUES (@Name, @Duration)
	SELECT @@IDENTITY