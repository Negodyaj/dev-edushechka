CREATE PROCEDURE dbo.Group_Material_Delete
	@GroupId int,
	@MaterialId int
AS
BEGIN
	DELETE FROM dbo.Group_Material
	WHERE GroupId = @GroupId AND MaterialId = @MaterialId
END