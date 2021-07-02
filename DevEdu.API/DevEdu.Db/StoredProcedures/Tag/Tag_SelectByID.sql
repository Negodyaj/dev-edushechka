CREATE PROCEDURE [dbo].[Tag_SelectByID]
	@ID int
AS
	SELECT Id, Name 
	from [dbo].[Tag]
	WHERE 
	Id = @ID
