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
	inner join dbo.[Homework] h on h.GroupId=g.Id
	WHERE (h.TaskId = @TaskId and g.IsDeleted=0)
END