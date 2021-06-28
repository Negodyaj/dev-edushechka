CREATE PROCEDURE [dbo].[Course_SelectAllTopicByCourseId]
	  @Id int
AS
  SELECT 
  * FROM [Course] as C
  LEFT JOIN [Course_Topic] as CT on CT.CourseId=C.Id
  LEFT JOIN [Topic] as T on T.Id=CT.TopicId
  WHERE (C.[Id] = @Id AND T.[IsDeleted]=0)