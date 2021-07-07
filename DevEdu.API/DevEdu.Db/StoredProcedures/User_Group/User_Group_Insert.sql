CREATE PROCEDURE [dbo].[User_Group_Insert]
	@GroupId int,
	@UserId int,
	@RoleId int
AS
BEGIN
	INSERT INTO [dbo].[User_Group] (GroupId, UserId, RoleId)
	VALUES (@GroupId, @UserId, @RoleId)
END
