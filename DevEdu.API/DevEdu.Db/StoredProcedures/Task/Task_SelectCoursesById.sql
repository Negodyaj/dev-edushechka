CREATE PROCEDURE dbo.Task_SelectCoursesByTaskId
	@Id int
AS
BEGIN
	SELECT
		c.Id,
		c.Name,
		c.Description,
		c.IsDeleted
	From dbo.Task t
		left join dbo.Course_Task ct on ct.TaskId = t.Id
		left join dbo.Course c on c.Id = ct.CourseId
	WHERE 
	t.Id = @Id
END