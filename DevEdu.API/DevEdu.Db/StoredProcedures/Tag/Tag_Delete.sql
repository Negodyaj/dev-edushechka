CREATE PROCEDURE [dbo].[Tag_SoftDelete]
	@ID int
AS
	UPDATE [Tag] 
	SET 
	IsDeleted = 1
	Where Id = @ID