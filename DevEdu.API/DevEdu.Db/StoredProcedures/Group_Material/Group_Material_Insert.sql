CREATE PROCEDURE dbo.Group_Material_Insert
	@GroupId		int,
	@MaterialId		int
AS
BEGIN
	INSERT dbo.Group_Material (GroupId, MaterialId)
	VALUES(@GroupId, @MaterialId)
END