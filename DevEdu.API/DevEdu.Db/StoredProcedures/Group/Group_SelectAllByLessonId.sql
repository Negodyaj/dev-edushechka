CREATE PROCEDURE dbo.Group_SelectAllByLessonId
	@LessonId int
AS
BEGIN
	SELECT 
		g.Id,
		g.Name,
		g.StartDate,
		g.IsDeleted,
		g.GroupStatusId as id
	FROM dbo.[Group] g
	inner join dbo.[Group_Lesson] gl on gl.GroupId=g.Id
	WHERE (gl.LessonId = LessonId and g.IsDeleted=0)
END