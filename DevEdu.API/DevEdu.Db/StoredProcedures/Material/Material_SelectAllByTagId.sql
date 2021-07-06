CREATE PROCEDURE dbo.Material_SelectAllByTagId
	@TagId int
AS
BEGIN
	SELECT 
	m.Id, m.Content, m.IsDeleted 
	FROM dbo.Material m
	left join dbo.Tag_Material tm on m.Id = tm.MaterialId
	WHERE (tm.TagId = @TagId AND IsDeleted=0)
END
