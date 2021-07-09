CREATE PROCEDURE dbo.Course_SelectById
	@Id int
AS
BEGIN
	SELECT 
	   C.Id
      ,C.Name
      ,C.Description
      ,C.IsDeleted
	  ,T.Id
	  ,T.Name
	  ,CT.Position
	  ,M.Id
	  ,M.Content
	  ,TS.Id
	  ,TS.Name
	  ,G.Id
	  ,G.Timetable

  FROM [DevEdu].[dbo].[Course] C WITH (NOLOCK)
  LEFT JOIN Course_Topic CT WITH (NOLOCK) ON C.Id = CT.CourseId
  LEFT JOIN Topic T WITH (NOLOCK) ON CT.TopicId = T.Id

  LEFT JOIN Course_Material CM WITH (NOLOCK) ON C.Id = CM.CourseId
  LEFT JOIN Material M WITH (NOLOCK) ON CM.MaterialId = M.Id

  LEFT JOIN Course_Task CTS WITH (NOLOCK) ON CTS.CourseId = C.Id
  LEFT JOIN Task TS WITH (NOLOCK) ON TS.Id =  CTS.TaskId

  LEFT JOIN [Group] G WITH (NOLOCK) ON C.Id = G.CourseId
	WHERE (C.Id = @Id)
END