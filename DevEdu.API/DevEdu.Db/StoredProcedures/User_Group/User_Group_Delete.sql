CREATE PROCEDURE [dbo].[User_Group_Delete]
	@UserId int,
	@GroupId int
AS
	DELETE [dbo].[User_Group]
	WHERE UserId = @UserId and GroupId = @GroupId
