CREATE PROCEDURE [dbo].[Task_SelectById]
	@ID int
AS
	SELECT * from [dbo].[Task]
	WHERE 
	IsDeleted = 0 AND
	Id = @ID