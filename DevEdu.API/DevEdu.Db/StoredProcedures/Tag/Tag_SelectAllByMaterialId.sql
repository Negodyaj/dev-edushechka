CREATE PROCEDURE dbo.Tag_SelectAllByMaterialId
	@MaterialId int
AS
BEGIN
	SELECT 
		t.Id,
		t.Name
	FROM dbo.Tag t
		left join dbo.Tag_Material tm on tm.TagId = t.Id
	WHERE tm.MaterialId = @MaterialId AND t.IsDeleted = 0
END