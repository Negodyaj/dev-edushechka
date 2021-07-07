CREATE PROCEDURE [dbo].[Tag_Insert]
	@Name nvarchar(50)
AS
BEGIN
	INSERT INTO [dbo].[Tag] (Name)
	VALUES (@Name)
	SELECT @@IDENTITY
END