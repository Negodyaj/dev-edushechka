CREATE PROCEDURE dbo.Material_SelectAllByTagId
	@TagId int
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
	WHERE m.Id in (
		SELECT
			m.Id
		FROM dbo.Material m
			left join dbo.Tag_Material tm on tm.MaterialId = m.Id
		WHERE tm.TagId = @TagId
	) AND m.IsDeleted=0 AND t.IsDeleted = 0
END