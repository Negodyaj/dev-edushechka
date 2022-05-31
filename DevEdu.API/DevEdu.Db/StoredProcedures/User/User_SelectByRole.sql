CREATE PROCEDURE dbo.User_SelectByRole
	@RoleId int
AS
BEGIN
	SELECT
		t.Id,
		t.FirstName,
		t.LastName,
		t.Email,
		t.Photo,
		t.GroupId Id
	from (
	select distinct
	    U.Id,
		U.FirstName,
		U.LastName,
		U.Email,
		U.Photo,
		ug.GroupId
	FROM dbo.[User] U
	inner join dbo.User_Role ur on ur.UserId = u.Id
	left join dbo.[User_Group] UG on UG.[UserId] = U.Id
	WHERE (ur.RoleId = @RoleId)
	) t
	order by t.LastName asc, t.GroupId desc
END