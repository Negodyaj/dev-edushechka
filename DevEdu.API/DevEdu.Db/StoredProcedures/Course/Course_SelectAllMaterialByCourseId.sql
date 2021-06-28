CREATE PROCEDURE [dbo].[Course_SelectAllMaterialByCourseId]
	  @Id int
AS
  SELECT 
  C.Id, C.Name, C.Description,
  M.Id as MaterialId, M.Content
  FROM [Course] as C
  LEFT JOIN [Course_Material] as CM on CM.CourseId=C.Id
  LEFT JOIN [Material] as M on M.Id=CM.MaterialId
  WHERE (C.[Id] = @Id AND M.[IsDeleted]=0)