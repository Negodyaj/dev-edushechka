CREATE PROCEDURE dbo.Group_Task_SelectById
	@GroupId int,
	@TaskId int
AS
BEGIN
	SELECT 
		gt.Id,
		gt.StartDate,
		gt.EndDate,
		t.Id,
		t.Name,
		t.Description,
		t.Links,
		t.IsRequired,
		t.IsDeleted,
		g.Id,
		g.[Name],
		g.StartDate,
		g.GroupStatusId as id
	FROM dbo.Group_Task gt
	inner join Task t on gt.TaskId=t.Id
	inner join [Group] g on gt.GroupId=g.Id
	WHERE (gt.GroupId=@GroupId and gt.TaskId=@TaskId)
END