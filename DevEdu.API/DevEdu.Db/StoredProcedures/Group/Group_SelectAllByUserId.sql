CREATE PROCEDURE dbo.Group_SelectAllByUserId
	@UserId int
AS
BEGIN
	SELECT 
		g.Id,
		g.Name,
		g.StartDate,
		g.EndDate,
		g.IsDeleted,
		g.GroupStatusId as id,
		c.Id as id,
		c.Name
	FROM dbo.[Group] g
	inner join dbo.[User_Group] ug on ug.GroupId=g.Id
	inner join [Course] c on c.Id = g.CourseId
	WHERE (ug.UserId = @UserId and g.IsDeleted=0)
END
