CREATE PROCEDURE dbo.User_Group_SelectByUserIdAndUserRoleAndGroupId
	@UserId int,
	@RoleId int,
	@GroupId int
AS
BEGIN
	SELECT 
		ug.Id
	FROM dbo.User_Group ug
	WHERE 
	UserId = @UserId
	AND RoleId = @RoleId
	AND GroupId = @GroupId
END