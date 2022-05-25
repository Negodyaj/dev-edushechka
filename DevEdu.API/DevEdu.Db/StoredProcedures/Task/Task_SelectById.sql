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
		t.GroupId,
		c.CourseId as Id
	From dbo.Task t LEFT JOIN dbo.Course_Task C WITH (NOLOCK) ON T.Id = C.TaskId
	WHERE t.Id = @Id
END