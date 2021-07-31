CREATE PROCEDURE dbo.Group_SelectAllByUserId
	@UserId int
AS
BEGIN
	SELECT 
		g.Id,
		g.Name,
		g.StartDate,
		g.IsDeleted,
		g.GroupStatusId as id
	FROM dbo.[Group] g
	inner join dbo.[User_Group] ug on ug.GroupId=g.Id
	WHERE (ug.UserId = @UserId and g.IsDeleted=0)
END