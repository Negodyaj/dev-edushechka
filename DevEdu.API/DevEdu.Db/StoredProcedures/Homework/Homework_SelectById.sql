CREATE PROCEDURE dbo.Homework_SelectById
	@Id int
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
		t.IsDeleted,
		g.Id,
		g.Name,
		g.StartDate,
		g.IsDeleted,
		g.GroupStatusId as Id
	FROM dbo.Homework h
	inner join Task t on h.TaskId=t.Id
	inner join [Group] g on h.GroupId=g.Id
	WHERE (h.Id=@Id)
END