CREATE PROCEDURE [dbo].[Tag_Material_Delete]
	@TagId int ,
	@MaterialId int
AS
	Delete from [dbo].[Tag_Material]
	Where TagId = @TagId and MaterialId = @MaterialId

