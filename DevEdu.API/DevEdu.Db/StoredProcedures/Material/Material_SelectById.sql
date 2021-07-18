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
		t.IsDeleted
	FROM dbo.Material m
		left join dbo.Tag_Material tm on tm.MaterialId = m.Id
		left join dbo.Tag t on t.Id = tm.TagId
	WHERE m.Id = @Id
END