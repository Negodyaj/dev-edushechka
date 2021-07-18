CREATE PROCEDURE dbo.Material_SelectAll
AS
BEGIN
	SELECT 
		m.Id, 
		m.Content,
		t.Id,
		t.Name 
	FROM dbo.Material m
		left join dbo.Tag_Material tm on tm.MaterialId = m.Id
		left join dbo.Tag t on t.Id = tm.TagId
	WHERE m.IsDeleted = 0 AND t.IsDeleted = 0
	ORDER BY m.Id
END

