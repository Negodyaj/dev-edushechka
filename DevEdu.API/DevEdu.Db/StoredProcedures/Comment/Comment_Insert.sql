CREATE PROCEDURE [dbo].[Comment_Insert]
	@Text nvarchar(max)
AS
BEGIN
	INSERT INTO [Comment] ([Text])
	VALUES (@Text)
	SELECT @@IDENTITY
END