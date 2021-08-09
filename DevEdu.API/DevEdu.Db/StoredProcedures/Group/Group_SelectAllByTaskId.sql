CREATE PROCEDURE dbo.Group_SelectAllByTaskId
	@TaskId int
AS
BEGIN
	SELECT 
		g.Id,
		g.Name,
		g.StartDate,
		g.IsDeleted,
		g.GroupStatusId as id
	FROM dbo.[Group] g
	inner join dbo.[Group_Task] gt on gt.GroupId=g.Id
	WHERE (gt.TaskId = @TaskId and g.IsDeleted=0)
END