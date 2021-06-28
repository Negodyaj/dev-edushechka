CREATE PROCEDURE [dbo].[Course_SelectAllTaskByCourseId]
	  @Id int
AS
  SELECT 
  * FROM [Course] as C
  LEFT JOIN [Course_Task] as CT on CT.CourseId=C.Id
  LEFT JOIN [Task] as T on T.Id=CT.TaskId
  WHERE (C.[Id] = @Id AND T.[IsDeleted]=0)