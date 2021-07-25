CREATE PROCEDURE dbo.Group_SelectByMaterialId
	@Id int
AS
BEGIN
	SELECT
		g.Id,
		g.Name,
		g.StartDate
	FROM dbo.Group_Material gm
		left join dbo.[Group] g on g.Id = gm.GroupId
	WHERE 
		gm.MaterialId = @Id AND g.IsDeleted = 0 AND g.GroupStatusId = 1
END
