CREATE PROCEDURE dbo.Task_SelectById
	@Id int
AS
BEGIN
	SELECT
		t.Id,
		t.Name,
		t.Description,
		t.Links,
		t.IsRequired,
		tg.Id,
		tg.Name
	From dbo.Task t
		left join dbo.Tag_Task tgt on tgt.TaskId = t.Id
		left join dbo.Tag tg on tg.Id = tgt.TagId
	WHERE 
	t.Id = @Id
END