CREATE PROCEDURE dbo.Group_Task_SelectAllByGroupId
	@GroupId int
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
		t.IsDeleted
	FROM dbo.Group_Task gt
	inner join Task t on gt.TaskId=t.id
	WHERE (gt.GroupId = @GroupId and t.IsDeleted=0)
END