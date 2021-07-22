CREATE PROCEDURE dbo.Course_SelectById
	@Id int
AS
BEGIN
	SELECT 
	   C.Id
      ,C.Name
      ,C.Description
      ,C.IsDeleted
	  ,G.Id
	  ,G.Timetable
	  

  FROM [dbo].[Course] C WITH (NOLOCK)

  LEFT JOIN [Group] G WITH (NOLOCK) ON C.Id = G.CourseId
	WHERE (C.Id = @Id)
END