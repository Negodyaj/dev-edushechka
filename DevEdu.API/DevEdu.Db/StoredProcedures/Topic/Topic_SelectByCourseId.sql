CREATE PROCEDURE [dbo].[Topic_SelectByCourseId]
	@CourseId int
AS
BEGIN
	SELECT 
	T.Id
	,T.Name
	,T.Duration
	,T.IsDeleted 
	,C.Id
	,C.Position
	FROM dbo.Topic T WITH (NOLOCK)
	LEFT JOIN dbo.Course_Topic C WITH (NOLOCK) ON T.Id = C.TopicId
	WHERE (C.CourseId = @CourseId AND T.IsDeleted=0)
END
