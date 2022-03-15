CREATE PROCEDURE [dbo].[Material_SelectByCourseId] 
    @CourseId int
AS
BEGIN
    SELECT m.Id,
           m.Content,
           m.Link
    FROM dbo.Material m WITH (NOLOCK)
             LEFT JOIN dbo.Course c WITH (NOLOCK) ON m.CourseId = c.Id
    WHERE (c.Id = @CourseId AND m.IsDeleted = 0)
END
