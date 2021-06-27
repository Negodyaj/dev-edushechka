CREATE PROCEDURE [dbo].[Comment_Insert]
	@Text nvarchar(max)
AS
	INSERT INTO [Comment] ([Text])
	VALUES (@Text)
	SELECT @@IDENTITY