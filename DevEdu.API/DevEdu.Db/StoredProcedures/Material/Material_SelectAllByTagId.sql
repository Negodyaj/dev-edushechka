CREATE PROCEDURE dbo.Material_SelectAllByTagId
	@TagId int
AS
BEGIN
	SELECT 
		m.Id, 
		m.Content, 
		m.IsDeleted
	FROM dbo.Material m
		left join dbo.Tag_Material tm on tm.MaterialId = m.Id
		left join dbo.Tag t on t.Id = tm.TagId
	WHERE (tm.TagId = @TagId AND m.IsDeleted=0 AND t.IsDeleted = 0)
END