CREATE PROCEDURE [dbo].[Course_SelectAllGroupByCourseId]
	  @Id int
AS
  SELECT 
  * FROM [Course] as C
  LEFT JOIN [Group] as G on G.CourseId = C.Id
  LEFT JOIN [GroupStatus] as GS on GS.Id=G.GroupStatusId
  WHERE (C.[Id] = @Id AND G.[IsDeleted]=0)