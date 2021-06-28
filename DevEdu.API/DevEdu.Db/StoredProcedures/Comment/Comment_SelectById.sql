CREATE PROCEDURE dbo.Comment_SelectById
	@Id int
AS
BEGIN
	SELECT 
	Id, UserId, Text, Date, IsDeleted
	FROM dbo.Comment
	WHERE (Id = @Id)
END