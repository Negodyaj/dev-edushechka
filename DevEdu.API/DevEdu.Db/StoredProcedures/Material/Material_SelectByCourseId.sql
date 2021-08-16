CREATE PROCEDURE [dbo].[Material_SelectByCourseId]
	@CourseId int
AS
BEGIN
	SELECT 
	 M.Id,
	 M.Content
	FROM dbo.Material M WITH (NOLOCK)
	LEFT JOIN dbo.Course_Material C WITH (NOLOCK) ON M.Id = C.MaterialId
	WHERE (C.CourseId = @CourseId AND M.IsDeleted=0)
END
