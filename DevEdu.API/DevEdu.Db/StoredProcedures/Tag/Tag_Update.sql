CREATE PROCEDURE [dbo].[Tag_Update]
	@ID int,
	@Name nvarchar(50)
AS
	UPDATE [Tag] 
	SET 
	Name = @Name
	Where Id = @ID
