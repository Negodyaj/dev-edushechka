CREATE PROCEDURE [dbo].[Tag_SelectByID]
	@Id int
AS
	SELECT Id, Name 
	from [dbo].[Tag]
	WHERE 
	Id = @Id
