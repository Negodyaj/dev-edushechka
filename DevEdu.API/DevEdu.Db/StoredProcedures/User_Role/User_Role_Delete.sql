CREATE PROCEDURE  dbo.User_Role_Delete
	@UserId int,
	@RoleId int
AS	
BEGIN
	DELETE FROM dbo.User_Role
	WHERE  UserId=@UserId AND RoleId=@RoleId
END