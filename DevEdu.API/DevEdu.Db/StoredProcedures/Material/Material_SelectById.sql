CREATE PROCEDURE dbo.Material_SelectById
	@Id int
AS
BEGIN
	SELECT 
		m.Id, 
		m.Content, 
		m.IsDeleted,
		t.Id,
		t.Name,
		c.Id,
		c.Name
	FROM dbo.Material m
		left join dbo.Tag_Material tm on tm.MaterialId = m.Id
		left join dbo.Tag t on t.Id = tm.TagId
		left join dbo.Course_Material cm on cm.MaterialId = m.Id
		left join dbo.Course c on c.Id = cm.CourseId
	WHERE m.Id = @Id
END