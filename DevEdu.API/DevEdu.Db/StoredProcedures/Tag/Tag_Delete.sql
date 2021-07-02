CREATE PROCEDURE [dbo].[Tag_Delete]
	@ID int
AS
	UPDATE [Tag] 
	SET 
	IsDeleted = 1
	Where Id = @ID