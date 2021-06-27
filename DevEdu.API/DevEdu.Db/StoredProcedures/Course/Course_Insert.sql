CREATE PROCEDURE [dbo].[Course_Insert]
	@Name nvarchar(255),
	@Description nvarchar(max)
AS
	INSERT INTO [Course] ([Name],[Description])
	VALUES (@Name, @Description)
	SELECT @@IDENTITY