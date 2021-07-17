CREATE PROCEDURE dbo.Course_SelectAll
AS
BEGIN
	SELECT 
	   C.Id
      ,C.Name
      ,C.Description
      ,C.IsDeleted
	  ,G.CourseId
	  ,G.Timetable
	  

  FROM [DevEdu].[dbo].[Course] C WITH (NOLOCK)

  LEFT JOIN [Group] G WITH (NOLOCK) ON C.Id = G.CourseId
	WHERE (C.IsDeleted = 0)
END