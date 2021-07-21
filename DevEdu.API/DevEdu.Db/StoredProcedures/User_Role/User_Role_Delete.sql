CREATE PROCEDURE [dbo].[User_Role_Delete]
	@UserId int,
	@RoleId int
AS
BEGIN
	DELETE [dbo].[User_Role]
	WHERE	UserId = @UserId 
	and		RoleId = @RoleId
END
