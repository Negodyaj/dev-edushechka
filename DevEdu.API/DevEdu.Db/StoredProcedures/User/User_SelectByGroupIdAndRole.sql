CREATE PROCEDURE dbo.User_SelectByGroupIdAndRole
	@GroupId int,
	@RoleId int
AS
BEGIN
	SELECT 
	    U.Id,
		U.FirstName,
		U.LastName,
		U.Email,
		U.Photo
	FROM dbo.[User] U
	inner join dbo.[User_Group] UG on UG.[UserId] = U.Id
	WHERE (UG.GroupId = @GroupId AND UG.RoleId = @RoleId)
END