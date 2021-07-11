CREATE PROCEDURE dbo.Task_SelectCoursesById
	@Id int
AS
BEGIN
	SELECT
		c.Name,
		c.Description
	From dbo.Task t
		left join dbo.Course_Task ct on ct.TaskId = t.Id
		left join dbo.Course c on c.Id = ct.CourseId
	WHERE 
	t.Id = @Id
END