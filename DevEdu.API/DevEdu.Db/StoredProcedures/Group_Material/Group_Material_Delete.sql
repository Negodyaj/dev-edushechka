CREATE PROCEDURE [dbo].[Group_Material_Delete]
	@Material int,
	@GroupId int
AS
BEGIN
	DELETE FROM dbo.Group_Material
	WHERE GroupId = @GroupId AND MaterialId = @Material
END
