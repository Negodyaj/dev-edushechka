CREATE PROCEDURE dbo.Homework_SelectAllByTaskId
	@TaskId int
AS
BEGIN
	SELECT 
		h.Id,
		h.StartDate,
		h.EndDate,
		g.Id,
		g.Name,
		g.StartDate,
		g.IsDeleted,
		g.GroupStatusId as id
	FROM dbo.Homework h
	inner join [Group] g on h.GroupId=g.Id
	WHERE (h.TaskId = @TaskId and g.IsDeleted=0)
END