CREATE PROCEDURE [dbo].[Group_Material_Delete]
	@Material int,
	@GroupId int
AS
	DELETE FROM [dbo].[Group_Material]
	OUTPUT DELETED.Id
	WHERE GroupId = @GroupId AND MaterialId = @Material
