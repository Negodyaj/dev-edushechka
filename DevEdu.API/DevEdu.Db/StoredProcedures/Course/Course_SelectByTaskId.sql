CREATE PROCEDURE dbo.Course_SelectByTaskId
	@Id int
AS
BEGIN
	SELECT
		c.Id,
		c.Name,
		c.Description,
		c.IsDeleted
	From dbo.Course_Task ct
		left join dbo.Course c on c.Id = ct.CourseId
	WHERE 
	ct.TaskId = @Id
END