CREATE PROCEDURE dbo.Course_SelectByMaterialId
	@Id int
AS
BEGIN
	SELECT
		c.Id,
		c.Name
	FROM dbo.Course_Material cm
		left join dbo.Course c on c.Id = cm.CourseId
	WHERE 
		cm.MaterialId = @Id AND c.IsDeleted = 0
END
