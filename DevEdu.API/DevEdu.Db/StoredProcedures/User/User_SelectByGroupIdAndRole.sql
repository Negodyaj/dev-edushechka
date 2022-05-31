CREATE PROCEDURE dbo.User_SelectByGroupIdAndRole
	@GroupId int = NULL,
	@RoleId int
AS
BEGIN
	SELECT 
	    U.Id,
		U.FirstName,
		U.LastName,
		U.Email,
		U.Photo,
		u.Id as Id
	FROM dbo.[User] U
	left join dbo.[User_Group] UG on UG.[UserId] = U.Id
	WHERE ((@GroupId is not null and UG.GroupId = @GroupId or @GroupId is null) AND UG.RoleId = @RoleId)
	order by u.LastName asc, u.id desc
END