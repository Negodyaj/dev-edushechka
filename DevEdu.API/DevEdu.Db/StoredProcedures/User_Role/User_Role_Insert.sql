CREATE PROCEDURE [dbo].[User_Role_Insert]
	@UserId int,
	@RoleId int
AS
BEGIN
	INSERT into dbo.[User_Role] ([UserId],[RoleId])
	VALUES (@UserId,@RoleId)
END