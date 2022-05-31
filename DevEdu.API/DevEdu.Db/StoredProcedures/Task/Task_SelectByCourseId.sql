CREATE PROCEDURE [dbo].[Task_SelectByCourseId]
	@CourseId int
AS
BEGIN
	SELECT 
		t.Id,
		t.Name,
		t.Description,
		t.Links,
		t.IsRequired,
		t.GroupId as Id
	FROM dbo.Task T WITH (NOLOCK)
	LEFT JOIN dbo.Course_Task C WITH (NOLOCK) ON T.Id = C.TaskId
	WHERE (C.CourseId = @CourseId AND T.IsDeleted=0)
END
