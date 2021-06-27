CREATE PROCEDURE [dbo].[Tag_Insert]
	@Name nvarchar(50)
AS
	INSERT INTO [Tag] (Name)
	VALUES (@Name)
	SELECT @@IDENTITY