CREATE PROCEDURE dbo.Group_Material_Insert
	@MaterialId int,
	@GroupId int
AS
BEGIN
	INSERT dbo.Group_Material (MaterialId, GroupId)
	VALUES(@MaterialId, @GroupId)
END