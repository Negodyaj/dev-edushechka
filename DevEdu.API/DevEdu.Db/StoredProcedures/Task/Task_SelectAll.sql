CREATE PROCEDURE dbo.Task_SelectAll
AS
BEGIN
	SELECT
		t.Id,
		t.Name,
		t.Description,
		t.Links,
		t.IsRequired
	From dbo.Task t
	WHERE t.IsDeleted = 0
END