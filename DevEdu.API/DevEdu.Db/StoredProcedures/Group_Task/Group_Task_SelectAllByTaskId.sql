CREATE PROCEDURE dbo.Group_Task_SelectAllByTaskId
	@TaskId int
AS
BEGIN
	SELECT 
		gt.Id,
		gt.StartDate,
		gt.EndDate,
		g.Id,
		g.Name,
		g.StartDate,
		g.GroupStatusId as id
	FROM dbo.Group_Task gt
	inner join [Group] g on gt.GroupId=g.Id
	WHERE (gt.TaskId = @TaskId and g.IsDeleted=0)
END