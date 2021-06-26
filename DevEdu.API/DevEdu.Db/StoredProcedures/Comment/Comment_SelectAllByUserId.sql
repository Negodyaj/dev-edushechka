CREATE PROCEDURE [dbo].[Comment_SelectAllByUserId]
	@UserId int
AS
	SELECT * FROM [Comment]
	WHERE ([UserId] = @UserId)