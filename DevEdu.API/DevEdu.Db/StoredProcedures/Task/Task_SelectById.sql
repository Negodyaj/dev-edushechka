CREATE PROCEDURE dbo.Task_SelectById
	@Id int
AS
BEGIN
	SELECT
		t.Id,
		t.Name,
		t.Description,
		t.Links,
		t.IsRequired
	From dbo.Task t
	WHERE 
	t.Id = @Id
END