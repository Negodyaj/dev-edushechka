CREATE PROCEDURE [dbo].[Tag_Material_Delete]
	@TagID int ,
	@MaterialID int
AS
	Delete from [dbo].[Tag_Material]
	Where TagID = @TagID and MaterialID = @MaterialID

