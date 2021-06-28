CREATE PROCEDURE dbo.Comment_SelectAllByUserId
	@UserId int
AS
BEGIN
	SELECT 
	Id, UserId, Text, Date, IsDeleted
	FROM dbo.Comment
	WHERE (UserId = @UserId AND IsDeleted=0)
END