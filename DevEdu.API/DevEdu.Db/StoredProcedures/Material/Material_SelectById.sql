CREATE PROCEDURE dbo.Material_SelectById
	@Id int
AS
BEGIN
	SELECT 
		m.Id, 
		m.Content,
		x.Id,
		x.Name,
		x.IsDeleted
	FROM dbo.Material m left join
	(
		SELECT
			t.Id,
			t.Name,
			t.IsDeleted,
			tm.MaterialId
		FROM dbo.Tag t 
			inner join dbo.Tag_Material tm on tm.TagId = t.Id
			WHERE t.IsDeleted = 0
	) x on x.MaterialId = m.Id
	WHERE m.Id = @Id AND m.IsDeleted = 0 
END