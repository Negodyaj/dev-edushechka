CREATE PROCEDURE [dbo].[User_Group_Insert]
	@GroupId int,
	@UserId int,
	@RoleId int
AS
	INSERT INTO User_Group (GroupId, UserId, RoleId)
	VALUES (@GroupId, @UserId, @RoleId)
	SELECT @@IDENTITY
