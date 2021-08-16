CREATE PROCEDURE [dbo].[Tag_SelectByID]
	@Id int
AS
BEGIN
	SELECT 
		Id,
		Name,
		IsDeleted
	FROM Tag
	WHERE Id = @Id
END