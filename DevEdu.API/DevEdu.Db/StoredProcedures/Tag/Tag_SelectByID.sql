CREATE PROCEDURE [dbo].[Tag_SelectByID]
	@ID int
AS
	SELECT * from [dbo].[Tag]
	WHERE 
	IsDeleted = 0 AND
	Id = @ID
