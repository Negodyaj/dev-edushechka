CREATE PROCEDURE [dbo].[Group_Material_Insert]
	@Material int,
	@GroupId int
AS
BEGIN
	INSERT dbo.Group_Material (MaterialId, GroupId)
	VALUES(@Material, @GroupId)
END
