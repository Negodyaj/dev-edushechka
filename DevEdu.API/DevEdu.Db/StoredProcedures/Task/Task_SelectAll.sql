CREATE PROCEDURE dbo.Task_SelectAll
AS
BEGIN
	SELECT
		t.Id,
		t.Name,
		t.StartDate,
		t.EndDate,
		t.Description,
		t.Links,
		t.IsRequired,
		tg.Id,
		tg.Name
	From dbo.Task t
		left join dbo.Tag_Task tgt on tgt.TaskId = t.Id
		left join dbo.Tag tg on tg.Id = tgt.TagId
	WHERE t.IsDeleted = 0
END