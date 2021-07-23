CREATE PROCEDURE [dbo].User_Group_SelectAllByUserId
	@UserId int
AS
BEGIN
	SELECT 
		ug.Id,
		ug.GroupId,
		ug.UserId,
		ug.RoleId,
		g.Id,
		g.Name,
		g.StartDate,
		g.IsDeleted,
		g.GroupStatusId as id
	FROM dbo.[User_Group] ug
	inner join dbo.[Group] g on ug.GroupId=g.Id
	WHERE (ug.UserId = @UserId and g.IsDeleted=0)
END