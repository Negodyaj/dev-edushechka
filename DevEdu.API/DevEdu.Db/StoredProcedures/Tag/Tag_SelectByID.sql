CREATE PROCEDURE [dbo].[Tag_SelectByID]
	@Id int
AS
BEGIN
	SELECT Id, Name 
	from [dbo].[Tag]
	WHERE 
	Id = @Id
END