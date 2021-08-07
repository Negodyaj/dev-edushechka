CREATE PROCEDURE dbo.Homework_SelectAllByGroupId
	@GroupId int
AS
BEGIN
	SELECT 
		h.Id,
		h.StartDate,
		h.EndDate,
		t.Id,
		t.Name,
		t.Description,
		t.Links,
		t.IsRequired,
		t.IsDeleted
	FROM dbo.Homework h
	inner join Task t on h.TaskId=t.Id
	WHERE (h.GroupId = @GroupId and t.IsDeleted=0)
END