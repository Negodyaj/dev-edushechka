CREATE PROCEDURE dbo.Material_Tag_Delete
	@TagId int,
	@MaterialId int
AS
BEGIN
	Delete from dbo.Tag_Material
	Where TagId = @TagId and MaterialId = @MaterialId
END
