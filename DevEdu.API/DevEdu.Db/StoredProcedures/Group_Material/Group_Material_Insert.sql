CREATE PROCEDURE [dbo].[Group_Material_Insert]
	@Material int,
	@GroupId int
AS
	INSERT [dbo].[Group_Material]
	OUTPUT INSERTED.ID, INSERTED.GroupId, INSERTED.MaterialId
	VALUES(
	 @Material
    ,@GroupId
	)
